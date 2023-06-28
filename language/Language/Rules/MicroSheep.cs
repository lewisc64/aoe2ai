using Language.Extensions;
using Language.ScriptItems;
using System.Collections.Generic;
using System.Linq;

namespace Language.Rules
{
    [ActiveRule]
    public class MicroSheep : RuleBase
    {
        private const int MaxShepherds = 6;

        public override string Name => "micro sheep";

        public override string Help => "Takes sheep one at a time. Garrisons them in a mill for Gurjaras.";

        public override string Usage => "micro sheep";

        public override IEnumerable<string> Examples => new[]
        {
            "micro sheep",
        };

        public MicroSheep()
            : base(@"^micro sheep$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var localTotal = context.CreateGoal(startId: 41);
            var localLast = context.CreateGoal(startId: 41);
            var remoteTotal = context.CreateGoal(startId: 41);
            var remoteLast = context.CreateGoal(startId: 41);

            if (localTotal != localLast - 1 || localTotal != remoteTotal - 2 || localTotal != remoteLast - 3)
            {
                throw new System.InvalidOperationException("Search state goals were not created with consecutive numbers.");
            }

            var tcPointGoal = context.CreatePointGoal();
            var mapCenterPointGoal = context.CreatePointGoal();
            var playerGoal = context.CreateVolatileGoal();
            var shepherdsGoal = context.CreateVolatileGoal();

            var normalMicroRules = new[]
            {
                new Defrule( // find the dead sheep
                    new[]
                    {
                        "true",
                    },
                    new[]
                    {
                        "up-full-reset-search",
                        $"up-get-point position-self {tcPointGoal}",
                        $"up-set-target-point {tcPointGoal}",
                        "up-modify-sn sn-focus-player-number c:= 0",
                        "up-filter-distance c: -1 c: 10",
                        $"up-filter-status c: {Game.StatusGather} c: {Game.ListActive}",
                        "up-find-status-remote c: livestock-class c: 1",
                        $"up-get-search-state {localTotal}",
                    }),
                new Defrule( // no dead sheep, find the dying sheep?
                    new[]
                    {
                        $"goal {remoteTotal} 0",
                    },
                    new[]
                    {
                        $"up-filter-status c: {Game.StatusDown} c: {Game.ListActive}",
                        "up-find-status-remote c: livestock-class c: 1",
                        $"up-get-search-state {localTotal}",
                    }),
                new Defrule( // task shepherds to take the dead sheep
                    new[]
                    {
                        $"up-compare-goal {remoteTotal} c:>= 1",
                    },
                    new[]
                    {
                        "up-reset-filters",
                        $"up-find-local c: {Game.MaleShepherd} c: 255",
                        $"up-find-local c: {Game.FemaleShepherd} c: 255",
                        "up-target-objects 0 action-default -1 -1",
                    }),
                new Defrule( // no dead sheep, kill the next sheep
                    new[]
                    {
                        $"goal {remoteTotal} 0",
                    },
                    new[]
                    {
                        "up-full-reset-search",
                        $"up-get-fact player-number 0 {playerGoal}",
                        $"up-modify-sn sn-focus-player-number g:= {playerGoal}",
                        "up-find-remote c: livestock-class c: 1",
                        $"up-find-local c: {Game.MaleShepherd} c: 255",
                        $"up-find-local c: {Game.FemaleShepherd} c: 255",
                        "up-target-objects 0 action-default -1 -1",
                    }),
                new Defrule( // move living sheep away from tc
                    new[]
                    {
                        "true",
                    },
                    new[]
                    {
                        "up-full-reset-search",
                        $"up-get-fact player-number 0 {playerGoal}",
                        $"up-modify-sn sn-focus-player-number g:= {playerGoal}",
                        "up-find-local c: livestock-class c: 255",
                        "up-remove-objects search-local -1 == 0",
                        "up-find-remote c: town-center c: 1",
                        "up-set-target-object search-remote c: 0",
                        $"up-get-point position-object {tcPointGoal}",
                        $"up-get-point position-center {mapCenterPointGoal}",
                        $"up-lerp-tiles {tcPointGoal} {mapCenterPointGoal} c: -3",
                        $"up-target-point {tcPointGoal} action-default -1 -1",
                    }),
                new Defrule( // find dead sheep with less than 30 food
                    new[]
                    {
                        "true",
                    },
                    new[]
                    {
                        "up-full-reset-search",
                        "up-modify-sn sn-focus-player-number c:= 0",
                        "up-filter-distance c: -1 c: 10",
                        $"up-filter-status c: {Game.StatusGather} c: {Game.ListActive}",
                        "up-find-status-remote c: livestock-class c: 1",
                        "up-find-local c: livestock-class c: 1",
                        $"up-remove-objects search-remote object-data-carry > 30",
                        $"up-get-search-state {localTotal}",
                    }),
                new Defrule( // move next sheep under tc if found
                    new[]
                    {
                        $"up-compare-goal {remoteTotal} c:>= 1",
                    },
                    new[]
                    {
                        "up-full-reset-search",
                        $"up-get-fact player-number 0 {playerGoal}",
                        $"up-modify-sn sn-focus-player-number g:= {playerGoal}",
                        "up-find-local c: livestock-class c: 1",
                        "up-find-remote c: town-center c: 1",
                        "set-strategic-number sn-target-point-adjustment 5",
                        "up-target-objects 0 action-move -1 -1",
                        "set-strategic-number sn-target-point-adjustment 0",
                    }),
                new Defrule( // count the shepherds
                    new[]
                    {
                        "true",
                    },
                    new[]
                    {
                        $"up-get-fact unit-type-count {Game.MaleShepherd} {shepherdsGoal}",
                        $"up-get-fact unit-type-count {Game.FemaleShepherd} {playerGoal}",
                        $"up-modify-goal {shepherdsGoal} g:+ {playerGoal}",
                    }),
                new Defrule( // retask excess shepherds onto berries
                    new[]
                    {
                        $"up-compare-goal {shepherdsGoal} c:> {MaxShepherds}",
                    },
                    new[]
                    {
                        "up-full-reset-search",
                        "up-modify-sn sn-focus-player-number c:= 0",
                        $"up-find-local c: {Game.MaleShepherd} c: 1",
                        $"up-find-local c: {Game.FemaleShepherd} c: 1",
                        "up-remove-objects search-local -1 >= 1",
                        $"up-find-remote c: {Game.ForageClassId} c: 255",
                        "up-target-objects 0 action-default -1 -1",
                    }),
            };

            var gurjaraMicroRules = new[]
            {
                new Defrule(
                    new[]
                    {
                        "true",
                    },
                    new[]
                    {
                        "up-full-reset-search", // task shepherds onto the berries
                        "up-modify-sn sn-focus-player-number c:= 0",
                        "up-find-local c: town-center c: 1",
                        "up-set-target-object search-local c: 0",
                        $"up-get-point position-object {tcPointGoal}",
                        $"up-set-target-point {tcPointGoal}",
                        "up-reset-search 1 1 0 0",
                        $"up-find-local c: {Game.MaleShepherd} c: 255",
                        $"up-find-local c: {Game.FemaleShepherd} c: 255",
                        "up-filter-distance c: -1 c: 30",
                        $"up-find-remote c: {Game.ForageClassId} c: 255",
                        "up-target-objects 0 action-default -1 -1",

                        "up-reset-search 1 1 1 1", // garrison the sheep
                        $"up-get-fact player-number 0 {playerGoal}",
                        $"up-modify-sn sn-focus-player-number g:= {playerGoal}",
                        "up-reset-filters",
                        "up-find-local c: livestock-class c: 255",
                        "up-find-remote c: mill c: 1",
                        "up-target-objects 0 action-garrison -1 -1",
                        "up-reset-search 1 1 0 0",
                        "up-find-local c: mill c: 1",
                        "up-target-objects 0 action-gather -1 -1",
                    }),
            };

            context.FreeVolatileGoal(playerGoal);
            context.AddToScriptWithJump(normalMicroRules, Condition.JoinConditions("and", context.ConditionStack.Concat(new[] { new Condition("civ-selected gurjaras") })));
            context.AddToScriptWithJump(gurjaraMicroRules, Condition.JoinConditions("and", context.ConditionStack.Concat(new[] { new Condition("civ-selected gurjaras").Invert() })));
        }
    }
}
