using Language.ScriptItems;
using System.Collections.Generic;
using System.Linq;
using Language.Extensions;

namespace Language.Rules
{
    [ActiveRule]
    public class BuildFarms : RuleBase
    {
        public override string Name => "build farms";

        public override string Help => "Builds farms according to how many food gatherers should exist.";

        public override string Usage => "build farms";

        public override IEnumerable<string> Examples => new[]
        {
            "build farms",
        };

        public BuildFarms()
            : base(@"^build (farms|pastures)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            context.UsingVolatileGoal(gathererGoal =>
            {
                context.UsingVolatileGoal(farmGoal =>
                {
                    var rules = new[]
                    {
                        new Defrule(
                            [
                                new Condition($"civ-selected {Game.KhitansCiv}").Invert(),
                            ],
                            [
                                new Action($"up-get-fact unit-type-count villager {gathererGoal}"),
                                new Action($"up-modify-goal {gathererGoal} s:* sn-food-gatherer-percentage"),
                                new Action($"up-modify-goal {gathererGoal} c:/ 100"),
                                new Action($"up-get-fact building-type-count-total farm {farmGoal}"),
                            ]),
                        new Defrule(
                            [
                                new Condition($"civ-selected {Game.KhitansCiv}").Invert(),
                                new Condition($"up-compare-goal {farmGoal} g:< {gathererGoal}"),
                                new Condition("can-build farm"),
                            ],
                            [
                                new Action("build farm"),
                            ]),
                        new Defrule(
                            [
                                $"civ-selected {Game.KhitansCiv}",
                            ],
                            [
                                $"up-get-fact unit-type-count villager {gathererGoal}",
                                $"up-modify-goal {gathererGoal} s:* sn-food-gatherer-percentage",
                                $"up-modify-goal {gathererGoal} c:/ 200", // two villagers per pasture
                                $"up-get-fact building-type-count-total {Game.PastureId} {farmGoal}",
                            ]),
                        new Defrule(
                            [
                                $"civ-selected {Game.KhitansCiv}",
                                $"up-compare-goal {farmGoal} g:< {gathererGoal}",
                                $"can-build {Game.PastureId}",
                            ],
                            [
                                $"build {Game.PastureId}",
                            ])
                    };

                    context.AddToScript(context.ApplyStacks(rules));
                });
            });
        }
    }
}
