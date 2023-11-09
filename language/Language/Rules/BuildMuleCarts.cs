using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules
{
    [ActiveRule]
    public class BuildMuleCarts : RuleBase
    {
        private const int VillagerInterval = 10;

        public override string Name => "build mule carts";

        public override string Help => $"Builds mule carts. Creates 1 mule cart for every {VillagerInterval} villagers.";

        public override string Usage => "build mule carts";

        public override IEnumerable<string> Examples => new[]
        {
            "build mule carts",
        };

        public BuildMuleCarts()
            : base(@"^build mule carts$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var rules = new List<Defrule>();

            var villsDividedGoal = context.CreateVolatileGoal();
            var muleCartCountGoal = context.CreateVolatileGoal();

            rules.Add(new Defrule(
                new[]
                {
                    "true",
                },
                new[]
                {
                    $"up-get-fact unit-type-count villager {villsDividedGoal}",
                    $"up-modify-goal {villsDividedGoal} c:/ {VillagerInterval}",
                    $"up-modify-goal {villsDividedGoal} c:+ 1",
                    $"up-get-fact unit-type-count-total mule-cart {muleCartCountGoal}",
                }));

            rules.Add(new Defrule(
                new[]
                {
                    $"up-compare-goal {muleCartCountGoal} g:< {villsDividedGoal}",
                    "can-build mule-cart",
                    "up-pending-objects c: mule-cart == 0",
                },
                new[]
                {
                    "build mule-cart"
                }));

            context.FreeVolatileGoals(new[] { villsDividedGoal, muleCartCountGoal });

            context.AddToScript(context.ApplyStacks(rules));
        }
    }
}
