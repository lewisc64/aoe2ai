using Language.Extensions;
using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules
{
    [ActiveRule]
    public class BuildHouses : RuleBase
    {
        private const int DefaultHeadroom = 5;

        public override string Name => "chat to";

        public override string Help => $"Sets up rule to build houses, default headroom is {DefaultHeadroom}.";

        public override string Usage => @"build houses with AMOUNT headroom";

        public override IEnumerable<string> Examples => new[]
        {
            "build houses",
            "build houses with 15 headroom",
        };

        public BuildHouses()
            : base(@"^build (?<style>houses|yurts)(?: with (?<headroom>[^ ]+) headroom)?$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var style = data["style"].Value;
            var headroom = data["headroom"].Value.ReplaceIfNullOrEmpty(DefaultHeadroom.ToString());

            var building = style == "houses" ? "house" : Game.YurtId.ToString();

            var conditions = new[]
            {
                "population-headroom != 0",
                $"up-pending-objects c: {building} < 2",
                $"can-build {building}",
                $"housing-headroom < {headroom}",
            };

            var actions = new[]
            {
                $"build {building}",
            };

            var rule = new Defrule(conditions, actions);
            context.AddToScript(context.ApplyStacks(rule));
        }
    }
}
