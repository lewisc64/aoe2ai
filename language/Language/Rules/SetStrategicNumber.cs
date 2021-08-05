using Language.ScriptItems;

namespace Language.Rules
{
    [ActiveRule]
    public class SetStrategicNumber : RuleBase
    {
        public override string Name => "set strategic number";

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
