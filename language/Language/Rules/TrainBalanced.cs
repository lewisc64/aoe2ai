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
            var unitBuildingGoal = context.CreateVolatileGoal();
            var totalWeightGoal = context.CreateVolatileGoal();

            rules.Add(new Defrule(new[] { "true" }, new[] { $"set-goal {militaryCountGoal} 0", $"set-goal {totalWeightGoal} 0" }));

            foreach (var (unit, weight) in unitData)
            {
                rules.Add(new Defrule(
                    new[]
                    {
                        "true",
                    },
                    new[]
                    {
                        $"up-get-object-type-data c: {unit} {Game.ObjectDataTrainSite} {unitBuildingGoal}",
                    }));

                rules.Add(new Defrule(
                    new[]
                    {
                        $"unit-available {unit}",
                        $"up-object-type-count g: {unitBuildingGoal} c:>= 1",
                    },
                    new[]
                    {
                        $"up-get-fact unit-type-count-total {(Game.UnitSets.ContainsKey(unit) ? Game.UnitSets[unit] : unit)} {unitCountGoal}",
                        $"up-modify-goal {militaryCountGoal} g:+ {unitCountGoal}",
                        $"up-modify-goal {totalWeightGoal} c:+ {weight}",
                    }));
            }

            var unitThresholdGoal = context.CreateVolatileGoal();

            foreach (var (unit, weight) in unitData)
            {
                rules.Add(new Defrule(
                    new[]
                    {
                        "true",
                    },
                    new[]
                    {
                        $"up-get-fact unit-type-count-total {(Game.UnitSets.ContainsKey(unit) ? Game.UnitSets[unit] : unit)} {unitCountGoal}",
                        $"up-modify-goal {unitThresholdGoal} g:= {militaryCountGoal}",
                        $"up-modify-goal {unitThresholdGoal} c:* {weight}",
                        $"up-modify-goal {unitThresholdGoal} g:/ {totalWeightGoal}",
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
            context.FreeVolatileGoal(unitBuildingGoal);
            context.FreeVolatileGoal(unitThresholdGoal);
            context.FreeVolatileGoal(totalWeightGoal);

            context.AddToScript(context.ApplyStacks(rules));
        }
    }
}
