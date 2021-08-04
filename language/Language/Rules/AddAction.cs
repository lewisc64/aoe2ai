using Language.ScriptItems;

namespace Language.Rules
{
    [ActiveRule]
    public class AddAction : Rule
    {
        public AddAction()
            : base(@"^#add action (?<condition>.+)|#remove action")
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
