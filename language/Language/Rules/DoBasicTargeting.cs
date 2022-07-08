using Language.Extensions;
using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules
{
    [ActiveRule]
    public class DoBasicTargeting : RuleBase
    {
        public override string Name => "do basic targeting";

        public override string Help => "Targets the closest enemy, does not retarget until they lose or become allied.";

        public override IEnumerable<string> Examples => new[]
        {
            "do basic targeting",
        };

        public DoBasicTargeting()
            : base(@"^do basic targeting$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            context.UsingVolatileGoal(closestPlayerGoal =>
            {
                var rules = new[]
                {
                    new Defrule(
                        new[]
                        {
                            Condition.JoinConditions(
                                "or",
                                new[]
                                {
                                    new Condition("strategic-number sn-target-player-number <= 0"),
                                    new Condition("player-in-game target-player").Invert(),
                                    new Condition("stance-toward target-player enemy").Invert(),
                                }),
                        },
                        new[]
                        {
                            new Action($"up-find-player enemy find-closest {closestPlayerGoal}"),
                            new Action($"up-modify-sn sn-target-player-number g:= {closestPlayerGoal}"),
                        }),
                };

                context.AddToScript(context.ApplyStacks(rules));
            });
        }
    }
}
