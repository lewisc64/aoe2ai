using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules
{
    [ActiveRule(-1)]
    public class Build : RuleBase
    {
        private const int MaxConcurrentBuilds = 5;

        public override string Name => "build";

        public override string Help => $"Sets up the rule to build the building. Can only build {MaxConcurrentBuilds} buildings at a time to prevent accidental build queue flooding.";

        public override string Usage => "build ?forward AMOUNT BUILDING_NAME with RESOURCE_NAME escrow";

        public override IEnumerable<string> Examples => new[]
        {
            "build 1 barracks",
            "build forward castle",
            "build archery-range with escrow",
        };

        public Build()
            : base(@"^build (?<forward>forward )?(?:(?<amount>[^ ]+) )?(?<building>[^ ]+)(?: near (?<near>[^ ]+))?(?<withescrow> with escrow)?$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var forward = data["forward"].Success;
            var amount = data["amount"].Value;
            var building = data["building"].Value;
            var near = data["near"].Value;
            var withEscrow = data["withescrow"].Success;

            var conditions = new List<string>();
            var actions = new List<string>();

            if (withEscrow)
            {
                conditions.Add($"can-build-with-escrow {building}");
            }
            else
            {
                conditions.Add($"can-build {building}");
            }

            var compressable = true;

            conditions.Add($"up-pending-objects c: {building} < {MaxConcurrentBuilds}");
            if (!string.IsNullOrEmpty(amount))
            {
                conditions.Add($"building-type-count-total {building} < {amount}");
            }

            if (string.IsNullOrEmpty(near))
            {
                if (withEscrow)
                {
                    context.UsingVolatileGoal(escrowGoal =>
                    {
                        actions.Add($"set-goal {escrowGoal} with-escrow");
                        actions.Add($"up-build {(forward ? "place-forward" : "place-normal")} {escrowGoal} c: {building}"); 
                    });
                }
                else
                {
                    actions.Add($"build{(forward ? "-forward" : "")} {building}");   
                }
            }
            else
            {
                if (forward)
                {
                    conditions.Add($"players-building-type-count target-player {near} >= 1");
                }
                else
                {
                    conditions.Add($"building-type-count {near} >= 1");
                }
                
                actions.Add($"up-set-placement-data {(forward ? "target-player" : "my-player-number")} {near} c: 0");
                if (withEscrow)
                {
                    context.UsingVolatileGoal(escrowGoal =>
                    {
                        actions.Add($"set-goal {escrowGoal} with-escrow");
                        actions.Add($"up-build {(forward ? "place-forward" : "place-control")} {escrowGoal} c: {building}"); 
                    });
                }
                else
                {
                    actions.Add($"up-build {(forward ? "place-forward" : "place-control")} 0 c: {building}");
                }
                
                compressable = false;
            }

            context.AddToScript(context.ApplyStacks(new Defrule(conditions, actions) { Compressable = compressable }));
        }
    }
}
