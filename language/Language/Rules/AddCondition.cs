using Language.ScriptItems;

namespace Language.Rules
{
    [ActiveRule]
    public class AddCondition : RuleBase
    {
        public override string Name => "add condition";

        public override string Help => "Adds a condition to the condition stack. 'If' is preferred.";

        public override string Usage => @"#add condition CONDITION
    RULES
#remove condition";

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
