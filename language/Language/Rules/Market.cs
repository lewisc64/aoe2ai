using Language.ScriptItems;

namespace Language.Rules
{
    [ActiveRule]
    public class Market : RuleBase
    {
        public override string Name => "market";

        public Market()
            : base(@"^(?<action>buy|sell) (?<resource>[^ ]+) when (?<testresource>[^ +]+) (?<comparison>[<>!=]+) (?<amount>[^ ]+)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var action = data["action"].Value;
            var resource = data["resource"].Value;
            var testResource = data["testresource"].Value;
            var comparison = data["comparison"].Value;
            var amount = data["amount"].Value;

            var rule = new Defrule(
                new[]
                {
                    $"{testResource}-amount {comparison} {amount}",
                    $"can-{action}-commodity {resource}",
                },
                new[]
                {
                    $"{action}-commodity {resource}",
                });

            context.AddToScript(context.ApplyStacks(rule));
        }
    }
}
