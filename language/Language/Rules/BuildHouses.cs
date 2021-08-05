using Language.Extensions;
using Language.ScriptItems;

namespace Language.Rules
{
    [ActiveRule]
    public class BuildHouses : RuleBase
    {
        private const int DefaultHeadroom = 5;

        private const int YurtBuildingId = 712;

        public override string Name => "chat to";

        public BuildHouses()
            : base(@"^build (?<style>houses|yurts)(?: with (?<headroom>[^ ]+) headroom)?$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var style = data["style"].Value;
            var headroom = data["headroom"].Value.ReplaceIfNullOrEmpty(DefaultHeadroom.ToString());

            var building = style == "houses" ? "house" : YurtBuildingId.ToString();

            var conditions = new[]
            {
                "population-headroom != 0",
                $"up-pending-objects c: {building} == 0",
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
