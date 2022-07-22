using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules
{
    [ActiveRule]
    public class LureBoars : RuleBase
    {
        public override string Name => "lure boars";

        public override string Help => "Lures boars using one villager to the town center.";

        public override string Usage => "lure boars";

        public override IEnumerable<string> Examples => new[]
        {
            "lure boars",
        };

        public LureBoars()
            : base(@"^lure boars$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var rules = new[]
            {
                new Defrule(
                    new[]
                    {
                        "dropsite-min-distance live-boar != -1"
                    },
                    new[]
                    {
                        "set-strategic-number sn-enable-boar-hunting 2",
                        "set-strategic-number sn-minimum-number-hunters 1",
                        "set-strategic-number sn-minimum-boar-lure-group-size 1",
                        "set-strategic-number sn-minimum-boar-hunt-group-size 1",
                        "set-strategic-number sn-maximum-hunt-drop-distance 48",
                    }),
                new Defrule(
                    new[]
                    {
                        "dropsite-min-distance live-boar == -1"
                    },
                    new[]
                    {
                        "set-strategic-number sn-enable-boar-hunting 1",
                        "set-strategic-number sn-minimum-number-hunters 0",
                        "set-strategic-number sn-minimum-boar-lure-group-size 0",
                        "set-strategic-number sn-minimum-boar-hunt-group-size 0",
                        "set-strategic-number sn-maximum-hunt-drop-distance 8",
                    }),
                new Defrule(
                    new[]
                    {
                        "dropsite-min-distance live-boar < 4",
                        "dropsite-min-distance live-boar >= 0",
                    },
                    new[]
                    {
                        "up-request-hunters c: 8",
                    }),
                new Defrule(
                    new[]
                    {
                        "food-amount < 50",
                        "up-pending-objects c: villager <= 1",
                    },
                    new[]
                    {
                        "up-drop-resources c: boar-food 10",
                    }),
            };

            context.AddToScript(context.ApplyStacks(rules));
        }
    }
}
