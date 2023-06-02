using Language.ScriptItems;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Language.Rules
{
    [ActiveRule]
    public class ChatTo : RuleBase
    {
        public override string Name => "chat to";

        public override string Usage => "chat to PLAYER_TYPE \"MESSAGE\"";

        public override IEnumerable<string> Examples => new[]
        {
            "chat to all \"hello everyone!\"",
            "chat to self \"hello me!\"",
            "chat to allies \"hello friends!\"",
            "chat to 5 \"hello player 5!\"",
            "chat to target-player \"hello target player!\"",
        };

        public ChatTo()
            : base(@"^chat to (?:(?<playerwildcard>all|self|allies)|(?<player>[^ ]+)) ""(?<message>.+)""$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var message = data["message"].Value;

            string constName = GetUniqueKey(message);
            if (!context.Constants.Contains(constName))
            {
                context.AddToScript(context.CreateConstant(constName, message));
            }

            if (data["playerwildcard"].Success)
            {
                context.AddToScript(context.ApplyStacks(new Defrule(new[] { "true" }, new[] { $"chat-to-{data["playerwildcard"].Value} {constName}".Replace("to-self", "local-to-self") })));
            }
            else
            {
                context.AddToScript(context.ApplyStacks(new Defrule(new[] { "true" }, new[] { $"chat-to-player {data["player"].Value} {constName}" })));
            }
        }

        private string GetUniqueKey(string message)
        {
            using var hasher = SHA1.Create();
            return $"chat-{BitConverter.ToString(hasher.ComputeHash(Encoding.UTF8.GetBytes(message))).Replace("-", "").ToLower()}";
        }
    }
}
