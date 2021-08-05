using Language.ScriptItems;

namespace Language.Rules
{
    [ActiveRule]
    public class Cheat : RuleBase
    {
        public override string Name => "cheat";

        public Cheat()
            : base(@"^cheat (?<amount>[^ ]+) (?<resource>[^ ]+)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var amount = data["amount"].Value;
            var resource = data["resource"].Value;

            context.AddToScript(context.ApplyStacks(new Defrule(new[] { "true" }, new[] { $"cc-add-resource {resource} {amount}" })));
        }
    }
}
