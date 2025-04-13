using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules;

[ActiveRule]
public class DefendWithDuc : RuleBase
{
    public override string Name => "defend with duc";

    public override string Help => @"Moves untasked far units towards the town, and attack-moves enemy units within.

Affects the following sn's:
 - sn-disable-defend-groups (sets to 1)
 - sn-number-attack-groups (sets to 0)
 - sn-gather-defense-units (sets to 0)";

    public override string Usage => "defend with duc";

    public override IEnumerable<string> Examples => [
        "defend with duc",
    ];

    public DefendWithDuc()
        : base(@"^defend with duc$")
    {
    }

    public override void Parse(string line, TranspilerContext context)
    {
        var point = context.CreateVolatilePointGoal();

        var rules = new[]
        {
            new Defrule(
                [
                    "true",
                ],
                [
                    "set-strategic-number sn-disable-defend-groups 1",
                    "set-strategic-number sn-number-attack-groups 0",
                    "set-strategic-number sn-gather-defense-units 0",
                    $"up-get-point position-self {point}",
                    $"up-set-target-point {point}",
                ]),
            new Defrule(
                [
                    new Condition("true"),
                ],
                [
                    new Action("up-full-reset-search"),
                    new Action("up-filter-exclude -1 -1 orderid-explore -1"),
                    new Action("up-filter-range -1 -1 30 -1"),
                    
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
                    new Action("up-find-local c: packed-trebuchet-class c: 255"),
                    new Action("up-find-local c: unpacked-trebuchet-class c: 255"),
                    new Action($"up-remove-objects search-local {Game.ObjectDataIdling} == 0"),
                    new Action($"up-target-point {point} {Game.ActionAttackMove} -1 stance-aggressive"),
                ]),
        };

        context.FreeVolatilePointGoal(point);
        
        context.AddToScript(context.ApplyStacks(rules));
    }
}
