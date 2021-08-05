using Language.ScriptItems;

namespace Language.Rules
{
    [ActiveRule]
    public class ChatTo : RuleBase
    {
        public override string Name => "chat to";

        public ChatTo()
            : base(@"^chat to (?:(?<playerwildcard>all|self|allies)|(?<player>[^ ]+)) (?<message>.+)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);

            if (data["playerwildcard"].Success)
            {
                context.AddToScript(context.ApplyStacks(new Defrule(new[] { "true" }, new[] { $"chat-to-{data["playerwildcard"].Value} {data["message"].Value}" })));
            }
            else
            {
                context.AddToScript(context.ApplyStacks(new Defrule(new[] { "true" }, new[] { $"chat-to-player {data["player"].Value} {data["message"].Value}" })));
            }
        }
    }
}
