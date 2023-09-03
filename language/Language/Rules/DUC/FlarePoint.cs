using Language.ScriptItems;

namespace Language.Rules.DUC
{
    [ActiveRule]
    public class FlarePoint : RuleBase
    {
        public override string Name => "DUC flare point";

        public override string Help => "TODO";

        public override string Usage => "TODO";

        public FlarePoint()
            : base(@"^\$flare point (?<point>[^ ]+)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var point = data["point"].Value;

            var goalPointX = context.GetDucPointGoalNumber(point);

            context.UsingVolatilePointGoal(tempGoalX =>
            {
                var rule = new Defrule(
                    new[]
                    {
                        "true",
                    },
                    new[]
                    {
                        $"up-modify-goal {tempGoalX} g:= {goalPointX}",
                        $"up-modify-goal {tempGoalX + 1} g:= {goalPointX + 1}",
                        $"up-modify-goal {tempGoalX} c:/ 100",
                        $"up-modify-goal {tempGoalX + 1} c:/ 100",
                        $"up-send-flare {tempGoalX}",
                    });

                context.AddToScript(context.ApplyStacks(rule));
            });
        }
    }
}
