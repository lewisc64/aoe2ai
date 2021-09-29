using Language.Extensions;
using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules
{
    [ActiveRule]
    public class AutoExpandTownSize : RuleBase
    {
        private const int MinimumTownSize = 5;

        private const int Increment = 5;

        private const int DefaultMaximum = 30;

        private static readonly IEnumerable<string> Buildings = new[]
        {
            "farm",
            "archery-range",
            "barracks",
            "blacksmith",
            "bombard-tower",
            "castle",
            "guard-tower",
            "house",
            "keep",
            "market",
            "monastery",
            "outpost",
            "siege-workshop",
            "stable",
            "town-center",
            "university",
            "watch-tower",
            "watch-tower-line",
            "wonder",
        };

        public override string Name => "auto expand town size";

        public override string Help => @"Automatically expands the town size based on if a building is meant to be built but there are no villagers to build it.
Can be used in conjunction with town size attacks modifying only sn-maximum-town-size. Upon activation it will shrink the town size to a minimum once and expand from there.

Affects the following sn's:
 - sn-maximum-town-size
 - sn-minimum-town-size
 - sn-safe-town-size
 - sn-maximum-food-drop-distance";

        public override string Usage => "auto expand town size";

        public AutoExpandTownSize()
            : base(@"^auto expand town size(?: to (?<maximum>[^ ]+))$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var maximum = GetData(line)["maximum"].Value.ReplaceIfNullOrEmpty(DefaultMaximum.ToString());

            var buildingInQueueGoal = context.CreateGoal();

            var rules = new List<Defrule>()
            {
                new Defrule(
                    new[] {
                        "true"
                    },
                    new[]
                    {
                        $"set-strategic-number sn-minimum-town-size {MinimumTownSize}",
                        $"set-strategic-number sn-maximum-town-size {MinimumTownSize}",
                        $"set-strategic-number sn-safe-town-size {MinimumTownSize}",
                        $"set-strategic-number sn-maximum-food-drop-distance {MinimumTownSize}",
                        "disable-self"
                    }),
                new Defrule(new[] { "true" }, new[] { $"set-goal {buildingInQueueGoal} 0" }),
            };

            foreach (var building in Buildings)
            {
                rules.Add(new Defrule(
                    new[]
                    {
                        $"goal {buildingInQueueGoal} 0",
                        $"up-pending-objects c: {building} > 0"
                    },
                    new[]
                    {
                        $"set-goal {buildingInQueueGoal} 1"
                    }));
            }

            rules.Add(new Defrule(
                new[]
                {
                    $"goal {buildingInQueueGoal} 1",
                    "unit-type-count 118 == 0",
                    "unit-type-count 212 == 0",
                    "civilian-population >= 2",
                    $"strategic-number sn-maximum-town-size < {maximum}",
                    "not (town-under-attack)",
                },
                new[]
                {
                    $"up-modify-sn sn-minimum-town-size c:+ {Increment}",
                    $"up-modify-sn sn-maximum-town-size c:+ {Increment}",
                    "up-modify-sn sn-safe-town-size s:= sn-minimum-town-size",
                    "up-modify-sn sn-maximum-food-drop-distance s:= sn-minimum-town-size",
                }));

            context.AddToScript(context.ApplyStacks(rules));
        }
    }
}
