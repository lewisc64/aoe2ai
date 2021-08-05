using Language.ScriptItems;

namespace Language.Rules
{
    [ActiveRule]
    public class BuildFarms : RuleBase
    {
        public override string Name => "build farms";

        public BuildFarms()
            : base(@"^build farms$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var gathererGoal = context.CreateGoal();
            var farmGoal = context.CreateGoal();

            var infoRule = new Defrule(
                new[]
                {
                    "true",
                },
                new[]
                {
                    $"up-get-fact unit-type-count villager {gathererGoal}",
                    $"up-modify-goal {gathererGoal} s:* sn-food-gatherer-percentage",
                    $"up-modify-goal {gathererGoal} c:/ 100",
                    $"up-get-fact building-type-count-total farm {farmGoal}",
                });

            var buildRule = new Defrule(
                new[]
                {
                    $"up-compare-goal {farmGoal} g:< {gathererGoal}",
                    "can-build farm",
                },
                new[]
                {
                    "build farm"
                });

            context.AddToScript(context.ApplyStacks(infoRule));
            context.AddToScript(context.ApplyStacks(buildRule));
        }
    }
}
