using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules
{
    [ActiveRule]
    public class DoBasicDiplomacy : RuleBase
    {
        public override string Name => "do basic diplomacy";

        public override string Help => "Includes rules to manage open diplomacy games. Neutrals everyone, makes one ally, makes one enemy. Will set an attacking player to enemy.";

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

            var allyCountGoal = context.CreateGoal();
            var neutralCountGoal = context.CreateGoal();
            var enemyCountGoal = context.CreateGoal();

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
                        $"random-number == {player}",
                        $"goal {enemyCountGoal} 0",
                        $"stance-toward {player} neutral",
                        $"not (players-stance {player} ally)",
                        $"player-in-game {player}",
                    },
                    new[]
                    {
                        $"set-stance {player} enemy",
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
                        $"player-in-game {player}",
                    },
                    new[]
                    {
                        $"set-stance {player} enemy",
                    }));
            }

            // don't make more than one ally if we aren't backstabbing
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

                // if we have an enemy, ally neutral players who ally us.
                for (var player = 1; player <= 8; player++)
                {
                    stanceChangeRules.Add(new Defrule(
                        new[]
                        {
                            $"not (goal {enemyCountGoal} 0)",
                            $"players-stance {player} ally",
                            $"stance-toward {player} neutral",
                            $"player-in-game {player}",
                        },
                        new[]
                        {
                            $"set-stance {player} ally",
                        }));
                }
            }

            // no allies, ally somone who has allied us.
            for (var player = 1; player <= 8; player++)
            {
                stanceChangeRules.Add(new Defrule(
                    new[]
                    {
                        $"random-number == {player}",
                        $"goal {allyCountGoal} 0",
                        "players-stance any-neutral ally",
                        $"not (stance-toward {player} ally)",
                        $"players-stance {player} ally",
                        $"player-in-game {player}",
                    },
                    new[]
                    {
                        $"set-stance {player} ally",
                    }));
            }

            // still no allies, ally a neutral
            for (var player = 1; player <= 8; player++)
            {
                stanceChangeRules.Add(new Defrule(
                    new[]
                    {
                        $"random-number == {player}",
                        $"goal {allyCountGoal} 0",
                        "players-stance any-neutral neutral",
                        $"stance-toward {player} neutral",
                        $"player-in-game {player}",
                    },
                    new[]
                    {
                        $"set-stance {player} ally",
                    }));
            }

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
                        $"players-stance {player} ally",
                        $"not (stance-toward {player} ally)",
                        $"not (player-in-game {player})",
                    },
                    new[]
                    {
                        $"set-stance {player} ally",
                    }));
            }

            var threatPlayerGoal = context.CreateGoal();

            rules.Add(new Defrule(
                new[]
                {
                    "true",
                },
                new[]
                {
                    $"up-get-threat-data -1 {threatPlayerGoal} -1 -1",
                }));

            // enemy those who attack us
            for (var player = 1; player <= 8; player++)
            {
                rules.Add(new Defrule(
                    new[]
                    {
                        $"goal {threatPlayerGoal} {player}",
                        $"not (stance-toward {player} enemy)",
                    },
                    new[]
                    {
                        $"set-stance {player} enemy",
                    }));
            }

            context.AddToScript(context.ApplyStacks(rules));
        }
    }
}
