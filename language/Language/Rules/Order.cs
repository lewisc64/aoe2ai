using Language.ScriptItems;
using System.Collections.Generic;
using System.Linq;

namespace Language.Rules
{
    [ActiveRule(10)]
    public class Order : RuleBase
    {
        public override string Name => "order";

        public override string Help => "Executes statements in order once every rule pass. Loops back to the beginning upon reaching the end.";

        public override string Example => "train archer-line => train skirmisher-line";

        public Order()
            : base(@"^.+?( *=> *.+)+$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var segments = line.Split("=>");
            var transpiler = new Transpiler();
            var goalNumber = context.CreateGoal();

            var items = new List<IScriptItem>();

            for (var i = 0; i < segments.Length; i++)
            {
                var segment = segments[i];

                Script segmentItems = null;

                context.UsingSubcontext(subcontext =>
                {
                    subcontext.CurrentFileName = $"{subcontext.CurrentFileName} -> order expression component '{segment}'";
                    segmentItems = transpiler.Transpile(segment, subcontext, suppressStackWarnings: true);
                });

                if (segmentItems.Count(x => x is Defrule) >= 2)
                {
                    throw new System.InvalidOperationException($"'{segment}' transpiles to more than one defrule, which order does not support.");
                }
                else if (segmentItems.Count(x => x is Defrule) == 0)
                {
                    throw new System.InvalidOperationException($"'{segment}' does not transpile to any defrules.");
                }

                foreach (var segmentItem in segmentItems)
                {
                    (segmentItem as Defrule)?.Conditions.Add(new Condition($"goal {goalNumber} {i}"));
                    (segmentItem as Defrule)?.Actions.Add(new Action($"set-goal {goalNumber} {(i + 1) % segments.Length}"));
                    items.Add(segmentItem);
                }
            }

            items.Reverse();
            items.Insert(0, new Defrule(new[] { "true" }, new[] { $"set-goal {goalNumber} 0", "disable-self" }));

            context.AddToScript(context.ApplyStacks(items));
        }
    }
}
