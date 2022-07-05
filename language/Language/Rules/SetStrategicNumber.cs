using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules
{
    [ActiveRule]
    public class SetStrategicNumber : RuleBase
    {
        public override string Name => "set strategic number";

        public override string Help => "Sets a strategic number. Uses the 'sn-' prefix for recognition of the rule.";

        public override string Usage => "STRATEGIC_NUMBER_NAME = VALUE";

        public override IEnumerable<string> Examples => new[]
        {
            "sn-gold-gatherer-percentage = 50",
            "sn-gold-gatherer-percentage -= 5",
        };

        public SetStrategicNumber()
            : base(@"^(?<name>sn-[^ ]+) ?(?<mathop>\+|\-|\*|\/)?= ?(?<value>.+)$")
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
                rule.Actions.Add(new Action($"set-strategic-number {name} {value}"));
            }
            else
            {
                rule.Actions.Add(new Action($"up-modify-sn {name} c:{mathOp} {value}"));
            }

            context.AddToScript(context.ApplyStacks(rule));
        }
    }
}
