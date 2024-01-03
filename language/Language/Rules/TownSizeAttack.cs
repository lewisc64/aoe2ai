using Language.Extensions;
using Language.ScriptItems;
using System.Collections.Generic;
using System.Linq;

namespace Language.Rules
{
    [ActiveRule]
    public class TownSizeAttack : RuleBase
    {
        private static readonly IEnumerable<string> BuildingsOfInterest = new[]
        {
            "town-center",
            "castle"
        };

        public override string Name => "town size attack";

        public override string Help => "Performs a town size attack (TSA) by inflating the town size until enemy buildings are within it.";

        public override string Usage => "town size attack";

        public override IEnumerable<string> Examples => new[]
        {
            "town size attack",
        };

        public TownSizeAttack()
            : base(@"^town size attack$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var rules = new List<Defrule>();

            rules.Add(new Defrule(
                new[]
                {
                    Condition.JoinConditions("or", BuildingsOfInterest.Select(x => new Condition($"up-building-type-in-town c: {x} >= 1"))),
                },
                new[]
                {
                    new Action("up-modify-sn sn-maximum-town-size c:- 1"),
                }));

            rules.Add(new Defrule(
                new[]
                {
                    rules.Last().Conditions.Single().Invert(),
                },
                new[]
                {
                    new Action("up-modify-sn sn-maximum-town-size c:+ 5"),
                }));

            rules.Add(new Defrule(
                new[]
                {
                    "true",
                },
                new[]
                {
                    "up-modify-sn sn-maximum-town-size c:max 50",
                    "up-modify-sn sn-maximum-town-size c:min 680",
                }));

            context.AddToScript(context.ApplyStacks(rules));
        }
    }
}
