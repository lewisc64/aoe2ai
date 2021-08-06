using Language.ScriptItems;

namespace Language.Rules
{
    [ActiveRule]
    public class SetStrategicNumber : RuleBase
    {
        public override string Name => "set strategic number";

        public override string Help => "Sets a strategic number. Uses the 'sn-' prefix for recognition of the rule.";

        public override string Usage => "STRATEGIC_NUMBER_NAME = VALUE";

        public override string Example => @"sn-maximum-gold-drop-distance = 8
sn-maximum-town-size += 5";

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
