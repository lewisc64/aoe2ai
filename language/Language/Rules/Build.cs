using Language.Extensions;
using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules
{
    [ActiveRule(-1)]
    public class Build : RuleBase
    {
        private const int DefaultNumberBuildings = 100; // prevent build queue getting flooded

        public override string Name => "build";

        public override string Help => $"Sets up the rule to build the building. If amount is unspecified, the building will be built to a maximum of {DefaultNumberBuildings}.";

        public override string Usage => "build ?forward AMOUNT BUILDING_NAME with RESOURCE_NAME escrow";

        public override string Example => @"build 1 barracks
build forward castle
build archery-range with wood escrow";

        public Build()
            : base(@"^build (?<forward>forward )?(?:(?<amount>[^ ]+) )?(?<building>[^ ]+)(?: near (?<near>[^ ]+))?(?: with (?<escrowlist>(?:[^ ]+(?: and )?)*) escrow)?$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var forward = data["forward"].Success;
            var amount = data["amount"].Value.ReplaceIfNullOrEmpty(DefaultNumberBuildings.ToString());
            var building = data["building"].Value;
            var near = data["near"].Value;
            var escrowList = data["escrowlist"].Value;

            var conditions = new List<string>();
            var actions = new List<string>();

            if (!string.IsNullOrEmpty(escrowList))
            {
                conditions.Add($"can-build-with-escrow {building}");
                foreach (var resource in escrowList.Split(" and "))
                {
                    actions.Add($"release-escrow {resource}");
                }
            }
            else
            {
                conditions.Add($"can-build {building}");
            }

            var compressable = true;

            if (!string.IsNullOrEmpty(amount))
            {
                conditions.Add($"building-type-count-total {building} < {amount}");
            }

            if (string.IsNullOrEmpty(near))
            {
                actions.Add($"build{(forward ? "-forward" : "")} {building}");
            }
            else
            {
                actions.Add($"up-set-placement-data {(forward ? "any-enemy" : "my-player-number")} {near} c: 0");
                actions.Add($"up-build place-{(forward ? "forward" : "control")} 0 c: {building}");
                compressable = false;
            }

            context.AddToScript(context.ApplyStacks(new Defrule(conditions, actions) { Compressable = compressable }));
        }
    }
}
