using Language.ScriptItems;
using System.Collections.Generic;
using System.Linq;

namespace Language.Rules
{
    [ActiveRule]
    public class SetEscrow : RuleBase
    {
        public override string Name => "set escrow";

        public override string Help => "Creates rule that sets the escrow percentage.";

        public override string Usage => "escrow PERCENTAGE RESOURCE_NAME";

        public override IEnumerable<string> Examples => new[]
        {
            "escrow 10 wood",
            "escrow 50 gold with maximum 300",
        };

        public SetEscrow()
            : base(@"^escrow (?<amount>[^ ]+) (?<resource>[^ ]+)(?: with maximum (?<maximum>[^ ]+))?$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var amount = data["amount"].Value;
            var resource = data["resource"].Value;

            var rules = new List<Defrule>();

            rules.Add(new Defrule(new[] { "true" }, new[] { $"set-escrow-percentage {resource} {amount}" }));

            if (data["maximum"].Success)
            {
                var maximum = data["maximum"].Value;
                rules.Last().Conditions.Add(new Condition($"escrow-amount {resource} < {maximum}"));
                rules.Add(new Defrule(new[] { $"escrow-amount {resource} >= {maximum}" }, new[] { $"set-escrow-percentage {resource} 0", $"up-modify-escrow {resource} c:= {maximum}" }));
            }

            context.AddToScript(context.ApplyStacks(rules));
        }
    }
}
