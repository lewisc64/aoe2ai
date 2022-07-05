using Language.Extensions;
using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules
{
    [ActiveRule]
    public class If : RuleBase
    {
        public override string Name => "if";

        public override string Help => "Adds a condition to the condition stack.";

        public override string Usage => @"#if CONDITION
    RULES
#else if CONDITION
    RULES
#else
    RULES
#end if";

        public override IEnumerable<string> Examples => new[]
        {
            @"#if goal 1 1
    chat to all ""goal 1 is 1!""
#end if",
            @"#if current-age == dark-age
    chat to all ""dark age""
#else if current-age == feudal-age
    chat to all ""feudal age""
#else
    chat to all ""castle age or imperial age""
#end if",
        };

        public If()
            : base(@"^(#if (?<ifcondition>.+)|#else if (?<elseifcondition>.+)|#else|#end if)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);

            if (line.StartsWith("#if"))
            {
                var condition = Condition.Parse(data["ifcondition"].Value);
                context.ConditionStack.Push(condition);
                context.DataStack.Push(condition);
                context.DataStack.Push(1);
            }
            else if (line.StartsWith("#else"))
            {
                var amount = (int)context.DataStack.Pop();

                var conditions = new List<Condition>();

                for (var i = 0; i < amount; i++)
                {
                    context.ConditionStack.Pop();
                    conditions.Add((Condition)context.DataStack.Pop());
                }

                foreach (var condition in conditions)
                {
                    if (condition is CombinatoryCondition combCondition && combCondition.Text == "or")
                    {
                         context.ConditionStack.Push(condition.DeMorgans().Invert());
                    }
                    else
                    {
                        context.ConditionStack.Push(condition.Invert());
                    }
                    context.DataStack.Push(condition);
                }

                if (line.StartsWith("#else if"))
                {
                    var condition = Condition.Parse(data["elseifcondition"].Value);
                    context.ConditionStack.Push(condition);
                    context.DataStack.Push(condition);
                    context.DataStack.Push(amount + 1);
                }
                else
                {
                    context.DataStack.Push(amount);
                }
            }
            else
            {
                var amount = (int)context.DataStack.Pop();
                for (var i = 0; i < amount; i++)
                {
                    context.ConditionStack.Pop();
                    context.DataStack.Pop();
                }
            }
        }
    }
}
