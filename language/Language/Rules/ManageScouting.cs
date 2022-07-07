﻿using Language.Extensions;
using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules
{
    [ActiveRule]
    public class ManageScouting : RuleBase
    {
        public override string Name => "manage scouting";

        public override string Help => "Scouts using one soldier. Will scout with a villager for 10 minutes if none are available.";

        public override string Usage => "manage scouting";

        public override IEnumerable<string> Examples => new[]
        {
            "manage scouting",
        };

        public ManageScouting()
            : base(@"^manage scouting$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var rules = new[]
            {
                new Defrule(
                    new[]
                    {
                        "true",
                    },
                    new[]
                    {
                        "set-strategic-number sn-percent-civilian-explorers 0",
                        "set-strategic-number sn-cap-civilian-explorers 0",
                        "set-strategic-number sn-total-number-explorers 1",
                        "set-strategic-number sn-number-explore-groups 1",
                        "set-strategic-number sn-initial-exploration-required 0",
                        "disable-self",
                    }),
                new Defrule(
                    new[]
                    {
                        new Condition("soldier-count == 0"),
                        Condition.JoinConditions(
                            "or",
                            new[]
                            {
                                new Condition("game-time < 600"),
                                Condition.JoinConditions(
                                    "and",
                                    new[]
                                    {
                                        new Condition("resource-found gold"),
                                        new Condition("resource-found stone"),
                                    }).Invert(),
                            }),
                        new Condition("strategic-number sn-cap-civilian-explorers == 0"),
                    },
                    new[]
                    {
                        new Action("set-strategic-number sn-percent-civilian-explorers 100"),
                        new Action("set-strategic-number sn-cap-civilian-explorers 1"),
                    }),
                new Defrule(
                    new[]
                    {
                        Condition.JoinConditions(
                            "or",
                            new[]
                            {
                                new Condition("soldier-count >= 1"),
                                Condition.JoinConditions(
                                    "and",
                                    new[]
                                    {
                                        new Condition("game-time >= 600"),
                                        new Condition("resource-found gold"),
                                        new Condition("resource-found stone"),
                                    }),
                            }),
                        new Condition("strategic-number sn-cap-civilian-explorers == 1"),
                    },
                    new[]
                    {
                        new Action("set-strategic-number sn-percent-civilian-explorers 0"),
                        new Action("set-strategic-number sn-cap-civilian-explorers 0"),
                    }),
            };

            context.AddToScript(context.ApplyStacks(rules));
        }
    }
}
