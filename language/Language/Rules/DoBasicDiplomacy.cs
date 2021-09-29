using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules
{
    [ActiveRule]
    public class DoBasicDiplomacy : RuleBase
    {
        public override string Name => "do basic diplomacy";

        public override string Help => "Includes rules to manage open diplomacy games. Neutrals everyone, makes one ally, makes one enemy. Will set an attacking player to enemy.";

        public override string Usage => "do basic diplomacy";

        public DoBasicDiplomacy()
            : base(@"^do basic diplomacy$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
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

            rules.Add(new Defrule(
                new[]
                {
                    "true",
                },
                new[]
                {
                    "generate-random-number 8",
                }));

            // no enemies, make a neutral player our enemy.
            for (var player = 1; player <= 8; player++)
            {
                rules.Add(new Defrule(
                    new[]
                    {
                        $"random-number == {player}",
                        "not (player-in-game any-enemy)",
                        $"stance-toward {player} neutral",
                        $"not (players-stance {player} ally)",
                    },
                    new[]
                    {
                        $"set-stance {player} enemy",
                    }));
            }

            // still no enemies, make a neutral player our enemy even if they've allied us.
            for (var player = 1; player <= 8; player++)
            {
                rules.Add(new Defrule(
                    new[]
                    {
                        $"random-number == {player}",
                        "not (player-in-game any-enemy)",
                        $"stance-toward {player} neutral",
                    },
                    new[]
                    {
                        $"set-stance {player} enemy",
                    }));
            }

            // still no enemies, backstab an ally.
            for (var player = 1; player <= 8; player++)
            {
                rules.Add(new Defrule(
                    new[]
                    {
                        $"random-number == {player}",
                        "not (player-in-game any-enemy)",
                        $"not (stance-toward {player} enemy)",
                        $"stance-toward {player} ally",
                    },
                    new[]
                    {
                        $"set-stance {player} enemy",
                    }));
            }

            // no allies, ally somone who has allied us.
            for (var player = 1; player <= 8; player++)
            {
                rules.Add(new Defrule(
                    new[]
                    {
                        $"random-number == {player}",
                        "not (player-in-game any-ally)",
                        "players-stance any-neutral ally",
                        $"not (stance-toward {player} ally)",
                        $"players-stance {player} ally",
                    },
                    new[]
                    {
                        $"set-stance {player} ally",
                    }));
            }

            // still no allies, ally a neutral
            for (var player = 1; player <= 8; player++)
            {
                rules.Add(new Defrule(
                    new[]
                    {
                        $"random-number == {player}",
                        "not (player-in-game any-ally)",
                        "players-stance any-neutral neutral",
                        $"stance-toward {player} neutral",
                    },
                    new[]
                    {
                        $"set-stance {player} ally",
                    }));
            }

            // if we have an enemy, ally neutral players who ally us.
            for (var player = 1; player <= 8; player++)
            {
                rules.Add(new Defrule(
                    new[]
                    {
                        "player-in-game any-enemy",
                        $"players-stance {player} ally",
                        $"stance-toward {player} neutral",
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

            for (var player = 1; player <= 8; player++)
            {
                rules.Add(new Defrule(
                    new[]
                    {
                        $"goal {threatPlayerGoal} {player}",
                        $"not (stance-toward {player} enemy)"
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
