﻿using Language.ScriptItems;

namespace Language.Rules.DUC
{
    [ActiveRule]
    public class RemoveFromList : RuleBase
    {
        public override string Name => "DUC remove from list";

        public override string Help => "TODO";

        public override string Usage => "TODO";

        public RemoveFromList()
            : base(@"^\$remove from (?<list>local|remote) where (?<condition>.+)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var list = data["list"].Value;
            var condition = data["condition"].Value;

            var rule = new Defrule(
                new[]
                {
                    "true",
                },
                new[]
                {
                    $"up-remove-objects search-{list} {condition}",
                });

            context.AddToScript(context.ApplyStacks(rule));
        }
    }
}
