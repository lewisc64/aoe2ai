using Language.ScriptItems;

namespace Language.Rules
{
    [ActiveRule]
    public class AddAction : RuleBase
    {
        public override string Name => "add action";

        public override string Help => "Adds an action to the action stack.";

        public override string Usage => @"#add action ACTION
    RULES
#remove action";

        public AddAction()
            : base(@"^(?:#add action (?<condition>.+)|#remove action)")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            if (line.StartsWith("#remove"))
            {
                context.ActionStack.Pop();
            }
            else
            {
                var action = GetData(line)["condition"].Value;
                context.ActionStack.Push(new Action(action));
            }
        }
    }
}
