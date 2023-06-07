using Language.ScriptItems;

namespace Language.Rules.DUC
{
    [ActiveRule]
    public class LerpPoint : RuleBase
    {
        public override string Name => "DUC lerp point";

        public override string Help => "TODO";

        public override string Usage => "TODO";

        public LerpPoint()
            : base(@"^\$lerp point (?<point>[^ ]+) (?<tiles>[0-9]+(?:\.[0-9]{1,2})?) tiles? (?<direction>towards|away from) (?<otherpoint>[^ ]+)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var pointName = data["point"].Value;
            var tiles = float.Parse(data["tiles"].Value) * (data["direction"].Value == "towards" ? 1 : -1);
            var otherPointName = data["otherpoint"].Value;

            var rule = new Defrule(
                new[]
                {
                    "true",
                },
                new[]
                {
                    $"up-lerp-tiles {context.GetDucPointGoalNumber(pointName)} {context.GetDucPointGoalNumber(otherPointName)} c: {tiles * 100}",
                });

            context.AddToScript(context.ApplyStacks(rule));
        }
    }
}
