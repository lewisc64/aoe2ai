using Language.ScriptItems;
using System;
using System.Collections.Generic;

namespace Language.Rules
{
    [ActiveRule]
    public class DodgeFortifications : RuleBase
    {
        private const int SafetyBuffer = 8;

        public override string Name => "dodge fortifications";

        public override string Usage => "dodge fortifications";

        public override IEnumerable<string> Examples => new[]
        {
            "dodge fortifications",
        };

        public DodgeFortifications()
            : base(@"^dodge fortifications$")
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
                throw new InvalidOperationException("Search state goals were not created with consecutive numbers.");
            }

            var rules = new List<Defrule>();

            rules.Add(new Defrule(
                new[]
                {
                    "true",
                },
                new[]
                {
                    "set-strategic-number sn-focus-player-number 1",
                    "up-full-reset-search",
                }));
            rules.Add(new Defrule(
                new[]
                {
                    $"players-stance focus-player enemy",
                },
                new[]
                {
                    $"up-find-remote c: town-center c: 255",
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
                    "up-remove-objects search-remote object-data-hitpoints < 400",
                    "set-strategic-number sn-focus-player-number 1",
                }));
            rules.Add(new Defrule(
                new[]
                {
                    $"players-stance focus-player enemy",
                },
                new[]
                {
                    $"up-find-remote c: castle c: 255",
                    $"up-find-remote c: donjon c: 255",
                    $"up-find-remote c: krepost c: 255",
                    $"up-find-remote c: tower-class c: 255",
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
                    "up-remove-objects search-remote object-data-target == siege-weapon-class",
                    $"up-get-search-state {localTotal}",
                }));

            var objectPoint = context.CreateVolatilePointGoal();
            var unitPoint = context.CreateVolatilePointGoal();

            var currentRemoteNumber = context.CreateVolatileGoal();
            var buildingRange = context.CreateVolatileGoal();

            rules.Add(new Defrule(
                new[]
                {
                    "true",
                },
                new[]
                {
                    $"set-goal {currentRemoteNumber} 0",
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

                    "up-reset-filters",
                    $"up-get-object-data object-data-range {buildingRange}",
                    $"up-modify-goal {buildingRange} c:+ {SafetyBuffer}",
                    $"up-filter-distance c: -1 g: {buildingRange}",
                    $"up-filter-exclude -1 -1 orderid-move -1",
                    "up-filter-include cmdid-military -1 -1 -1",
                    "up-filter-exclude -1 -1 -1 siege-weapon-class",

                    "up-reset-search 1 1 0 0",
                    $"up-find-local c: all-units-class c: 255",
                    $"up-remove-objects search-local {Game.ObjectDataPierceArmor} >= 100",
                    $"up-set-target-object search-local c: 0",
                    $"up-get-object-data {Game.ObjectDataPreciseX} {unitPoint}",
                    $"up-get-object-data {Game.ObjectDataPreciseY} {unitPoint + 1}",

                    $"up-lerp-tiles {unitPoint} {objectPoint} c: -200",
                    $"up-cross-tiles {unitPoint} {objectPoint} c: 500",

                    "set-strategic-number sn-target-point-adjustment 6",
                    $"up-target-point {unitPoint} action-default -1 -1",
                    "set-strategic-number sn-target-point-adjustment 0",

                    $"up-modify-goal {currentRemoteNumber} c:+ 1",
                    "up-jump-rule -1",
                })
            {
                Compressable = false,
                Splittable = false,
            });

            context.FreeVolatilePointGoal(objectPoint);
            context.FreeVolatilePointGoal(unitPoint);

            context.FreeVolatileGoal(currentRemoteNumber);
            context.FreeVolatileGoal(buildingRange);

            context.AddToScript(context.ApplyStacks(rules));
        }
    }
}
