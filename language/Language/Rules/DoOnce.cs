using Language.ScriptItems;

namespace Language.Rules
{
    [ActiveRule]
    public class DoOnce : RuleBase
    {
        public override string Name => "do once";

        public override string Help => "Adds 'disable-self' to the action stack. Makes sure each rule in the block individually runs only once.";

        public override string Usage => @"#do once
    RULES
#end do";

        public DoOnce()
            : base(@"^(?:#do once|#end do(?: once)?)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            if (line.StartsWith("#do"))
            {
                context.ActionStack.Push(new Action("disable-self"));
            }
            else
            {
                context.ActionStack.Pop();
            }
        }
    }
}
