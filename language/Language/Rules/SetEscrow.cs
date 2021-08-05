﻿using Language.ScriptItems;

namespace Language.Rules
{
    [ActiveRule]
    public class SetEscrow : RuleBase
    {
        public override string Name => "set escrow";

        public SetEscrow()
            : base(@"^escrow (?<amount>[^ ]+) (?<resource>.+)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var amount = data["amount"].Value;
            var resource = data["resource"].Value;

            var rule = new Defrule(new[] { "true" }, new[] { $"set-escrow-percentage {resource} {amount}" });
            context.AddToScript(context.ApplyStacks(rule));
        }
    }
}