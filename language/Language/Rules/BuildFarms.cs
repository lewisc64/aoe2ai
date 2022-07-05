using Language.ScriptItems;
using System.Collections.Generic;

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
            : base(@"^build farms$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            context.UsingVolatileGoal(gathererGoal =>
            {
                context.UsingVolatileGoal(farmGoal =>
                {
                    var infoRule = new Defrule(
                        new[]
                        {
                            "true",
                        },
                        new[]
                        {
                            $"up-get-fact unit-type-count villager {gathererGoal}",
                            $"up-modify-goal {gathererGoal} s:* sn-food-gatherer-percentage",
                            $"up-modify-goal {gathererGoal} c:/ 100",
                            $"up-get-fact building-type-count-total farm {farmGoal}",
                        });

                    var buildRule = new Defrule(
                        new[]
                        {
                            $"up-compare-goal {farmGoal} g:< {gathererGoal}",
                            "can-build farm",
                        },
                        new[]
                        {
                            "build farm"
                        });

                    context.AddToScript(context.ApplyStacks(infoRule));
                    context.AddToScript(context.ApplyStacks(buildRule));
                });
            });
        }
    }
}
