using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules
{
    [ActiveRule]
    public class CreateAction : RuleBase
    {
        public override string Name => "create action";

        public override string Help => "Creates a rule with the action contained within.";

        public override string Usage => "@ACTION";

        public override IEnumerable<string> Examples => new[]
        {
            "@up-get-fact military-population 0 1",
        };

        public CreateAction()
            : base(@"^@(?<action>.+)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var rule = new Defrule(new[] { "true" }, new[] { GetData(line)["action"].Value });
            context.AddToScript(context.ApplyStacks(rule));
        }
    }
}
