using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules
{
    [ActiveRule]
    public class SetName : RuleBase
    {
        public override string Name => "set name";

        public override string Help => "Sets the name of the AI.";

        public override string Usage => "set name to \"NAME\"";

        public override IEnumerable<string> Examples => new[]
        {
            "set name to \"Cool AI\"",
        };

        public SetName()
            : base(@"^set name to ""(?<name>.+)""$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var name = data["name"].Value;

            var rule = new Defrule(new[] { "true" }, new[] { $"up-change-name \"{name}\"" });
            context.AddToScript(context.ApplyStacks(rule));
        }
    }
}
