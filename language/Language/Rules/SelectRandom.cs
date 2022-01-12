using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules
{
    [ActiveRule]
    public class SelectRandom : RuleBase
    {
        public override string Name => "select random";

        public override string Help => "A random block separated by randors will be allowed to execute. Using persistant mode means the randomly chosen one is picked every time, otherwise it will change.";

        public override string Usage => @"#select random ?persistant
   RULES
#randor
   RULES
#end select";

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

                generateRule.Actions.Add(new Action($"set-goal {goalNumber} 0"));
                if (persistant)
                {
                    generateRule.Actions.Add(new Action("disable-self"));
                }

                context.AddToScript(context.ApplyStacks(generateRule));

                context.ConditionStack.Push(new Condition($"goal {goalNumber} 1"));
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
                        rule.Actions.Add(new Action($"up-get-fact random-number 0 {goalNumber}"));
                        if (persistant)
                        {
                            rule.Actions.Add(new Action("disable-self"));
                        }
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
