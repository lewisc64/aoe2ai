using Language.ScriptItems;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Language.Rules
{
    [ActiveRule]
    public class Load : RuleBase
    {
        public override string Name => "load";

        public Load()
            : base(@"^load ""(?<rel>[^""]+)""$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var relativePath = GetData(line)["rel"].Value;
            var path = Path.Combine(context.CurrentPath, relativePath);

            var transpiler = new Transpiler();
            var rules = transpiler.Transpile(
                File.ReadAllText(path),
                new TranspilerContext
                {
                    ActionStack = new Stack<Action>(context.ActionStack.Select(x => x.Copy())),
                    CurrentPath = context.CurrentPath,
                });

            context.AddToScriptWithJump(rules, Condition.JoinConditions("and", context.ConditionStack).Invert());
        }
    }
}
