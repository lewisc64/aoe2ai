using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules
{
    [ActiveRule]
    public class EnableWalls : RuleBase
    {
        public override string Name => "enable walls";

        public override string Help => "Sets up rule that allows the AI to build walls on the specified perimeter.";

        public override string Usage => "enable walls on perimeter PERIMETER_NUMBER";

        public override IEnumerable<string> Examples => new[]
        {
            "enable walls on perimeter 1",
            "enable walls on perimeter 2",
        };

        public EnableWalls()
            : base(@"^enable walls (?:with|on) perimeter (?<perimeter>1|2)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var perimeter = GetData(line)["perimeter"].Value;

            var rule = new Defrule(
                new[]
                {
                    "true",
                },
                new[]
                {
                    $"enable-wall-placement {perimeter}",
                    "disable-self",
                });

            context.AddToScript(context.ApplyStacks(rule));
        }
    }
}
