using Language.Extensions;
using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules
{
    [ActiveRule]
    public class BuildMills : RuleBase
    {
        private static readonly Dictionary<string, int> FarmSupport = new()
        {
            { "town-center", 9 },
            { "mill", 6 },
        };

        public override string Name => "build mills";

        public override string Help => "Builds mills based on how many farms there are.";

        public override string Usage => "build mills";

        public override IEnumerable<string> Examples => new[]
        {
            "build mills",
        };

        public BuildMills()
            : base(@"^build mills$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var rules = new List<Defrule>();

            var currentFarmsSupportedGoal = context.CreateVolatileGoal();

            rules.Add(new Defrule(new[] { "true" }, new[] { $"set-goal {currentFarmsSupportedGoal} 0" }));

            context.UsingVolatileGoal(buildingCountGoal =>
            {
                foreach (var (building, support) in FarmSupport)
                {
                    rules.Add(new Defrule(
                        new[]
                        {
                            "true",
                        },
                        new[]
                        {
                            $"up-get-fact building-type-count-total {building} {buildingCountGoal}",
                            $"up-modify-goal {buildingCountGoal} c:* {support}",
                            $"up-modify-goal {currentFarmsSupportedGoal} g:+ {buildingCountGoal}"
                        }));
                }
            });

            var farmCountGoal = context.CreateVolatileGoal();

            rules.Add(new Defrule(new[] { "true" }, new[] { $"up-get-fact building-type-count-total farm {farmCountGoal}" }));

            rules.Add(new Defrule(
                new[]
                {
                    new CombinatoryCondition(
                        "or",
                        new[]
                        {
                            new Condition("building-type-count-total mill == 0"),
                            new CombinatoryCondition(
                                "and",
                                new[]
                                {
                                    new Condition("civ-selected khmer").Invert(),
                                    new Condition($"up-compare-goal {farmCountGoal} g:>= {currentFarmsSupportedGoal}"),
                                }),
                        }),
                    new CombinatoryCondition(
                        "or",
                        new[]
                        {
                            new Condition("resource-found food"),
                            new CombinatoryCondition(
                                "and",
                                new[]
                                {
                                    new Condition("civ-selected khmer").Invert(),
                                    new Condition("game-time >= 60"),
                                }),
                        }),
                    new Condition("can-build mill"),
                },
                new[] {
                    new Action("build mill"),
                }));

            context.AddToScript(context.ApplyStacks(rules));

            context.FreeVolatileGoal(currentFarmsSupportedGoal);
            context.FreeVolatileGoal(farmCountGoal);
        }
    }
}
