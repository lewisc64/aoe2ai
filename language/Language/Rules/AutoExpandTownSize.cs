using Language.Extensions;
using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules
{
    [ActiveRule]
    public class AutoExpandTownSize : RuleBase
    {
        private const int MinimumTownSize = 8;

        private const int DefaultMaximum = 30;

        private static readonly IEnumerable<string> TownBuildings = new[]
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

        public override string Help => @"Automatically expands town size heuristically based on the number of buildings.
Can be used in conjunction with town size attacks modifying only sn-maximum-town-size above the maximum supplied.

Affects the following sn's:
 - sn-maximum-town-size
 - sn-minimum-town-size
 - sn-safe-town-size
 - sn-maximum-food-drop-distance";

        public override string Usage => @"auto expand town size
auto expand town size to 30";

        public AutoExpandTownSize()
            : base(@"^auto expand town size(?: to (?<maximum>[^ ]+))?$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var maximum = GetData(line)["maximum"].Value.ReplaceIfNullOrEmpty(DefaultMaximum.ToString());

            var buildingCountGoal = context.CreateGoal();
            var desiredSizeGoal = context.CreateGoal();

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
                new Defrule(new[] { "true" }, new[] { $"set-goal {desiredSizeGoal} 0" }),
            };

            foreach (var building in TownBuildings)
            {
                rules.Add(new Defrule(
                    new[]
                    {
                        "true"
                    },
                    new[]
                    {
                        $"up-get-fact building-type-count {building} {buildingCountGoal}",
                        $"up-modify-goal {desiredSizeGoal} g:+ {buildingCountGoal}",
                    }));
            }

            rules.Add(new Defrule(
                new[]
                {
                    $"strategic-number sn-maximum-town-size <= {maximum}",
                },
                new[]
                {
                    $"up-modify-goal {desiredSizeGoal} c:/ 3",
                    $"up-modify-goal {desiredSizeGoal} c:+ 5",
                    $"up-modify-goal {desiredSizeGoal} c:max {MinimumTownSize}",
                    $"up-modify-goal {desiredSizeGoal} c:min {maximum}",
                    $"up-modify-sn sn-minimum-town-size g:= {desiredSizeGoal}",
                    $"up-modify-sn sn-minimum-town-size c:min {maximum}",
                    "up-modify-sn sn-maximum-town-size s:= sn-minimum-town-size",
                    "up-modify-sn sn-safe-town-size s:= sn-minimum-town-size",
                    "up-modify-sn sn-maximum-food-drop-distance s:= sn-minimum-town-size",
                }));

            context.AddToScript(context.ApplyStacks(rules));
        }
    }
}
