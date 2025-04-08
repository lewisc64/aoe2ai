using Language.ScriptItems;
using System.Collections.Generic;
using System.Linq;

namespace Language.Rules
{
    [ActiveRule(-1)]
    public class Train : RuleBase
    {
        public override string Name => "train";

        public override string Help => "Trains a unit using the specified parameters.";

        public override string Usage => @"train UNIT_NAME
train UNIT_NAME with RESOURCE_NAME escrow
train AMOUNT UNIT_NAME
train AMOUNT UNIT_NAME with RESOURCE_NAME escrow";

        public override IEnumerable<string> Examples => new[]
        {
            "train archer-line",
            "train 10 archer-line",
            "train 10 archer-line with escrow",
        };

        public Train()
            : base(@"^train (?:(?<amount>[^ ]+) )?(?<unit>[^ ]+)(?<withescrow> with escrow)?$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var amount = data["amount"].Value;
            var unit = data["unit"].Value;
            var withEscrow = data["withescrow"].Success;

            var conditions = new List<string>();
            var actions = new List<string>();

            if (withEscrow)
            {
                conditions.Add($"can-train-with-escrow {unit}");
                context.UsingVolatileGoal(escrowGoal =>
                {
                    actions.Add($"set-goal {escrowGoal} with-escrow");
                    actions.Add($"up-train {escrowGoal} c: {unit}"); 
                });
            }
            else
            {
                conditions.Add($"can-train {unit}");
                actions.Add($"train {unit}");
            }

            if (!string.IsNullOrEmpty(amount))
            {
                conditions.Add($"unit-type-count-total {(Game.UnitSets.ContainsKey(unit) ? Game.UnitSets[unit] : unit)} < {amount}");
            }

            context.AddToScript(context.ApplyStacks(new Defrule(conditions, actions)));
        }
    }
}
