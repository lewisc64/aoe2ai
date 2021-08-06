using Language.Extensions;
using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules
{
    [ActiveRule]
    public class Respond : RuleBase
    {
        public override string Name => "respond";

        public override string Help => "When the AI sees the specified amount, it reacts with the specified parameters. If building/unit is unspecified, unit is assumed. If amount is unspecified, 1 is assumed.";

        public override string Usage => "respond to ?AMOUNT NAME ?BUILDING/UNIT with NAME ?BUILDING/UNIT";

        public Respond()
            : base(@"^respond to (?:(?<enemyamount>[^ ]+) )??(?<enemyname>[^ ]+)(?: (?<enemytype>building|unit))?(?: from(?: player)? (?<player>[^ ]+))? with (?:(?<createamount>[^ ]+) )?(?<createname>[^ ]+)(?: (?<createtype>building|unit))?$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var enemyAmount = data["enemyamount"].Value.ReplaceIfNullOrEmpty("1");
            var enemyName = data["enemyname"].Value;
            var enemyType = data["enemytype"].Value.ReplaceIfNullOrEmpty("unit");
            var player = data["player"].Value.ReplaceIfNullOrEmpty("any-enemy");
            var createAmount = data["createamount"].Value;
            var createName = data["createname"].Value;
            var createType = data["createtype"].Value.ReplaceIfNullOrEmpty("unit");

            var action = createType == "unit" ? "train" : "build";

            var conditions = new List<string>();
            var actions = new List<string>();

            conditions.Add($"players-{enemyType}-type-count {player} {enemyName} >= {enemyAmount}");

            if (!string.IsNullOrEmpty(createAmount))
            {
                conditions.Add($"{createType}-type-count-total {createName} < {createAmount}");
            }

            conditions.Add($"can-{action} {createName}");

            actions.Add($"{action} {createName}");

            context.AddToScript(context.ApplyStacks(new Defrule(conditions, actions)));
        }
    }
}
