using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules
{
    [ActiveRule]
    public class DistributeVillagersTo : RuleBase
    {
        public override string Name => "distribute villagers to";

        public override string Help => "Modifies the gatherer percentages. Precomputes, so no constants or facts can be used, just numbers.";

        public override string Usage => "distribute PERCENTAGE villagers from RESOURCE_NAME to RESOURCE_NAME";

        public override IEnumerable<string> Examples => new[]
        {
            "distribute 5 villagers from wood to stone",
            "distribute 10 villagers from wood and food to gold",
            "distribute 8 villagers from food to gold and stone using modifiers",
        };

        public DistributeVillagersTo()
            : base(@"^distribute (?<amount>[0-9]+) villagers? from (?<fromlist>(?:[^ ]+(?: and )?)*) to (?<tolist>.+?)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var amount = int.Parse(data["amount"].Value);
            var fromList = data["fromlist"].Value.Split(" and ");
            var toList = data["tolist"].Value.Split(" and ");

            var conditions = new List<string>() { "true" };
            var actions = new List<string>() { "do-nothing" };

            foreach (var resource in fromList)
            {
                conditions.Add($"strategic-number sn-{resource}-gatherer-percentage >= {amount / fromList.Length}");
                actions.Add($"up-modify-sn sn-{resource}-gatherer-percentage c:- {amount / fromList.Length}");
            }

            foreach (var resource in toList)
            {
                conditions.Add($"strategic-number sn-{resource}-gatherer-percentage <= {100 - amount / toList.Length}");
                actions.Add($"up-modify-sn sn-{resource}-gatherer-percentage c:+ {amount / toList.Length}");
            }

            var rule = new Defrule(conditions, actions);
            context.AddToScript(context.ApplyStacks(rule));
        }
    }
}
