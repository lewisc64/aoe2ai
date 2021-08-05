using Language.ScriptItems;

namespace Language.Rules
{
    [ActiveRule]
    public class ModifyTownSize : RuleBase
    {
        public override string Name => "modify town size";

        public ModifyTownSize()
            : base(@"^(?<operation>set|increase|decrease) town size (?:to|by) (?<amount>.+)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var operation = data["operation"].Value;
            var amount = data["amount"].Value;

            var rule = new Defrule();

            if (operation == "set")
            {
                rule.Actions.Add(new Action($"set-strategic-number sn-maximum-town-size {amount}"));
            }
            else
            {
                rule.Actions.Add(new Action($"up-modify-sn sn-maximum-town-size c:{(operation == "increase" ? "+" : "-")} {amount}"));
            }

            context.AddToScript(context.ApplyStacks(rule));
        }
    }
}
