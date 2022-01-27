using Language.Extensions;
using Language.ScriptItems;
using System.Collections.Generic;
using System.Linq;

namespace Language.Rules
{
    [ActiveRule]
    public class AutoBalance : RuleBase
    {
        public override string Name => "auto balance";

        public override string Help => "Redistributes villagers at a set interval around a threshold. By default it will balance every 60 seconds with a threshold of 300. When getting the resource amounts, it subtracts the escrowed portion.";

        public override string Usage => "auto balance RESOURCES around THRESHOLD every AMOUNT seconds";

        public override string Example => @"auto balance wood and food and gold
auto balance all
auto balance wood and food every 30 seconds";

        public AutoBalance()
            : base(@"^auto balance (?<resourcelist>(?:[^ ]+(?: and (?=\w))?)+)(?: around (?<threshold>[^ ]+))?(?: every (?<time>[^ ]+) seconds)?$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var resources = data["resourcelist"].Value
                .ReplaceIfNullOrEmpty("wood and food and gold")
                .Replace("all", "wood and food and gold and stone")
                .Split(" and ");
            var threshold = data["threshold"].Value.ReplaceIfNullOrEmpty("300");
            var interval = data["time"].Value.ReplaceIfNullOrEmpty("60");

            var rules = new List<Defrule>();
            var timer = context.CreateTimer();

            rules.Add(new Defrule(new[] { "true" }, new[] { $"enable-timer {timer} {interval}", "disable-self" }));

            (var escrowRules, var goalToResourceMap) = CreateNonEscrowedResourceGoals(context, resources);
            rules.AddRange(escrowRules);

            foreach (var rule in GetBalanceRules(8, int.Parse(threshold), goalToResourceMap, goalToResourceMap.Keys))
            {
                rule.Conditions.Add(new Condition($"timer-triggered {timer}"));
                rules.Add(rule);
            }

            rules.Add(new Defrule(new[] { $"timer-triggered {timer}" }, new[] { $"disable-timer {timer}", $"enable-timer {timer} {interval}" }));

            context.AddToScript(context.ApplyStacks(rules));
        }

        private IEnumerable<Defrule> GetBalanceRules(int amount, int threshold, Dictionary<int, string> goalToResourceMap, IEnumerable<int> resourceGoalsQueue, IEnumerable<int> aboveThreshold = null, IEnumerable<int> belowThreshold = null)
        {
            aboveThreshold = aboveThreshold ?? new int[0];
            belowThreshold = belowThreshold ?? new int[0];

            if (resourceGoalsQueue.Any())
            {
                var resource = resourceGoalsQueue.First();
                foreach (var rule in GetBalanceRules(amount, threshold, goalToResourceMap, resourceGoalsQueue.Skip(1), aboveThreshold.Concat(new[] { resource }), belowThreshold))
                {
                    yield return rule;
                }
                foreach (var rule in GetBalanceRules(amount, threshold, goalToResourceMap, resourceGoalsQueue.Skip(1), aboveThreshold, belowThreshold.Concat(new[] { resource })))
                {
                    yield return rule;
                }
            }
            else if (aboveThreshold.Any() && belowThreshold.Any())
            {
                var rule = new Defrule();

                var halfAmount = amount / 2;
                var above = aboveThreshold.Count();
                var below = belowThreshold.Count();

                var aboveModAmount = halfAmount / above - halfAmount % below;
                var belowModAmount = halfAmount / below - halfAmount % above;

                if (aboveModAmount * above != belowModAmount * below)
                {
                    throw new System.InvalidOperationException("Failed to split resources evenly.");
                }

                foreach (var goalNumber in aboveThreshold)
                {
                    rule.Conditions.Add(new Condition($"strategic-number sn-{goalToResourceMap[goalNumber]}-gatherer-percentage >= {aboveModAmount}"));
                    rule.Conditions.Add(new Condition($"up-compare-goal {goalNumber} c:> {threshold}"));

                    rule.Actions.Add(new Action($"up-modify-sn sn-{goalToResourceMap[goalNumber]}-gatherer-percentage c:- {aboveModAmount}"));
                }

                foreach (var goalNumber in belowThreshold)
                {
                    rule.Conditions.Add(new Condition($"strategic-number sn-{goalToResourceMap[goalNumber]}-gatherer-percentage <= {100 - belowModAmount}"));
                    rule.Conditions.Add(new Condition($"up-compare-goal {goalNumber} c:<= {threshold}"));

                    rule.Actions.Add(new Action($"up-modify-sn sn-{goalToResourceMap[goalNumber]}-gatherer-percentage c:+ {belowModAmount}"));
                }

                yield return rule;
            }
        }
    }
}
