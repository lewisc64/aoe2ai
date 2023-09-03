using Language.ScriptItems;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Language.Rules
{
    [ActiveRule]
    public class KeepAway : RuleBase
    {
        public override string Name => "keep away";

        public override string Usage => "keep UNIT TILES tiles away from enemy/ally/my OBJECTCLASS";

        public override IEnumerable<string> Examples => new[]
        {
            "keep archer-line 8 tiles away from enemy town-center",
        };

        public KeepAway()
            : base(@"^keep (?<unit>[^ ]+) (?<tiles>[^ ]+) tiles away from (?<playertype>enemy|ally|my) (?<object>[^ ]+)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var unit = data["unit"].Value;
            var tiles = data["tiles"].Value;
            var playerType = data["playertype"].Value;
            var objectName = data["object"].Value;

            var localTotal = context.CreateGoal(startId: 41);
            var localLast = context.CreateGoal(startId: 41);
            var remoteTotal = context.CreateGoal(startId: 41);
            var remoteLast = context.CreateGoal(startId: 41);

            if (localTotal != localLast - 1 || localTotal != remoteTotal - 2 || localTotal != remoteLast - 3)
            {
                throw new InvalidOperationException("Search state goals were not created with consecutive numbers.");
            }

            var rules = new List<Defrule>();

            context.UsingVolatileGoal(focusPlayerGoal =>
            {
                string playerFindAction;

                if (playerType == "enemy")
                {
                    playerFindAction = $"up-find-player enemy find-closest {focusPlayerGoal}";
                }
                else if (playerType == "ally")
                {
                    playerFindAction = $"up-find-player ally find-closest {focusPlayerGoal}";
                }
                else if (playerType == "my")
                {
                    playerFindAction = $"up-get-fact player-number 0 {focusPlayerGoal}";
                }
                else
                {
                    throw new InvalidOperationException("Unknown player type");
                }

                rules.Add(new Defrule(
                    new[]
                    {
                        "true",
                    },
                    new[]
                    {
                        "up-full-reset-search",
                        playerFindAction,
                        $"up-modify-sn sn-focus-player-number g:= {focusPlayerGoal}",
                        $"up-find-remote c: {objectName} c: 255",
                        "up-remove-objects search-remote object-data-target == siege-weapon-class",
                        $"up-get-search-state {localTotal}",
                    }));
            });

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
                        $"up-filter-distance c: -1 c: {tiles}",
                        $"up-filter-exclude -1 -1 orderid-move -1",
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
                        $"up-find-local c: {unit} c: 255",
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

            context.AddToScript(context.ApplyStacks(rules));
        }
    }
}
