using Language.ScriptItems;

namespace Language.Rules
{
    [ActiveRule]
    public class When : RuleBase
    {
        public override string Name => "when";

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
