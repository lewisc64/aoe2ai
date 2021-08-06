using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules
{
    [ActiveRule]
    public class BuildWalls : RuleBase
    {
        public override string Name => "build walls";

        public override string Help => "Wall placement must be enabled on the same perimeter to function.";

        public override string Usage => "build stone/palisade walls/gates on perimeter PERIMETER_NUMBER";

        public BuildWalls()
            : base(@"^build (?<material>stone|palisade) (?<type>walls|gates) (?:with|on) perimeter (?<perimeter>1|2)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var material = data["material"].Value;
            var type = data["type"].Value;
            var perimeter = data["perimeter"].Value;

            var building = material == "stone" ? "stone-wall-line" : "palisade-wall";

            var conditions = new List<string>();
            var actions = new List<string>();

            if (type == "walls")
            {
                conditions.Add($"can-build-wall {perimeter} {building}");

                actions.Add($"build-wall {perimeter} {building}");
            }
            else
            {
                conditions.Add($"building-type-count-total {building} > 0");
                conditions.Add($"can-build-gate {perimeter}");
                conditions.Add("building-type-count-total gate < 5");

                actions.Add($"build-gate {perimeter}");
            }

            var rule = new Defrule(conditions, actions);
            context.AddToScript(context.ApplyStacks(rule));
        }
    }
}
