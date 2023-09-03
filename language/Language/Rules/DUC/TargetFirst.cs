using Language.ScriptItems;

namespace Language.Rules.DUC
{
    [ActiveRule]
    public class TargetFirst : RuleBase
    {
        public override string Name => "DUC target first in list";

        public override string Help => "TODO";

        public override string Usage => "TODO";

        public TargetFirst()
            : base(@"^\$target is first (?<list>local|remote)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var list = data["list"].Value;

            var goalPointX = context.CreateGoal();
            context.CreateGoal();

            var rule = new Defrule(
                new[]
                {
                    "true",
                },
                new[]
                {
                    $"up-set-target-object search-{list} c: 0",
                    $"up-get-point position-object {goalPointX}",
                    $"up-set-target-point {goalPointX}",
                });

            context.AddToScript(context.ApplyStacks(rule));
        }
    }
}
