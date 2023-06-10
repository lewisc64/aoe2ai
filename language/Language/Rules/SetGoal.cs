using Language.ScriptItems;
using System.Collections.Generic;
using System.Linq;

namespace Language.Rules
{
    [ActiveRule]
    public class SetGoal : RuleBase
    {
        public override string Name => "set goal";

        public override string Help => "Sets a goal, and sets up the constant if it does not already exist.";

        public override string Usage => "goal GOAL_NAME = VALUE";

        public override IEnumerable<string> Examples => new[]
        {
            "goal test = 1",
            "goal test += 1",
            "goal test *= 5",
        };

        public SetGoal()
            : base(@"^goal (?<name>[^ ]+) ?(?<mathop>\+|\-|\*|\/)?= ?(?<value>.+)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var name = data["name"].Value;
            var mathOp = data["mathop"].Value;
            var value = data["value"].Value;

            var rule = new Defrule();

            if (string.IsNullOrEmpty(mathOp))
            {
                if (!context.Goals.Values.Contains(name))
                {
                    context.CreateGoal(name);
                }
                rule.Actions.Add(new Action($"set-goal {name} {value}"));
            }
            else
            {
                rule.Actions.Add(new Action($"up-modify-goal {name} c:{mathOp} {value}"));
            }

            context.AddToScript(context.ApplyStacks(rule));
        }
    }
}
