﻿using Language.ScriptItems;

namespace Language.Rules
{
    [ActiveRule]
    public class SetConst : RuleBase
    {
        public override string Name => "set const";

        public SetConst()
            : base(@"^const (?<name>[^ ]+) ?= ?(?<value>.+)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var name = data["name"].Value;
            var value = data["value"].Value;

            context.AddToScript(new Defconst(name, value));
        }
    }
}