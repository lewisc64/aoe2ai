using Language.ScriptItems;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Language.Rules
{
    [ActiveRule]
    public class Call : RuleBase
    {
        private static readonly Regex ParamRegex = new Regex(@"(?<name>[^\s(,]+)\s*=\s*(?<value>(?:""(?:\\""|[^""])+""|[0-9]+))\s*");

        public override string Name => "call";

        public Call()
            : base(@"^call (?<name>[^ ()]+)(?:\(.+\))?$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var subroutineName = GetData(line)["name"].Value;

            var parameters = new Dictionary<string, string>();

            foreach (var match in ParamRegex.Matches(line).Cast<Match>())
            {
                var name = match.Groups["name"].Value;
                var value = match.Groups["value"].Value;

                if (value.StartsWith("\"") && value.EndsWith("\""))
                {
                    value = value.Trim('"');
                }
                parameters[name] = value.Replace("\\\"", "\"");
            }

            var subroutine = context.Subroutines[subroutineName];

            foreach ((string key, string value) in parameters)
            {
                subroutine = subroutine.Replace($"{{{key}}}", value);
            }

            var subcontext = context.Copy();
            subcontext.Script.Clear();

            var transpiler = new Transpiler();
            var rules = transpiler.Transpile(subroutine, subcontext);

            if (context.ConditionStack.Any())
            {
                context.AddToScriptWithJump(rules, Condition.JoinConditions("and", context.ConditionStack).Invert());
            }
            else
            {
                context.AddToScript(rules);
            }
        }
    }
}
