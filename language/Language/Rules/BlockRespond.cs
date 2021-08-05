using Language.Extensions;
using Language.ScriptItems;

namespace Language.Rules
{
    [ActiveRule]
    public class BlockRespond : RuleBase
    {
        public override string Name => "block respond";

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
