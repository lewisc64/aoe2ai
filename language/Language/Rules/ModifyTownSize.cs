using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules
{
    [ActiveRule]
    public class ModifyTownSize : RuleBase
    {
        public override string Name => "modify town size";

        public override string Help => "Modifies the sn-maximum-town-size strategic number.";

        public override string Usage => "set/increase/decrease town size by/to AMOUNT";

        public override IEnumerable<string> Examples => new[]
        {
            "set town size to 30",
            "increase town size by 10",
            "decrease town size by 5",
        };

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
