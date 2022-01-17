using Language.ScriptItems;
using System.Linq;

namespace Language.Rules
{
    [ActiveRule]
    public class NoMerge : RuleBase
    {
        public override string Name => "no merge";

        public override string Help => "Prevents the rules within from being merged together in compilation.";

        public override string Usage => @"#nomerge
   RULES
#end nomerge";

        public NoMerge()
            : base(@"^(?:#nomerge|#end nomerge)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            if (line.StartsWith("#end"))
            {
                var trackerRuleId = (string)context.DataStack.Pop();
                var trackerRulePosition = context.Script.Items.IndexOf(context.Script.First(x => (x as Defrule)?.Id == trackerRuleId));

                for (var i = trackerRulePosition + 1; i < context.Script.Items.Count; i++)
                {
                    var item = context.Script.Items[i];
                    if (item is Defrule)
                    {
                        ((Defrule)item).Compressable = false;
                    }
                }

                context.Script.Items.RemoveAt(trackerRulePosition);
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
