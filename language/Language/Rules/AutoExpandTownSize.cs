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
        };

        private const int MinimumTownSize = 8;

        private const int DefaultMaximum = 30;

        private const int Increment = 4;

        public override string Name => "auto expand town size";

        public override string Help => @"Automatically expands town size. Must be below any builds that it needs to affect. Any build rules below this will be placed at the given maximum.

Affects the following sn's:
 - sn-maximum-town-size
 - sn-minimum-town-size
 - sn-safe-town-size
 - sn-maximum-food-drop-distance

This will disturb any town size attacks because it takes absolute control over the town size. Disable this during TSA.";

        public override string Usage => @"auto expand town size
auto expand town size to 30";

        public AutoExpandTownSize()
            : base(@"^auto expand town size(?: to (?<maximum>[^ ]+))?$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var maximum = GetData(line)["maximum"].Value.ReplaceIfNullOrEmpty(DefaultMaximum.ToString());

            var hasBuiltBuildingGoal = context.CreateGoal();
            var didResetGoal = context.CreateGoal();

            foreach (var item in context.Script)
            {
                if (item is Defrule rule && rule.Actions.Any(x => x.Text.StartsWith("build ") && !IgnoredBuildings.Any(y => x.Text.EndsWith($" {y}"))))
                {
                    rule.Actions.Add(new Action($"set-goal {hasBuiltBuildingGoal} 1"));
                }
            }

            var rules = new List<Defrule>()
            {
                new Defrule(
                    new[] {
                        "true"
                    },
                    new[]
                    {
                        $"set-strategic-number sn-maximum-town-size {maximum}",
                        $"set-strategic-number sn-minimum-town-size {MinimumTownSize}",
                        $"set-strategic-number sn-safe-town-size {maximum}",
                        $"set-strategic-number sn-maximum-food-drop-distance {maximum}",
                        $"set-goal {didResetGoal} 0",
                        "disable-self"
                    }),
                new Defrule(
                    new[]
                    {
                        "true",
                    },
                    new[]
                    {
                        $"up-modify-sn sn-maximum-town-size c:+ {Increment}",
                        $"up-modify-sn sn-maximum-town-size c:min {maximum}",
                        $"set-strategic-number sn-minimum-town-size {MinimumTownSize}",
                        $"set-strategic-number sn-safe-town-size {maximum}",
                        $"set-strategic-number sn-maximum-food-drop-distance {maximum}",
                    }),
                new Defrule(
                    new[]
                    {
                        $"goal {hasBuiltBuildingGoal} 0",
                    },
                    new[]
                    {
                        $"set-goal {didResetGoal} 0",
                    }),
                new Defrule(
                    new[]
                    {
                        $"goal {hasBuiltBuildingGoal} 1",
                        $"goal {didResetGoal} 0",
                    },
                    new[]
                    {
                        "up-modify-sn sn-maximum-town-size s:= sn-minimum-town-size",
                        $"set-goal {didResetGoal} 1",
                    }),
                new Defrule(
                    new[]
                    {
                        "true",
                    },
                    new[]
                    {
                        $"set-goal {hasBuiltBuildingGoal} 0",
                    }),
            };

            context.AddToScript(context.ApplyStacks(rules));
        }
    }
}
