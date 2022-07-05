using Language.ScriptItems;
using System.Collections.Generic;
using System.Linq;

namespace Language.Rules
{
    [ActiveRule]
    public class BuildDropoffs : RuleBase
    {
        public override string Name => "chat to";

        public override string Help => "Sets up a rule for automatically refreshing dropoff points.";

        public override string Usage => "build DROPOFF_TYPE";

        public override IEnumerable<string> Examples => new[]
        {
            "build lumber camps",
            "build gold mining camps",
            "build stone mining camps",
            "build lumber camps maintaining 4 tiles",
        };

        public BuildDropoffs()
            : base(@"^build (?<dropofftype>lumber camps|(?:gold|stone) mining camps)(?: maintaining (?<tiles>[^ ]+) tiles?)?$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var dropoffType = data["dropofftype"].Value;
            var tiles = data["tiles"].Value;

            string resource;
            string building;

            if (dropoffType.Contains("mining camps"))
            {
                if (dropoffType.Contains("gold"))
                {
                    resource = "gold";
                }
                else
                {
                    resource = "stone";
                }
                building = "mining-camp";
                if (string.IsNullOrEmpty(tiles))
                {
                    tiles = "3";
                }
            }
            else if (dropoffType == "lumber camps")
            {
                resource = "wood";
                building = "lumber-camp";
                if (string.IsNullOrEmpty(tiles))
                {
                    tiles = "2";
                }
            }
            else
            {
                throw new System.InvalidOperationException("Unknown dropoff type.");
            }

            var conditions = new List<Condition>
            {
                new Condition($"dropsite-min-distance {resource} > {tiles}"),
                new Condition($"resource-found {resource}"),
                new Condition($"up-pending-objects c: {building} == 0"),
                new Condition($"can-build {building}"),
            };

            if (building == "mining-camp" && resource == "gold")
            {
                conditions[0] = new CombinatoryCondition(
                    "or",
                    new[]
                    {
                        conditions.First(),
                        Condition.Parse($"unit-type-count {Game.MaleGoldMinerId} == 0 and unit-type-count {Game.FemaleGoldMinerId} == 0 and strategic-number sn-gold-gatherer-percentage > 0"),
                    });
            }

            if (building == "lumber-camp")
            {
                conditions[0] = Condition.JoinConditions("and", conditions.GetRange(0, 2));
                conditions.RemoveAt(1);
                conditions[0] = new CombinatoryCondition(
                    "or",
                    new[]
                    {
                        conditions.First(),
                        Condition.Parse($"game-time >= 60 and building-type-count-total {building} == 0"),
                    });
            }

            var rule = new Defrule(conditions, new[] { new Action($"build {building}") });
            context.AddToScript(context.ApplyStacks(rule));
        }
    }
}
