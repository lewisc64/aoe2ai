using Language.ScriptItems;
using System.Linq;

namespace Language.Rules
{
    [ActiveRule]
    public class NoMerge : RuleBase
    {
        public override string Name => "no merge";

        public NoMerge()
            : base(@"^(?:#nomerge|#end nomerge)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            if (line.StartsWith("#end"))
            {
                var trackerRuleId = (string)context.DataStack.Pop();
                var trackerRulePosition = context.Script.IndexOf(context.Script.First(x => (x as Defrule)?.Id == trackerRuleId));

                for (var i = trackerRulePosition + 1; i < context.Script.Count; i++)
                {
                    var item = context.Script[i];
                    if (item is Defrule)
                    {
                        ((Defrule)item).Compressable = false;
                    }
                }

                context.Script.RemoveAt(trackerRulePosition);
            }
            else
            {
                var trackerRule = new Defrule();
                context.DataStack.Push(trackerRule.Id);
                context.AddToScript(trackerRule);
            }
        }
    }
}
