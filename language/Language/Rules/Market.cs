using Language.ScriptItems;
using System.Collections.Generic;
using System.Linq;

namespace Language.Rules
{
    [ActiveRule]
    public class Market : RuleBase
    {
        public override string Name => "market";

        public override string Help => "Buys/sells based on a condition. Ignores escrow amounts.";

        public override string Usage => "buy/sell RESOURCE_NAME when RESOURCE_NAME COMPARISON AMOUNT";

        public override IEnumerable<string> Examples => new[]
        {
            "buy food when gold > 100",
            "sell wood when wood > 1500",
        };

        public Market()
            : base(@"^(?<action>buy|sell) (?<resource>[^ ]+) when (?<testresource>[^ +]+) (?<comparison>[<>!=]+) (?<amount>[^ ]+)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var action = data["action"].Value;
            var resource = data["resource"].Value;
            var testResource = data["testresource"].Value;
            var comparison = data["comparison"].Value;
            var amount = data["amount"].Value;

            var nonEscrowedResult = CreateNonEscrowedResourceGoals(context, [testResource]);

            var rule = new Defrule(
                [
                    $"up-compare-goal {nonEscrowedResult.GoalToResourceMap.Keys.Single()} c:{comparison} {amount}",
                    $"can-{action}-commodity {resource}",
                ],
                [
                    $"{action}-commodity {resource}",
                ]);
            
            context.AddToScript(context.ApplyStacks(nonEscrowedResult.Rules));
            context.AddToScript(context.ApplyStacks(rule));
        }
    }
}
