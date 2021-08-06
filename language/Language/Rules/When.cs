using Language.ScriptItems;

namespace Language.Rules
{
    [ActiveRule]
    public class When : RuleBase
    {
        public override string Name => "when";

        public override string Help => "Rules in the 'then' block are allowed to trigger when any rule in the main 'when' block is triggered.";

        public override string Usage => @"#when
    RULE
#then ?always
    RULES
#end when";

        public override string Example => @"#when
    build houses
#then
    chat to all ""I built a house!""
#end when";

        public When()
            : base(@"^(?:#when|#then(?<always> always)?|#end when)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            if (line.StartsWith("#when"))
            {
                var goalNumber = context.CreateGoal();

                context.AddToScript(new Defrule(new[] { "true" }, new[] { $"set-goal {goalNumber} 0", "disable-self" }));

                context.ActionStack.Push(new Action($"set-goal {goalNumber} 1"));
                context.DataStack.Push(goalNumber);
            }
            else if (line.StartsWith("#then"))
            {
                var goalNumber = context.DataStack.Peek();

                context.DataStack.Push(GetData(line)["always"].Success);
                context.ActionStack.Pop();
                context.ConditionStack.Push(new Condition($"goal {goalNumber} 1"));
            }
            else
            {
                var always = (bool)context.DataStack.Pop();
                var goalNumber = context.DataStack.Pop();
                context.ConditionStack.Pop();

                if (!always)
                {
                    context.AddToScript(new Defrule(new[] { "true" }, new[] { $"set-goal {goalNumber} 0" }));
                }
            }
        }
    }
}
