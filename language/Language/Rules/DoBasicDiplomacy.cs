using Language.Extensions;
using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules
{
    [ActiveRule]
    public class DoBasicDiplomacy : RuleBase
    {
        public override string Name => "do basic diplomacy";

        public override string Help => "Includes rules to manage open diplomacy games. Tries to maintain one enemy.";

        public override string Usage => @"do basic diplomacy
do basic diplomacy without backstabbing";

        public DoBasicDiplomacy()
            : base(@"^do basic diplomacy(?<nobackstab> without backstabbing)?$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var doBackstabbing = string.IsNullOrEmpty(GetData(line)["nobackstab"].Value);

            var rules = new List<Defrule>();

            rules.Add(new Defrule(
                new[]
                {
                    "true",
                },
                new[]
                {
                    "set-stance every-enemy neutral",
                    "disable-self",
                }));

            var allyCountGoal = context.CreateVolatileGoal();
            var neutralCountGoal = context.CreateVolatileGoal();
            var enemyCountGoal = context.CreateVolatileGoal();

            rules.Add(new Defrule(
                new[]
                {
                    "true",
                },
                new[]
                {
                    "generate-random-number 8",
                    $"set-goal {allyCountGoal} 0",
                    $"set-goal {neutralCountGoal} 0",
                    $"set-goal {enemyCountGoal} 0",
                }));

            for (var player = 1; player <= 8; player++)
            {
                rules.Add(new Defrule(new[] { $"player-in-game {player}", $"stance-toward {player} ally" }, new[] { $"up-modify-goal {allyCountGoal} c:+ 1" }));
                rules.Add(new Defrule(new[] { $"player-in-game {player}", $"stance-toward {player} neutral" }, new[] { $"up-modify-goal {neutralCountGoal} c:+ 1" }));
                rules.Add(new Defrule(new[] { $"player-in-game {player}", $"stance-toward {player} enemy" }, new[] { $"up-modify-goal {enemyCountGoal} c:+ 1" }));
            }

            rules.Add(new Defrule(new[] { $"stance-toward my-player-number ally" }, new[] { $"up-modify-goal {allyCountGoal} c:- 1" }));
            rules.Add(new Defrule(new[] { $"stance-toward my-player-number neutral" }, new[] { $"up-modify-goal {neutralCountGoal} c:- 1" }));
            rules.Add(new Defrule(new[] { $"stance-toward my-player-number enemy" }, new[] { $"up-modify-goal {enemyCountGoal} c:- 1" }));

            var stanceChangeRules = new List<Defrule>();

            // no enemies, make a neutral player our enemy.
            for (var player = 1; player <= 8; player++)
            {
                stanceChangeRules.Add(new Defrule(
                    new[]
                    {
                        new Condition($"random-number == {player}"),
                        new Condition($"goal {enemyCountGoal} 0"),
                        new Condition($"stance-toward {player} neutral"),
                        new Condition($"players-stance {player} ally").Invert(),
                        new Condition($"player-in-game {player}"),
                    },
                    new[]
                    {
                        new Action($"set-stance {player} enemy"),
                    }));
            }

            // still no enemies, make a neutral player our enemy even if they've allied us.
            for (var player = 1; player <= 8; player++)
            {
                stanceChangeRules.Add(new Defrule(
                    new[]
                    {
                        $"random-number == {player}",
                        $"goal {enemyCountGoal} 0",
                        $"stance-toward {player} neutral",
                        $"players-stance {player} ally",
                        $"player-in-game {player}",
                    },
                    new[]
                    {
                        $"set-stance {player} enemy",
                    }));
            }

            if (doBackstabbing)
            {
                // still no enemies, backstab an ally.
                for (var player = 1; player <= 8; player++)
                {
                    stanceChangeRules.Add(new Defrule(
                        new[]
                        {
                            $"random-number == {player}",
                            $"goal {enemyCountGoal} 0",
                            $"goal {neutralCountGoal} 0",
                            $"player-in-game {player}",
                        },
                        new[]
                        {
                            $"set-stance {player} enemy",
                        }));
                }
            }
            else
            {
                // still no enemies, backstab an ally, but do not betray our last ally.
                for (var player = 1; player <= 8; player++)
                {
                    stanceChangeRules.Add(new Defrule(
                        new[]
                        {
                            $"random-number == {player}",
                            $"goal {enemyCountGoal} 0",
                            $"goal {neutralCountGoal} 0",
                            $"up-compare-goal {allyCountGoal} >= 2",
                            $"player-in-game {player}",
                        },
                        new[]
                        {
                            $"set-stance {player} enemy",
                        }));
                }
            }

            // if we have an enemy, ally all non-hostile neutrals.
            for (var player = 1; player <= 8; player++)
            {
                stanceChangeRules.Add(new Defrule(
                    new[]
                    {
                        new Condition($"random-number == {player}"),
                        new Condition($"goal {enemyCountGoal} 0").Invert(),
                        new Condition($"stance-toward {player} neutral"),
                        new Condition($"players-stance {player} enemy").Invert(),
                        new Condition($"player-in-game {player}"),
                    },
                    new[]
                    {
                        new Action($"set-stance {player} ally"),
                    }));
            }

            // try to ally enemies if we have more than one enemy (but only once per player).
            // threat-player checks below will set them back to enemy if they decline.
            for (var player = 1; player <= 8; player++)
            {
                stanceChangeRules.Add(new Defrule(
                    new[]
                    {
                        $"random-number == {player}",
                        $"up-compare-goal {enemyCountGoal} >= 2",
                        $"stance-toward {player} enemy",
                        $"players-stance {player} enemy",
                        $"player-in-game {player}",
                    },
                    new[]
                    {
                        $"set-stance {player} ally",
                        "disable-self",
                    }));
            }

            // accept enemy alliance requests if we have more than one enemy
            for (var player = 1; player <= 8; player++)
            {
                stanceChangeRules.Add(new Defrule(
                    new[]
                    {
                        new Condition($"random-number == {player}"),
                        new Condition($"up-compare-goal {enemyCountGoal} >= 2"),
                        new Condition($"stance-toward {player} ally").Invert(),
                        new Condition($"players-stance {player} ally"),
                        new Condition($"player-in-game {player}"),
                    },
                    new[]
                    {
                        new Action($"set-stance {player} ally"),
                    }));
            }

            // enemy neutrals who enemy us.
            for (var player = 1; player <= 8; player++)
            {
                stanceChangeRules.Add(new Defrule(
                    new[]
                    {
                        $"random-number == {player}",
                        $"stance-toward {player} neutral",
                        $"players-stance {player} enemy",
                        $"player-in-game {player}",
                    },
                    new[]
                    {
                        $"set-stance {player} enemy",
                    }));
            }

            context.FreeVolatileGoal(allyCountGoal);
            context.FreeVolatileGoal(neutralCountGoal);
            context.FreeVolatileGoal(enemyCountGoal);

            for (var i = 0; i < stanceChangeRules.Count - 1; i++)
            {
                stanceChangeRules[i].Actions.Add(new Action($"up-jump-rule {stanceChangeRules.Count - i - 1}"));
            }

            rules.AddRange(stanceChangeRules);

            // ally everyone upon resigning
            rules.Add(new Defrule(
                new[]
                {
                    "game-time >= 1200",
                    "population < 10",
                },
                new[]
                {
                    "set-stance every-enemy ally",
                    "set-stance every-neutral ally",
                    "resign",
                }));

            // ally those who have resigned and allied us
            for (var player = 1; player <= 8; player++)
            {
                rules.Add(new Defrule(
                    new[]
                    {
                        new Condition($"players-stance {player} ally"),
                        new Condition($"stance-toward {player} ally").Invert(),
                        new Condition($"player-in-game {player}").Invert(),
                    },
                    new[]
                    {
                        new Action($"set-stance {player} ally"),
                    }));
            }

            var threatTimeGoal = context.CreateVolatileGoal();
            var threatPlayerGoal = context.CreateVolatileGoal();

            rules.Add(new Defrule(
                new[]
                {
                    "true",
                },
                new[]
                {
                    $"up-get-threat-data {threatTimeGoal} {threatPlayerGoal} -1 -1",
                }));

            // enemy those who attack us
            for (var player = 1; player <= 8; player++)
            {
                rules.Add(new Defrule(
                    new[]
                    {
                        new Condition($"goal {threatPlayerGoal} {player}"),
                        new Condition($"up-compare-goal {threatTimeGoal} c:< 1000"),
                        new Condition($"stance-toward {player} enemy").Invert(),
                    },
                    new[]
                    {
                        new Action($"set-stance {player} enemy"),
                    }));
            }

            context.FreeVolatileGoal(threatTimeGoal);
            context.FreeVolatileGoal(threatPlayerGoal);

            // always ally self
            rules.Add(new Defrule(
                new[]
                {
                    "true",
                },
                new[]
                {
                    "set-stance my-player-number ally",
                }));

            context.AddToScript(context.ApplyStacks(rules));
        }
    }
}
