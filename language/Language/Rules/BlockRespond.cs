using Language.Extensions;
using Language.ScriptItems;

namespace Language.Rules
{
    [ActiveRule]
    public class BlockRespond : RuleBase
    {
        public override string Name => "block respond";

        public override string Help => "When the AI sees the specified amount, the body is allowed to trigger. If building/unit is unspecified, unit is assumed. If amount is unspecified, 1 is assumed.";

        public override string Usage => @"#respond to ?AMOUNT NAME ?BUILDING/UNIT
   RULES
#end respond";

        public override string Example => @"#respond to 3 scout-cavalry-line
    train 4 spearman-line
#end respond";

        public BlockRespond()
            : base(@"^(?:#respond to (?:(?<amount>[^ ]+) )??(?<name>[^ ]+)(?: (?<type>building|unit))?(?: from(?: player)? (?<player>[^ ]+))?|#end respond)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            if (line.StartsWith("#respond"))
            {
                var data = GetData(line);
                var amount = data["amount"].Value.ReplaceIfNullOrEmpty("1");
                var name = data["name"].Value;
                var type = data["type"].Value.ReplaceIfNullOrEmpty("unit");
                var player = data["player"].Value.ReplaceIfNullOrEmpty("any-enemy");

                context.ConditionStack.Push(new Condition($"players-{type}-type-count {player} {name} >= {amount}"));
            }
            else
            {
                context.ConditionStack.Pop();
            }
        }
    }
}
