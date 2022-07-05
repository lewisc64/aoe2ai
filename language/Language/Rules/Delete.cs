using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules
{
    [ActiveRule]
    public class Delete : RuleBase
    {
        public override string Name => "delete";

        public override string Help => "Creates a rule that deletes the specified object.";

        public override string Usage => "delete unit/building NAME";

        public override IEnumerable<string> Examples => new[]
        {
            "delete unit villager",
            "delete building town-center",
        };

        public Delete()
            : base(@"delete (?<type>unit|building) (?<name>.+)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var type = data["type"].Value;
            var name = data["name"].Value;

            var rule = new Defrule(new[] { "true" }, new[] { $"delete-{type} {name}" });
            context.AddToScript(context.ApplyStacks(rule));
        }
    }
}
