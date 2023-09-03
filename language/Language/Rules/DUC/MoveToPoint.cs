using Language.ScriptItems;

namespace Language.Rules.DUC
{
    [ActiveRule]
    public class MoveToPoint : RuleBase
    {
        public override string Name => "DUC move to point";

        public override string Help => "TODO";

        public override string Usage => "TODO";

        public MoveToPoint()
            : base(@"^\$move to point (?<point>[^ ]+)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var pointName = data["point"].Value;

            var rule = new Defrule(
                new[]
                {
                    "true",
                },
                new[]
                {
                    "set-strategic-number sn-target-point-adjustment 6",
                    $"up-target-point {context.GetDucPointGoalNumber(pointName)} action-default -1 -1",
                    "set-strategic-number sn-target-point-adjustment 0",
                });

            context.AddToScript(context.ApplyStacks(rule));
        }
    }
}
