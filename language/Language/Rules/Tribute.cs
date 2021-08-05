﻿using Language.ScriptItems;

namespace Language.Rules
{
    [ActiveRule]
    public class Tribute : RuleBase
    {
        public override string Name => "tribute";

        public Tribute()
            : base(@"^tribute (?<amount>[^ ]+) (?<resource>[^ ]+) to (?:player )?(?<player>[^ ]+)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);

            var amount = data["amount"].Value;
            var resource = data["resource"].Value;
            var player = data["player"].Value;

            var rule = new Defrule(new[] { "true" }, new[] { $"tribute-to-player {player} {resource} {amount}" });
            context.AddToScript(context.ApplyStacks(rule));
        }
    }
}