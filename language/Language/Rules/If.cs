using Language.Extensions;
using Language.ScriptItems;
using System.Collections.Generic;
using System.Linq;

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
            @"#ifg goal 5 1
    chat to all ""goal 5 is 1""
    @set-goal 5 2
    #if goal 5 2
        chat to all ""This will still execute, because the condition `goal 5 1` was frozen into a goal due to `ifg`.""
    #end if
#end if",
        };

        public If()
            : base(@"^(#if(?<ifusegoal>g)? (?<ifcondition>.+)|#else if(?<elseifusegoal>g)? (?<elseifcondition>.+)|#else|#end if)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);

            if (line.StartsWith("#if"))
            {
                var ifData = new IfData
                {
                    CurrentCondition = Condition.Parse(data["ifcondition"].Value),
                    OldConditions = new(),
                    VolatileGoalsUsed = new(),
                };

                if (data["ifusegoal"].Success)
                {
                    FreezeToGoal(context, ifData);
                }

                context.ConditionStack.Push(ifData.CurrentCondition);
                context.DataStack.Push(ifData);
            }
            else if (line.StartsWith("#else"))
            {
                var ifData = (IfData)context.DataStack.Peek();
                for (var i = 0; i < ifData.OldConditions.Count; i++)
                {
                    context.ConditionStack.Pop();
                }
                ifData.OldConditions.Add(context.ConditionStack.Pop());


                if (line.StartsWith("#else if"))
                {
                    ifData.CurrentCondition = Condition.Parse(data["elseifcondition"].Value);
                    if (data["elseifusegoal"].Success)
                    {
                        FreezeToGoal(context, ifData);
                    }
                    context.ConditionStack.Push(ifData.CurrentCondition);
                }
                else
                {
                    ifData.CurrentCondition = null;
                }

                foreach (var condition in ifData.OldConditions.Reverse<Condition>())
                {

                    if (condition is CombinatoryCondition combCondition && combCondition.Text == "or")
                    {
                        context.ConditionStack.Push(condition.DeMorgans().Invert());
                    }
                    else
                    {
                        context.ConditionStack.Push(condition.Invert());
                    }
                }
            }
            else
            {
                var ifData = (IfData)context.DataStack.Pop();

                if (ifData.CurrentCondition is not null)
                {
                    context.ConditionStack.Pop();
                }
                for (var i = 0; i < ifData.OldConditions.Count; i++)
                {
                    context.ConditionStack.Pop();
                }

                context.FreeVolatileGoals(ifData.VolatileGoalsUsed);
            }
        }

        private void FreezeToGoal(TranspilerContext context, IfData ifData)
        {
            var goal = context.CreateVolatileGoal();

            var rules = new[]
            {
                new Defrule(new[] { "true" }, new[] { $"set-goal {goal} 0" }),
                new Defrule(new[] { ifData.CurrentCondition }, new[] { new Action($"set-goal {goal} 1") }),
            };

            context.AddToScript(context.ApplyStacks(rules));
            ifData.VolatileGoalsUsed.Add(goal);
            ifData.CurrentCondition = new Condition($"goal {goal} 1");
        }

        private class IfData
        {
            public Condition CurrentCondition { get; set; }

            public List<Condition> OldConditions { get; set; }

            public List<int> VolatileGoalsUsed { get; set; }
        }
    }
}
