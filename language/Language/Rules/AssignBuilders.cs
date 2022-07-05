using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules
{
    [ActiveRule]
    public class AssignBuilders : RuleBase
    {
        public override string Name => "assign builders";

        public override string Help => "Sets the amount of builders that should build a building.";

        public override string Usage => "assign AMOUNT builders to BUILDING_NAME";

        public override IEnumerable<string> Examples => new[]
        {
            "assign 8 builders to castle"
        };

        public AssignBuilders()
            : base(@"^assign (?<amount>[^ ]+) builders? to (?<building>[^ ]+)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var amount = data["amount"].Value;
            var building = data["building"].Value;

            var rule = new Defrule(new[] { "true" }, new[] { $"up-assign-builders c: {building} c: {amount}" });
            context.AddToScript(context.ApplyStacks(rule));
        }
    }
}
