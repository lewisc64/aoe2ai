using Language.ScriptItems;

namespace Language.Rules
{
    [ActiveRule]
    public class EnableWalls : RuleBase
    {
        public override string Name => "enable walls";

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
