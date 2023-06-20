using Language.ScriptItems;
using System;
using System.Collections.Generic;

namespace Language.Rules
{
    [ActiveRule]
    public class PushDeer : RuleBase
    {
        private static readonly string[] DefaultUnits = new[] { "scout-cavalry-line", "eagle-warrior-line", "camel-line" };

        private const int DefaultPushRange = 30;

        private const int KillRange = 4;

        private const int KillVillagers = 4;

        private const int DistanceFromDeerPrecise = -150;

        public override string Name => "push deer";

        public override string Usage => "push deer with UNIT_LIST within 30 tiles";

        public override IEnumerable<string> Examples => new[]
        {
            "push deer",
            "push deer with archer-line within 100 tiles"
        };

        public PushDeer()
            : base(@"^push deer(?: with (?<unitlist>(?:[^ ]+(?: or (?=\S))?)+))?(?: within (?<range>[^ ]+) tiles)?$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var units = data["unitlist"].Success ? data["unitlist"].Value.Split(" or ") : DefaultUnits;
            var pushRange = data["range"].Success ? int.Parse(data["range"].Value) : DefaultPushRange;

            var rules = new List<Defrule>();

            var dropSitePoint = context.CreatePointGoal();
            var scoutTargetPoint = context.CreatePointGoal();

            var localTotal = context.CreateGoal();
            var localLast = context.CreateGoal();
            var remoteTotal = context.CreateGoal();
            var remoteLast = context.CreateGoal();

            if (localTotal != localLast - 1 || localTotal != remoteTotal - 2 || localTotal != remoteLast - 3)
            {
                throw new InvalidOperationException("Search state goals were not created with consecutive numbers.");
            }

            rules.Add(new Defrule(
                new[]
                {
                    "dropsite-min-distance deer-hunting != 255",
                    "dropsite-min-distance deer-hunting != -1",
                },
                new[]
                {
                    "up-full-reset-search",
                    $"up-get-point position-self {dropSitePoint}",
                    $"up-set-target-point {dropSitePoint}",
                    $"up-modify-goal {dropSitePoint} c:* 100",
                    $"up-modify-goal {dropSitePoint + 1} c:* 100",
                    $"set-strategic-number sn-focus-player-number {Game.GaiaPlayerNumber}",
                    $"up-filter-distance c: -1 c: {pushRange}",
                    $"up-find-remote c: {Game.DeerClassId} c: 100",
                    $"up-get-search-state {localTotal}",
                }));

            var moveScoutRule = new Defrule(
                new[]
                {
                    "dropsite-min-distance deer-hunting != 255",
                    "dropsite-min-distance deer-hunting != -1",
                    $"up-compare-goal {remoteTotal} c:>= 1",
                },
                new[]
                {
                    "up-clean-search search-remote 44 1",
                    "up-set-target-object search-remote c: 0",
                    $"up-get-object-data 38 {scoutTargetPoint}",
                    $"up-get-object-data 39 {scoutTargetPoint + 1}",
                    $"up-lerp-tiles {scoutTargetPoint} {dropSitePoint} c: {DistanceFromDeerPrecise}",
                    "up-reset-filters",
                });

            foreach (var unit in units)
            {
                moveScoutRule.Actions.Add(new ScriptItems.Action($"up-find-local c: {unit} c: 1"));
            }

            moveScoutRule.Actions.Add(new ScriptItems.Action("set-strategic-number sn-target-point-adjustment 6"));
            moveScoutRule.Actions.Add(new ScriptItems.Action($"up-target-point {scoutTargetPoint} action-default -1 -1"));
            moveScoutRule.Actions.Add(new ScriptItems.Action("set-strategic-number sn-target-point-adjustment 0"));

            rules.Add(moveScoutRule);

            rules.Add(new Defrule(
                new[]
                {
                    $"dropsite-min-distance deer-hunting <= {KillRange}",
                    "dropsite-min-distance deer-hunting != -1",
                },
                new[]
                {
                    "up-full-reset-search",
                    "up-find-local c: town-center c: 1",
                    "up-set-target-object search-local c: 0",
                    $"up-get-point position-object {dropSitePoint}",
                    $"up-set-target-point {dropSitePoint}",
                    "up-reset-search 1 1 0 0",
                    $"up-filter-distance c: -1 c: {KillRange}",
                    $"up-find-local c: villager-class c: {KillVillagers}",
                    $"up-remove-objects search-local object-data-target == {Game.DeerClassId}",
                    $"up-find-remote c: {Game.DeerClassId} c: 1",
                    $"up-get-search-state {localTotal}",
                }));

            rules.Add(new Defrule(
                new[]
                {
                    $"up-compare-goal {localTotal} >= 1",
                    $"up-compare-goal {remoteTotal} >= 1",
                },
                new[]
                {
                    "up-target-objects 0 action-default -1 -1",
                    "up-send-scout group-type-land-explore scout-border",
                }));

            context.AddToScript(context.ApplyStacks(rules));
        }
    }
}
