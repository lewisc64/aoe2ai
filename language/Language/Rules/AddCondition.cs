using Language.ScriptItems;

namespace Language.Rules
{
    [ActiveRule]
    public class AddCondition : RuleBase
    {
        public AddCondition()
            : base(@"^#add condition (?<condition>.+)|#remove condition$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            if (line.StartsWith("#remove"))
            {
                context.ConditionStack.Pop();
            }
            else
            {
                var condition = GetData(line)["condition"].Value;
                context.ConditionStack.Push(new Condition(condition));
            }
        }
    }
}
