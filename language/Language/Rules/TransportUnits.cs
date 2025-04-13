using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules;

[ActiveRule]
public class TransportUnits : RuleBase
{
    private const int TransportShipCapacity = 20;
    
    public override string Name => "transport units";

    public override string Help => "Garrisons units into transport ships and sends them to the target player.";

    public override string Usage => "transport units";

    public override IEnumerable<string> Examples => ["transport units"];

    public TransportUnits()
        : base(@"^transport units$")
    {
    }

    public override void Parse(string line, TranspilerContext context)
    {
        var targetMapZoneIdGoal = context.CreateVolatileGoal();
        var myPlayerNumberGoal = context.CreateVolatileGoal();
        var unitMapZoneGoal = context.CreateVolatileGoal();
        var targetPointGoal = context.CreateVolatilePointGoal();
        
        var rules = new[]
        {
            new Defrule(
                [
                    "soldier-count >= 1",
                    "unit-type-count transport-ship >= 1",
                ],
                [
                    "up-full-reset-search",
                    "up-modify-sn sn-focus-player-number s:= sn-target-player-number",
                    "up-filter-include -1 -1 -1 0",
                    "up-find-remote c: town-center c: 1",
                    "up-find-remote c: building-class c: 1",
                ]),
            new Defrule(
                [
                    "soldier-count >= 1",
                    "unit-type-count transport-ship >= 1",
                    "up-set-target-object search-remote c: 0",
                ],
                [
                    $"up-get-object-data {Game.ObjectDataMapZoneId} {targetMapZoneIdGoal}",
                    
                    "up-full-reset-search",
                    $"up-get-fact player-number 0 {myPlayerNumberGoal}",
                    $"up-modify-sn sn-focus-player-number g:= {myPlayerNumberGoal}",
                    
                    "fe-filter-garrisoned c: 0",
                    "up-find-local c: archery-class c: 255",
                    "up-find-local c: infantry-class c: 255",
                    "up-find-local c: cavalry-class c: 255",
                    "up-find-local c: siege-weapon-class c: 255",
                    "up-find-local c: monastery-class c: 255",
                    "up-find-local c: petard-class c: 255",
                    "up-find-local c: archery-cannon-class c: 255",
                    $"up-find-local c: {Game.ScoutCavalryClassId} c: 255",
                    "up-find-local c: packed-trebuchet-class c: 255",
                    "up-find-local c: scorpion-class c: 255",
                    $"up-remove-objects search-local {Game.ObjectDataMapZoneId} g:== {targetMapZoneIdGoal}",
                    "up-set-target-object search-local c: 0",
                    $"up-get-object-data {Game.ObjectDataMapZoneId} {unitMapZoneGoal}",
                    $"up-remove-objects search-local {Game.ObjectDataMapZoneId} g:!= {unitMapZoneGoal}",
                    $"up-get-point position-object {targetPointGoal}",
                    $"up-set-target-point {targetPointGoal}",
                        
                    "up-reset-filters",
                    $"up-find-remote c: {Game.TransportShipClassId} c: 40",
                    $"up-remove-objects search-remote {Game.ObjectDataGarrisonCount} c:>= {TransportShipCapacity}",
                    $"up-clean-search search-remote {Game.ObjectDataDistance} 1",
                    "up-set-target-object search-remote c: 0",
                    "up-target-objects 1 action-garrison -1 -1",
                ]),
            new Defrule(
                [
                    "soldier-count >= 1",
                    "unit-type-count transport-ship >= 1",
                ],
                [
                    "up-full-reset-search",
                    "up-modify-sn sn-focus-player-number s:= sn-target-player-number",
                    $"up-find-local c: {Game.TransportShipClassId} c: 40",
                    $"up-remove-objects search-local {Game.ObjectDataGarrisonCount} c:< 5",
                    $"up-get-point position-target {targetPointGoal}",
                    $"up-target-point {targetPointGoal} action-unload -1 -1",
                ]),
        };

        context.FreeVolatileGoal(targetMapZoneIdGoal);
        context.FreeVolatileGoal(myPlayerNumberGoal);
        context.FreeVolatileGoal(unitMapZoneGoal);
        context.FreeVolatilePointGoal(targetPointGoal);
        
        context.AddToScript(context.ApplyStacks(rules));
    }
}
