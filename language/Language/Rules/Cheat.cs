using Language.ScriptItems;

namespace Language.Rules
{
    [ActiveRule]
    public class Cheat : RuleBase
    {
        public override string Name => "cheat";

        public override string Help => "Gives the AI resources";

        public override string Usage => "cheat AMOUNT RESOURCE_NAME";

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
