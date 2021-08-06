using Language.ScriptItems;

namespace Language.Rules
{
    [ActiveRule]
    public class SendScout : RuleBase
    {
        public override string Name => "attack";

        public override string Help => "Sets up the userpatch rule to send the scout somewhere else.";

        public override string Usage => "scout AREA_NAME";

        public override string Example => @"scout opposite
scout enemy";

        public SendScout()
            : base(@"^scout (?<location>.+)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var location = GetData(line)["location"].Value;
            var rule = new Defrule(new[] { "true" }, new[] { $"up-send-scout group-type-land-explore scout-{location}" });
            context.AddToScript(context.ApplyStacks(rule));
        }
    }
}
