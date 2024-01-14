using Language.Extensions;
using Language.ScriptItems;
using System.Collections.Generic;
using System.Linq;

namespace Language.Rules
{
    [ActiveRule]
    public class AutoExpandTownSize : RuleBase
    {
        private static readonly string[] IgnoredBuildings = new[]
        {
            "farm",
            "lumber-camp",
            "mining-camp",
            "mill",
            "dock",
            "mule-cart",
        };

        private const int MinimumTownSize = 8;

        private const int DefaultMaximum = 30;

        private const int Increment = 4;

        public override string Name => "auto expand town size";

        public override string Help => @"Automatically expands town size in order to place buildings. Must be below any builds that it needs to affect. Any build rules below this will be placed at the given maximum.

Affects the following sn's:
 - sn-maximum-town-size
 - sn-minimum-town-size
 - sn-safe-town-size

This will disturb any town size attacks because it takes absolute control over the town size. Disable this during TSA.";

        public override string Usage => @"auto expand town size
auto expand town size to RADIUS";

        public override IEnumerable<string> Examples => new[]
        {
            "build barracks; auto expand town size",
            "build barracks; auto expand town size to 50",
        };

        public AutoExpandTownSize()
            : base(@"^auto expand town size(?: to (?<maximum>[^ ]+))?$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var maximum = GetData(line)["maximum"].Value.ReplaceIfNullOrEmpty(DefaultMaximum.ToString());

            var didResetGoal = context.CreateGoal();

            var buildings = context.Script
                .Where(x => x is Defrule rule && rule.Actions.Any(x => x.Text.StartsWith("build ")))
                .Cast<Defrule>()
                .Select(x => x.Actions.First(y => y.Text.StartsWith("build ")).Text.Split(" ").Last())
                .Except(IgnoredBuildings);

            var pendingBuildingsCondition = Condition.JoinConditions("or", buildings.Select(x => new Condition($"up-pending-placement c: {x}")));

            var numberBuildersGoal = context.CreateVolatileGoal();
            var buildersCapGoal = context.CreateVolatileGoal();

            var rules = new List<Defrule>
            {
                new Defrule(
                    new[]
                    {
                        "true"
                    },
                    new[]
                    {
                        $"set-goal {didResetGoal} 0",
                        $"set-strategic-number sn-maximum-town-size {MinimumTownSize}",
                        $"set-strategic-number sn-minimum-town-size {MinimumTownSize}",
                        $"set-strategic-number sn-safe-town-size {MinimumTownSize}",
                        "disable-self"
                    }),
                new Defrule(
                    new[]
                    {
                        pendingBuildingsCondition,
                        new Condition($"goal {didResetGoal} 0"),
                    },
                    new[]
                    {
                        new Action($"set-strategic-number sn-maximum-town-size {MinimumTownSize}"),
                        new Action($"set-strategic-number sn-safe-town-size {MinimumTownSize}"),
                        new Action($"set-goal {didResetGoal} 1"),
                    }),
                new Defrule(
                    new[]
                    {
                        pendingBuildingsCondition.Invert(),
                        new Condition($"goal {didResetGoal} 1"),
                    },
                    new[]
                    {
                        new Action($"set-goal {didResetGoal} 0"),
                    }),
                new Defrule(
                    new[]
                    {
                        "true",
                    },
                    new[]
                    {
                        $"up-get-fact unit-type-count villager {buildersCapGoal}",
                        $"up-modify-goal {buildersCapGoal} s:min sn-cap-civilian-builders",
                        $"up-get-fact unit-type-count villager-builder {numberBuildersGoal}",
                    }),
                new Defrule(
                    new[]
                    {
                        pendingBuildingsCondition,
                        new Condition($"up-compare-goal {numberBuildersGoal} g:< {buildersCapGoal}"),
                    },
                    new[]
                    {
                        new Action($"up-modify-sn sn-maximum-town-size c:+ {Increment}"),
                        new Action($"up-modify-sn sn-safe-town-size s:= sn-maximum-town-size"),
                    }),
                new Defrule(
                    new[]
                    {
                        pendingBuildingsCondition.Invert(),
                    },
                    new[]
                    {
                        new Action($"up-modify-sn sn-maximum-town-size c:= {maximum}"),
                        new Action($"up-modify-sn sn-safe-town-size s:= sn-maximum-town-size"),
                    }),/*
                new Defrule(
                    new[]
                    {
                        "true",
                    },
                    new[]
                    {
                        "up-chat-data-to-all \"sn-maximum-town-size: %d\" s: sn-maximum-town-size",
                    }),*/
            };

            context.FreeVolatileGoal(numberBuildersGoal);
            context.FreeVolatileGoal(buildersCapGoal);

            context.AddToScript(context.ApplyStacks(rules));
        }
    }
}
