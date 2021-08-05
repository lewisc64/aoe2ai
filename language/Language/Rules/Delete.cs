using Language.ScriptItems;

namespace Language.Rules
{
    [ActiveRule]
    public class Delete : RuleBase
    {
        public override string Name => "delete";

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
