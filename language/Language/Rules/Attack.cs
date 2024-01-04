using Language.ScriptItems;
using System.Collections.Generic;
using System.Linq;

namespace Language.Rules
{
    [ActiveRule]
    public class Attack : RuleBase
    {
        private const int Cooldown = 60;

        public override string Name => "attack";

        public override string Help => $"Makes use of the attack-now action, with a cooldown of {Cooldown} seconds.";

        public override string Usage => "attack with AMOUNT units";

        public override IEnumerable<string> Examples => new[]
        {
            "attack",
            "attack with 30 units",
        };

        public Attack()
            : base(@"^attack(?: with (?<amount>[^ ]+) units)?$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var amount = GetData(line)["amount"].Value;

            var rules = new List<Defrule>();

            var cooldownGoal = context.CreateGoal();
            var gameTimeGoal = context.CreateVolatileGoal();

            rules.Add(new Defrule(
                new[]
                {
                    "true",
                },
                new[]
                {
                    $"set-goal {cooldownGoal} 0",
                    "disable-self",
                }));

            rules.Add(new Defrule(
                new[]
                {
                    "true",
                },
                new[]
                {
                    $"up-get-fact game-time 0 {gameTimeGoal}",
                }));

            rules.Add(new Defrule(
                new[]
                {
                    $"up-compare-goal {gameTimeGoal} g:>= {cooldownGoal}",
                },
                new[]
                {
                    "attack-now",
                    $"up-modify-goal {cooldownGoal} g:= {gameTimeGoal}",
                    $"up-modify-goal {cooldownGoal} c:+ {Cooldown}",

                }));

            context.FreeVolatileGoal(gameTimeGoal);

            if (string.IsNullOrEmpty(amount))
            {
                context.AddToScript(context.ApplyStacks(rules));
            }
            else
            {
                context.AddToScriptWithJump(context.ApplyStacks(rules), new Condition($"military-population < {amount}"));
            }
        }
    }
}
