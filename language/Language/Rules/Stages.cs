using Language.ScriptItems;

namespace Language.Rules
{
    [ActiveRule]
    public class Stages : RuleBase
    {
        public override string Name => "stages";

        public override string Help => "Blocks out rules having them advance into each other switching by a goal.";

        public override string Usage => @"#stages
    RULES
#advance when CONDITION
    RULES
#end stages";

        public Stages()
            : base(@"^(?:#stages|#advance when (?<condition>.+)|#end stages)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            if (line.StartsWith("#stages"))
            {
                var stagesGoal = context.CreateGoal();

                var currentStage = 0;

                var rule = new Defrule(new[] { "true" }, new[] { $"set-goal {stagesGoal} 0", "disable-self" });
                context.AddToScript(context.ApplyStacks(rule));

                context.ConditionStack.Push(new Condition($"goal {stagesGoal} {currentStage}"));
                context.DataStack.Push(stagesGoal);
                context.DataStack.Push(currentStage);
            }
            else if (line.StartsWith("#advance"))
            {
                var condition = GetData(line)["condition"].Value;

                var previousStage = (int)context.DataStack.Pop();
                var stagesGoal = context.DataStack.Pop();

                context.ConditionStack.Pop();

                var rule = new Defrule(new[] { $"goal {stagesGoal} {previousStage}", condition }, new[] { $"set-goal {stagesGoal} {previousStage + 1}" });
                context.AddToScript(context.ApplyStacks(rule));

                context.ConditionStack.Push(new Condition($"goal {stagesGoal} {previousStage + 1}"));

                context.DataStack.Push(stagesGoal);
                context.DataStack.Push(previousStage + 1);
            }
            else
            {
                context.DataStack.Pop();
                context.DataStack.Pop();
                context.ConditionStack.Pop();
            }
        }
    }
}
