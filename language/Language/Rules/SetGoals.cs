using Language.ScriptItems;
using System.Collections.Generic;
using System.Linq;

namespace Language.Rules
{
    [ActiveRule]
    public class SetGoals : RuleBase
    {
        public override string Name => "set goals";

        public override string Help => "Sets multiple goals, constants, and ensures they are consecutive.";

        public override string Usage => "goals GOAL1, GOAL2, GOAL3 = 0";

        public override IEnumerable<string> Examples => new[]
        {
            "goals one, two, three = 0",
        };

        public SetGoals()
            : base(@"^goals (?<list>(?:[^\s,]+(?:,\s*))+[^\s,]+)\s*=\s*(?<value>[^ ]+)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var list = data["list"].Value.Split(new[] { ',', ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            var value = data["value"].Value;

            var lastGoalNumber = -1;
            var rule = new Defrule();

            foreach (var goalName in list)
            {
                if (!context.Goals.Values.Contains(goalName))
                {
                    var number = context.CreateGoal(goalName);
                    if (lastGoalNumber != -1 && lastGoalNumber != number - 1)
                    {
                        throw new System.InvalidOperationException("Goals were not created consecutively.");
                    }
                    lastGoalNumber = number;
                }
                rule.Actions.Add(new Action($"set-goal {goalName} {value}"));
            }

            context.AddToScript(context.ApplyStacks(rule));
        }
    }
}
