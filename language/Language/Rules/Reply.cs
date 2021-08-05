using Language.Extensions;
using Language.ScriptItems;

namespace Language.Rules
{
    [ActiveRule]
    public class Reply : RuleBase
    {
        public override string Name => "reply";

        public Reply()
            : base(@"^(?:#reply to (?<playertype>enemy|ally) taunt (?<tauntnumber>[^ ]+)|#end reply)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            if (line.StartsWith("#reply"))
            {
                var data = GetData(line);
                var playerType = data["playertype"].Value;
                var tauntNumber = data["tauntnumber"].Value;

                context.ConditionStack.Push(new Condition($"taunt-detected any-{playerType} {tauntNumber}"));
                context.DataStack.Push(playerType);
                context.DataStack.Push(tauntNumber);
            }
            else
            {
                var tauntNumber = context.DataStack.Pop();
                var playerType = context.DataStack.Pop();
                context.AddToScript(context.ApplyStacks(new Defrule(new[] { "true" }, new[] { $"acknowledge-taunt this-any-{playerType} {tauntNumber}" })));
                context.ConditionStack.Pop();
            }
        }
    }
}
