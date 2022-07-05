using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules
{
    [ActiveRule]
    public class DoOnce : RuleBase
    {
        public override string Name => "do once";

        public override string Help => "Adds 'disable-self' to the action stack. Makes sure each rule in the block individually runs only once. Can specify 'grouped' afterwards to additionaly switch on a goal.";

        public override string Usage => @"#do once ?grouped
    RULES
#end do";

        public override IEnumerable<string> Examples => new[]
        {
            @"#do once
    chat to all ""hello""
#end do",
            @"#do once grouped
    chat to all ""hello""
#end do",
        };

        public DoOnce()
            : base(@"^(?:#do once(?<grouped> grouped)?|#end do(?: once)?)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            if (line.StartsWith("#do"))
            {
                var grouped = !string.IsNullOrEmpty(GetData(line)["grouped"].Value);

                context.ActionStack.Push(new Action("disable-self"));

                if (grouped)
                {
                    var goal = context.CreateGoal();
                    context.AddToScript(new Defrule(new[] { "true" }, new[] { $"set-goal {goal} 1", "disable-self" }));

                    context.ConditionStack.Push(new Condition($"goal {goal} 1"));
                    context.DataStack.Push(goal);
                }
                context.DataStack.Push(grouped);
            }
            else
            {
                var grouped = (bool)context.DataStack.Pop();
                context.ActionStack.Pop();
                if (grouped)
                {
                    context.ConditionStack.Pop();

                    var goal = (int)context.DataStack.Pop();
                    context.AddToScript(context.ApplyStacks(new Defrule(new[] { "true" }, new[] { $"set-goal {goal} 0", "disable-self" })));
                }
            }
        }
    }
}
