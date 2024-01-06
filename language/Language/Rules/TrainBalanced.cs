using Language.ScriptItems;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Language.Rules
{
    [ActiveRule(1)]
    public class TrainBalanced : RuleBase
    {
        public override string Name => "train balanced";

        public override string Help => "Trains multiple types of units so they are in the specified ratios on the field.";

        public override string Usage => "train balanced WEIGHT UNIT WEIGHT UNIT [...]";

        public override IEnumerable<string> Examples => new[]
        {
            "train balanced 1 archer-line 1 militiaman-line",
            "train balanced 10 scout-cavalry-line 3 spearman-line",
        };

        public TrainBalanced()
            : base(@"^train balanced(?<units> [0-9]+ [^ ]+)+$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var unitData = GetData(line)["units"].Captures
                .Select(x => x.Value.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                .Select(x => (x[1], int.Parse(x[0])));

            var rules = new List<Defrule>();

            var militaryCountGoal = context.CreateVolatileGoal();
            var unitCountGoal = context.CreateVolatileGoal();
            var unitThresholdGoal = context.CreateVolatileGoal();

            rules.Add(new Defrule(new[] { "true" }, new[] { $"set-goal {militaryCountGoal} 0" }));

            foreach (var (unit, _) in unitData)
            {
                rules.Add(new Defrule(
                    new[]
                    {
                        "true",
                    },
                    new[]
                    {
                        $"up-get-fact unit-type-count-total {unit} {unitCountGoal}",
                        $"up-modify-goal {militaryCountGoal} g:+ {unitCountGoal}",
                    }));
            }

            var totalWeight = unitData.Select(x => x.Item2).Sum();

            foreach (var (unit, weight) in unitData)
            {
                rules.Add(new Defrule(
                    new[]
                    {
                        "true",
                    },
                    new[]
                    {
                        $"up-get-fact unit-type-count-total {unit} {unitCountGoal}",
                        $"up-modify-goal {unitThresholdGoal} g:= {militaryCountGoal}",
                        $"up-modify-goal {unitThresholdGoal} c:* {weight}",
                        $"up-modify-goal {unitThresholdGoal} c:/ {totalWeight}",
                    }));

                rules.Add(new Defrule(
                    new[]
                    {
                        $"up-compare-goal {unitCountGoal} g:<= {unitThresholdGoal}",
                        $"can-train {unit}",
                    },
                    new[]
                    {
                        $"train {unit}",
                    }));
            }

            context.FreeVolatileGoal(militaryCountGoal);
            context.FreeVolatileGoal(unitCountGoal);
            context.FreeVolatileGoal(unitThresholdGoal);

            context.AddToScript(context.ApplyStacks(rules));
        }
    }
}
