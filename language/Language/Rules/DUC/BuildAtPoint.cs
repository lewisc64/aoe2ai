using Language.ScriptItems;

namespace Language.Rules.DUC
{
    [ActiveRule]
    public class BuildAtPoint : RuleBase
    {
        public override string Name => "DUC build at point";

        public override string Help => "TODO";

        public override string Usage => "TODO";

        public BuildAtPoint()
            : base(@"^\$build (?<building>[^ ]+) at point (?<point>[^ ]+)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var building = data["building"].Value;
            var point = data["point"].Value;

            var goalPointX = context.GetDucPointGoalNumber(point);

            context.UsingVolatilePointGoal(tempGoalX =>
            {
                var rule = new Defrule(
                    new[]
                    {
                        $"can-build {building}",
                    },
                    new[]
                    {
                        $"up-modify-goal {tempGoalX} g:= {goalPointX}",
                        $"up-modify-goal {tempGoalX + 1} g:= {goalPointX + 1}",
                        $"up-modify-goal {tempGoalX} c:/ 100",
                        $"up-modify-goal {tempGoalX + 1} c:/ 100",
                        $"up-build-line {tempGoalX} {tempGoalX} c: {building}",
                    });

                context.AddToScript(context.ApplyStacks(rule));
            });
        }
    }
}
