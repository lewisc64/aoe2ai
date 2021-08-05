using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules
{
    [ActiveRule]
    public class SelectRandom : RuleBase
    {
        public override string Name => "select random";

        public SelectRandom()
            : base(@"^#select random(?<persistant> persistant)?|#randor|#end select(?: random)?$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);

            if (line.StartsWith("#select"))
            {
                var generateRule = new Defrule();

                var persistant = data["persistant"].Success;
                var goalNumber = context.CreateGoal();

                context.DataStack.Push(persistant);
                context.DataStack.Push(2);
                context.DataStack.Push(generateRule.Id);
                context.DataStack.Push(goalNumber);

                context.ConditionStack.Push(new Condition($"goal {goalNumber} 1"));

                generateRule.Actions.Add(new Action($"set-goal {goalNumber} 0"));
                if (persistant)
                {
                    generateRule.Actions.Add(new Action("disable-self"));
                }

                context.AddToScript(context.ApplyStacks(generateRule));
            }
            else if (line.StartsWith("#end"))
            {
                context.ConditionStack.Pop();

                var goalNumber = (int)context.DataStack.Pop();
                var generateRuleId = (string)context.DataStack.Pop();
                var numberOfBlocks = (int)context.DataStack.Pop() - 1;
                var persistant = (bool)context.DataStack.Pop();

                for (var i = 0; i < context.Script.Count; i++)
                {
                    var rule = context.Script[i] as Defrule;
                    if (rule != null && rule.Id == generateRuleId)
                    {
                        rule.Actions.Add(new Action($"generate-random-number {numberOfBlocks}"));

                        for (var goalValue = 1; goalValue <= numberOfBlocks; goalValue++)
                        {
                            var conditions = new List<string>();
                            var actions = new List<string>();

                            conditions.Add($"random-number == {goalValue}");
                            conditions.Add($"goal {goalNumber} 0");

                            actions.Add($"set-goal {goalNumber} {goalValue}");
                            if (persistant)
                            {
                                actions.Add("disable-self");
                            }

                            var goalSetRule = new Defrule(conditions, actions);
                            context.Script.Insert(i + goalValue, context.ApplyStacks(goalSetRule));
                        }
                        break;
                    }
                }
            }
            else
            {
                var goalNumber = (int)context.DataStack.Pop();
                var generateRuleId = (string)context.DataStack.Pop();
                var numberOfBlocks = (int)context.DataStack.Pop();

                context.ConditionStack.Pop();
                context.ConditionStack.Push(new Condition($"goal {goalNumber} {numberOfBlocks}"));

                context.DataStack.Push(numberOfBlocks + 1);
                context.DataStack.Push(generateRuleId);
                context.DataStack.Push(goalNumber);
            }
        }
    }
}
