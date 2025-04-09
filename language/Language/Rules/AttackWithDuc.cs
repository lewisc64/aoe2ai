using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules;

[ActiveRule]
public class AttackWithDuc : RuleBase
{
    public override string Name => "attack with duc";

    public override string Help => @"Attacks the target player with all military units using the attack move command.

Affects the following sn's:
 - sn-disable-defend-groups (sets to 1)
 - sn-number-attack-groups (sets to 0)
 - sn-gather-defense-units (sets to 0)";

    public override string Usage => "attack with duc";

    public override IEnumerable<string> Examples => [
        "attack with duc",
    ];

    public AttackWithDuc()
        : base(@"^attack with duc$")
    {
    }

    public override void Parse(string line, TranspilerContext context)
    {
        var point = context.CreateVolatilePointGoal();
        var unstickTimer = context.CreateTimer();

        var rules = new[]
        {
            new Defrule(
                [
                    "player-in-game target-player",
                ],
                [
                    "set-strategic-number sn-disable-defend-groups 1",
                    "set-strategic-number sn-number-attack-groups 0",
                    "set-strategic-number sn-gather-defense-units 0",
                    $"up-get-point position-target {point}",
                ]),
            new Defrule(
                [
                    new Condition("player-in-game target-player"),
                    Condition.JoinConditions(
                        "nand",
                        [
                            new Condition($"up-compare-goal {point} c:<= 0"),
                            new Condition($"up-compare-goal {point + 1} c:<= 0"),
                        ]),
                ],
                [
                    new Action("up-full-reset-search"),
                    new Action("up-filter-exclude -1 -1 orderid-explore -1"),
                    
                    new Action("up-find-local c: archery-class c: 255"),
                    new Action("up-find-local c: infantry-class c: 255"),
                    new Action("up-find-local c: petard-class c: 255"),
                    new Action("up-find-local c: archery-cannon-class c: 255"),
                    new Action($"up-remove-objects search-local {Game.ObjectDataIdling} == 0"),
                    new Action($"up-target-point {point} {Game.ActionAttackMove} -1 stance-aggressive"),
                    
                    new Action("up-reset-search 1 1 0 0"),
                    new Action("up-find-local c: cavalry-class c: 255"),
                    new Action("up-find-local c: cavalry-archer-class c: 255"),
                    new Action("up-find-local c: cavalry-cannon-class c: 255"),
                    new Action($"up-find-local c: {Game.ScoutCavalryClassId} c: 255"),
                    new Action($"up-remove-objects search-local {Game.ObjectDataIdling} == 0"),
                    new Action($"up-target-point {point} {Game.ActionAttackMove} -1 stance-aggressive"),
                    
                    new Action("up-reset-search 1 1 0 0"),
                    new Action("up-find-local c: siege-weapon-class c: 255"),
                    new Action("up-find-local c: monastery-class c: 255"),
                    new Action("up-find-local c: scorpion-class c: 255"),
                    new Action($"up-remove-objects search-local {Game.ObjectDataIdling} == 0"),
                    new Action($"up-target-point {point} {Game.ActionAttackMove} -1 stance-aggressive"),
                ]),
            new Defrule(
                [
                    "player-in-game target-player",
                    "unit-type-count trebuchet-set >= 1",
                ],
                [
                    "up-full-reset-search",
                    "up-find-local c: packed-trebuchet-class c: 255",
                    "up-find-local c: unpacked-trebuchet-class c: 255",
                    "up-modify-sn sn-focus-player-number s:= sn-target-player-number",
                    "up-find-remote c: castle c: 255",
                    "up-find-remote c: town-center c: 255",
                    "up-find-remote c: tower-class c: 255",
                    "up-find-remote c: donjon c: 255",
                    "up-find-remote c: krepost c: 255",
                    $"up-get-point position-self {point}",
                    $"up-set-target-point {point}",
                    $"up-clean-search search-remote {Game.ObjectDataDistance} 1",
                    "up-set-target-object search-remote c: 0",
                    "up-target-objects 1 action-default -1 -1",
                ]),
            new Defrule(
                [
                    "player-in-game target-player",
                ],
                [
                    $"enable-timer {unstickTimer} 300",
                    "disable-self",
                ]),
            new Defrule(
                [
                    "player-in-game target-player",
                    $"timer-triggered {unstickTimer}",
                ],
                [
                    "up-reset-unit c: archery-class",
                    "up-reset-unit c: infantry-class",
                    "up-reset-unit c: cavalry-class",
                    "up-reset-unit c: siege-weapon-class",
                    "up-reset-unit c: monastery-class",
                    "up-reset-unit c: petard-class",
                    "up-reset-unit c: cavalry-archer-class",
                    "up-reset-unit c: archery-cannon-class",
                    $"up-reset-unit c: {Game.ScoutCavalryClassId}",
                    "up-reset-unit c: scorpion-class",
                    $"disable-timer {unstickTimer}",
                    $"enable-timer {unstickTimer} 300",
                ]),
        };

        context.FreeVolatilePointGoal(point);
        
        context.AddToScript(context.ApplyStacks(rules));
    }
}
