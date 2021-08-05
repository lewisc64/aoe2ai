using Language.ScriptItems;

namespace Language.Rules
{
    [ActiveRule]
    public class CreateAction : RuleBase
    {
        public override string Name => "create action";

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
