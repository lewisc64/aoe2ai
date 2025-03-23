using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules
{
    [ActiveRule]
    public class MicroMonks : RuleBase
    {
        public override string Name => "micro monks";

        public override string Help => "Forces monks to only convert one target each.";

        public override string Usage => "micro monks";

        public override IEnumerable<string> Examples => new[]
        {
            "micro monks",
        };

        public MicroMonks()
            : base(@"^micro monks$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {

            var rules = new List<Defrule>();
            var iteratedRules = new List<Defrule>();

            var iterateGoal = context.CreateVolatileGoal();
            var tempTargetPoint = context.CreateVolatilePointGoal();
            var distanceGoal = context.CreateVolatileGoal();
            var localTotalGoal = context.CreateConsecutiveGoals(4);
            var remoteTotalGoal = localTotalGoal + 2;

            rules.Add(new Defrule(
                new[]
                {
                    "true",
                },
                new[]
                {
                    "up-full-reset-search",
                    "set-strategic-number sn-focus-player-number 1",
                    $"set-goal {iterateGoal} 0",
                }));

            rules.Add(new Defrule(
                new[]
                {
                    $"players-stance focus-player enemy",
                },
                new[]
                {
                    $"up-find-remote c: archery-class c: 255",
                })
            {
                Compressable = false,
                Splittable = false,
            });

            rules.Add(new Defrule(
                new[]
                {
                    $"strategic-number sn-focus-player-number < 8",
                },
                new[]
                {
                    $"up-modify-sn sn-focus-player-number c:+ 1",
                    "up-jump-rule -2",
                })
            {
                Compressable = false,
                Splittable = false,
            });

            rules.Add(new Defrule(
                new[]
                {
                    "true",
                },
                new[]
                {
                    "up-reset-search 1 1 0 0",
                    "up-reset-filters",
                    "up-find-local c: monastery-class c: 255",
                    $"up-remove-objects search-local {Game.ObjectDataIndex} g:!= {iterateGoal}",
                    $"up-get-search-state {localTotalGoal}",
                })
            {
                Compressable = false,
                Splittable = false,
            });

            iteratedRules.Add(new Defrule(
                new[]
                {
                    $"up-compare-goal {localTotalGoal} c:>= 1",
                },
                new[]
                {
                    "up-set-target-object search-local c: 0",
                    $"up-get-point position-object {tempTargetPoint}",
                    $"up-set-target-point {tempTargetPoint}",
                    $"up-clean-search search-remote {Game.ObjectDataDistance} 1",
                    "up-set-target-object search-remote c: 0",
                    //$"up-get-object-data {Game.ObjectDataDistance} {distanceGoal}",
                    $"up-chat-data-to-all \"i: %d\" g: {iterateGoal}",
                })
            {
                Compressable = false,
                Splittable = false,
            });

            iteratedRules.Add(new Defrule(
                new[]
                {
                    $"up-compare-goal {localTotalGoal} c:>= 1",
                    //$"up-compare-goal {distanceGoal} c:<= 20",
                },
                new[]
                {
                    $"up-target-objects 1 action-default -1 -1",
                    //$"up-remove-objects search-remote -1 == 0"
                })
            {
                Compressable = false,
                Splittable = false,
            });

            rules.AddRange(iteratedRules);

            rules.Add(new Defrule(
                new[]
                {
                    $"up-compare-goal {localTotalGoal} c:>= 1",
                },
                new[]
                {
                    $"up-modify-goal {iterateGoal} c:+ 1",
                    $"up-jump-rule -{iteratedRules.Count + 2}",
                })
            {
                Compressable = false,
                Splittable = false,
            });

            context.FreeVolatilePointGoal(tempTargetPoint);
            context.FreeVolatileGoal(distanceGoal);
            context.FreeVolatileGoal(iterateGoal);

            context.AddToScript(context.ApplyStacks(rules));
        }
    }
}
