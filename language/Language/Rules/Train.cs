using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules
{
    [ActiveRule]
    public class Train : RuleBase
    {
        public override string Name => "train";

        public Train()
            : base(@"^train (?:(?<amount>[^ ]+) )?(?<unit>[^ ]+)(?: with (?<escrowlist>(?:[^ ]+(?: and )?)*) escrow)?$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var amount = data["amount"].Value;
            var unit = data["unit"].Value;
            var escrowList = data["escrowlist"].Value;

            var conditions = new List<string>();
            var actions = new List<string>();

            if (!string.IsNullOrEmpty(escrowList))
            {
                conditions.Add($"can-train-with-escrow {unit}");
                foreach (var resource in escrowList.Split(" and "))
                {
                    actions.Add($"release-escrow {resource}");
                }
            }
            else
            {
                conditions.Add($"can-train {unit}");
            }

            if (!string.IsNullOrEmpty(amount))
            {
                conditions.Add($"unit-type-count-total {unit} < {amount}");
            }

            actions.Add($"train {unit}");

            context.AddToScript(context.ApplyStacks(new Defrule(conditions, actions)));
        }
    }
}
