using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules
{
    [ActiveRule]
    public class ChatDataTo : RuleBase
    {
        private static readonly Dictionary<string, string> DataTypeToTypeOpMap = new()
        {
            { "goal", "g:" },
            { "sn", "s:" },
            { "const", "c:" },
        };

        public override string Name => "chat data to";

        public override string Usage => "chat goal/sn/data to PLAYER_TYPE \"MESSAGE\" DATA";

        public override IEnumerable<string> Examples => new[]
        {
            "chat goal to all \"Hello everyone! Goal 1 is %d!\" 1",
            "chat sn to self \"My town size is: %d\" sn-maximum-town-size",
            @"@up-get-player-color my-player-number -1
chat const to allies ""I am color %s!"" 7031232",
        };

        public ChatDataTo()
            : base(@"^chat (?<datatype>goal|sn|const) to (?:(?<playerwildcard>all|self|allies)|(?<player>[^ ]+)) ""(?<message>.+)"" (?<data>[^ ]+)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var message = data["message"].Value;
            var insertData = data["data"].Value;

            string constName = ChatTo.GetUniqueKey(message);
            if (!context.Constants.Contains(constName))
            {
                context.AddToScript(context.CreateConstant(constName, message));
            }

            var typeOp = DataTypeToTypeOpMap[data["datatype"].Value];

            if (data["playerwildcard"].Success)
            {
                context.AddToScript(context.ApplyStacks(new Defrule(new[] { "true" }, new[] { $"up-chat-data-to-{data["playerwildcard"].Value} {constName} {typeOp} {insertData}".Replace("to-self", "local-to-self") })));
            }
            else
            {
                context.AddToScript(context.ApplyStacks(new Defrule(new[] { "true" }, new[] { $"up-chat-data-to-player {data["player"].Value} {constName} {typeOp} {insertData}" })));
            }
        }

    }
}
