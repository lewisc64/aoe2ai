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
            var closestPlayerGoal = context.CreateVolatileGoal();
            var justDidRetargetGoal = context.CreateVolatileGoal();

            var rules = new[]
            {
                new Defrule(new[] { "true" }, new[] { $"set-goal {justDidRetargetGoal} 0" }),
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
                                new Condition("players-population target-player == 0"),
                            }),
                    },
                    new[]
                    {
                        new Action($"up-find-player enemy find-closest {closestPlayerGoal}"),
                        new Action($"up-modify-sn sn-target-player-number g:= {closestPlayerGoal}"),
                        new Action($"set-goal {justDidRetargetGoal} 1"),
                    }),
                new Defrule(
                    new[]
                    {
                        $"goal {justDidRetargetGoal} 1",
                        "players-military-population target-player == 0",
                        "players-population target-player < 20",
                    },
                    new[]
                    {
                        $"up-find-next-player enemy find-closest {closestPlayerGoal}",
                    })
                {
                    Compressable = false,
                    Splittable = false,
                },
                new Defrule(
                    new[]
                    {
                        $"goal {justDidRetargetGoal} 1",
                        "players-military-population target-player == 0",
                        "players-population target-player < 20",
                        $"up-compare-goal {closestPlayerGoal} c:>= 1",
                    },
                    new[]
                    {
                        $"up-modify-sn sn-target-player-number g:= {closestPlayerGoal}",
                        "up-jump-rule -2",
                    })
                {
                    Compressable = false,
                    Splittable = false,
                },
            };

            context.FreeVolatileGoal(closestPlayerGoal);
            context.FreeVolatileGoal(justDidRetargetGoal);

            context.AddToScript(context.ApplyStacks(rules));
        }
    }
}
