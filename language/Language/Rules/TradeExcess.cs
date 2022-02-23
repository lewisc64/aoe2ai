using Language.Extensions;
using Language.ScriptItems;
using System.Collections.Generic;
using System.Linq;

namespace Language.Rules
{
    [ActiveRule]
    public class TradeExcess : RuleBase
    {
        public override string Name => "trade excess";

        public override string Help => "Uses the market to rebalance resources around a certain threshold. Ignores escrowed values.";

        public override string Usage => "trade excess RESOURCE_LIST at AMOUNT";

        public override string Example => @"trade excess food at 2000
trade excess wood and food and gold at 2000";

        public TradeExcess()
            : base(@"^trade excess (?:(?<resourcelist>(?:[^ ]+(?: and (?=\w))?)+) )?at (?<amount>[^ ]+)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var amount = data["amount"].Value;
            var resourceList = data["resourcelist"].Value
                .ReplaceIfNullOrEmpty("wood and food and gold and stone")
                .Split(" and ");

            var rules = new List<Defrule>();

            (var escrowRules, var goalToResourceMap) = CreateNonEscrowedResourceGoals(context, resourceList);

            foreach (var (resourceGoal, resource) in goalToResourceMap)
            {
                if (resource == "gold")
                {
                    continue;
                }

                rules.Add(new Defrule(
                    new[] { $"up-compare-goal {resourceGoal} c:> {amount}", $"can-sell-commodity {resource}" },
                    new[] { $"sell-commodity {resource}" }));
            }

            if (resourceList.Contains("gold"))
            {
                var goldGoal = goalToResourceMap.First(x => x.Value == "gold").Key;

                foreach (var rule in rules)
                {
                    rule.Conditions.Add(new Condition($"up-compare-goal {goldGoal} c:< {amount}"));
                }

                foreach (var (resourceGoal, resource) in goalToResourceMap)
                {
                    if (resource == "gold")
                    {
                        continue;
                    }

                    rules.Add(new Defrule(
                        new[] { $"up-compare-goal {resourceGoal} c:< {amount}", $"up-compare-goal {goldGoal} c:> {amount}", $"can-buy-commodity {resource}" },
                        new[] { $"buy-commodity {resource}" }));
                }
            }

            rules.InsertRange(0, escrowRules);

            context.AddToScript(context.ApplyStacks(rules));

            context.FreeVolatileGoals(goalToResourceMap.Keys);
        }
    }
}
