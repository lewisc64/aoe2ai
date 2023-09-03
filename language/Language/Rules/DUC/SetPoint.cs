using Language.ScriptItems;

namespace Language.Rules.DUC
{
    [ActiveRule]
    public class SetPoint : RuleBase
    {
        public override string Name => "DUC set point";

        public override string Help => "TODO";

        public override string Usage => "TODO";

        public SetPoint()
            : base(@"^\$point (?<point>[^ ]+) is first (?<list>local|remote)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var pointName = data["point"].Value;
            var list = data["list"].Value;

            var goalPointX = context.CreateDucPointGoal($"{pointName}");

            var rule = new Defrule(
                new[]
                {
                    "true",
                },
                new[]
                {
                    $"up-set-target-object search-{list} c: 0",
                    $"up-get-object-data {Game.ObjectDataPreciseX} {goalPointX}",
                    $"up-get-object-data {Game.ObjectDataPreciseY} {goalPointX + 1}",
                });

            context.AddToScript(context.ApplyStacks(rule));
        }
    }
}
