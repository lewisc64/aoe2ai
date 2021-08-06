using Language.ScriptItems;

namespace Language.Rules
{
    [ActiveRule]
    public class Attack : RuleBase
    {
        public override string Name => "attack";

        public override string Help => "Makes use of the attack-now action.";

        public override string Usage => "attack with AMOUNT units";

        public Attack()
            : base(@"^attack(?: with (?<amount>[^ ]+) units)?$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var amount = GetData(line)["amount"].Value;

            Defrule rule;

            if (string.IsNullOrEmpty(amount))
            {
                rule = new Defrule(
                    new[] { "true" },
                    new[] { "attack-now" });
            }
            else
            {
                rule = new Defrule(
                    new[] { $"military-population >= {amount}" },
                    new[] { "attack-now" });
            }

            context.AddToScript(context.ApplyStacks(rule));
        }
    }
}
