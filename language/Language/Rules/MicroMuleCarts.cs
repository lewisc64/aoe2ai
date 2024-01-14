using Language.ScriptItems;
using System.Collections.Generic;
using System.Linq;

namespace Language.Rules
{
    [ActiveRule]
    public class MicroMuleCarts : RuleBase
    {
        private const int MuleCartSpacing = 5;

        public override string Name => "micro mule carts";

        public override string Help => $"Moves mule carts to sensible locations.";

        public override string Usage => "micro mule carts";

        public override IEnumerable<string> Examples => new[]
        {
            "micro mule carts",
        };

        public MicroMuleCarts()
            : base(@"^micro mule carts$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var rules = new List<Defrule>();

            var localTotal = context.CreateConsecutiveGoals(4);
            var remoteTotal = localTotal + 2;

            var myPlayerNumberGoal = context.CreateVolatileGoal();

            // Move mule carts away from each other
            rules.Add(new Defrule(
            new[]
            {
                "true",
            },
            new[]
            {
                "set-strategic-number sn-defer-dropsite-update 0",
                "up-full-reset-search",
                $"up-get-fact player-number 0 {myPlayerNumberGoal}",
                $"up-modify-sn sn-focus-player-number g:= {myPlayerNumberGoal}",
                $"up-find-remote c: mule-cart c: 255",
                $"up-get-search-state {localTotal}",
            }));

            var objectPoint = context.CreateVolatilePointGoal();
            var unitPoint = context.CreateVolatilePointGoal();

            context.UsingVolatileGoal(currentRemoteNumber =>
            {
                rules.Add(new Defrule(
                    new[]
                    {
                        "true",
                    },
                    new[]
                    {
                        $"set-goal {currentRemoteNumber} 0",
                        "up-reset-filters",
                        $"up-filter-distance c: 1 c: {MuleCartSpacing}",
                    }));
                rules.Add(new Defrule(
                    new[]
                    {
                        $"up-compare-goal {currentRemoteNumber} g:< {remoteTotal}",
                    },
                    new[]
                    {
                        $"up-set-target-object search-remote g: {currentRemoteNumber}",
                        $"up-get-point position-object {objectPoint}",
                        $"up-set-target-point {objectPoint}",
                        $"up-get-object-data {Game.ObjectDataPreciseX} {objectPoint}",
                        $"up-get-object-data {Game.ObjectDataPreciseY} {objectPoint + 1}",

                        "up-reset-search 1 1 0 0",
                        $"up-find-local c: mule-cart c: 255",
                        $"up-set-target-object search-local c: 0",
                        $"up-get-object-data {Game.ObjectDataPreciseX} {unitPoint}",
                        $"up-get-object-data {Game.ObjectDataPreciseY} {unitPoint + 1}",

                        $"up-lerp-tiles {unitPoint} {objectPoint} c: -500",
                        "set-strategic-number sn-target-point-adjustment 6",
                        $"up-target-point {unitPoint} action-default -1 -1",
                        "set-strategic-number sn-target-point-adjustment 0",

                        $"up-modify-goal {currentRemoteNumber} c:+ 1",
                        "up-jump-rule -1",
                    }));
                rules.Last().Compressable = false;
            });

            context.FreeVolatilePointGoal(objectPoint);
            context.FreeVolatilePointGoal(unitPoint);

            // Move lazy mule carts to the nearest lumberjack
            rules.Add(new Defrule(
                new[]
                {
                    "true",
                },
                new[]
                {
                    "up-full-reset-search",
                    $"up-modify-sn sn-focus-player-number g:= {myPlayerNumberGoal}",
                    "up-filter-exclude -1 -1 orderid-move -1",
                    $"up-find-local c: mule-cart c: 255",
                    "up-set-target-object search-local c: 0",
                    "up-reset-filters",
                    $"up-find-remote c: {Game.MaleLumberjackId} c: 255",
                    $"up-find-remote c: {Game.FemaleLumberjackId} c: 255",
                    $"up-clean-search search-remote {Game.ObjectDataDistance} 1",
                    "up-target-objects 0 action-move -1 -1",
                }));

            // Move mule carts to resources that need some.
            foreach (var resource in new[] { "wood", "gold", "stone" })
            {
                rules.Add(new Defrule(
                    new[]
                    {
                        $"dropsite-min-distance {resource} > 5",
                        $"strategic-number sn-{resource}-gatherer-percentage > 0",
                    },
                    new[]
                    {
                        "up-full-reset-search",
                        "up-find-local c: town-center c: 1",
                        "up-set-target-object search-local c: 0",
                        "up-full-reset-search",
                        $"up-modify-sn sn-focus-player-number c:= 0",
                        $"up-filter-status c: {Game.StatusResource} c: {Game.ListActive}",
                        $"up-find-resource c: {resource} c: 1",
                        $"up-clean-search search-remote {Game.ObjectDataDistance} 1",
                        "up-find-local c: mule-cart c: 255",
                        "up-set-target-object search-remote c: 0",
                        $"up-clean-search search-local {Game.ObjectDataDistance} 1",
                        $"up-remove-objects search-local {Game.ObjectDataIndex} >= 1",
                        "up-target-objects 0 action-move -1 -1",
                    }));
            }

            context.FreeVolatileGoal(myPlayerNumberGoal);


            context.AddToScriptWithJump(context.ApplyStacks(rules), new Condition("unit-type-count mule-cart == 0"));
        }
    }
}
