using Language.ScriptItems;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Language.Rules
{
    [ActiveRule]
    public class InsertTemplate : RuleBase
    {
        private static readonly Regex ParamRegex = new Regex(@"(?<name>[^\s(,]+)\s*=\s*(?<value>(?:""(?:\\""|[^""])*""|[0-9]+))\s*");

        public override string Name => "insert template";

        public override string Help => "Inserts all the rules within a template. Can make replacements as a form of pre-processing.";

        public override string Usage => "insert TEMPLATE_NAME";

        public override string Example => @"#template train-unit
    train {unit}
#end template

insert train-unit(unit=""archer-line\"")
";

        public InsertTemplate()
            : base(@"^insert (?<name>[^ ()]+)(?:\(.+\))?$")
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

            var subroutine = context.Templates[subroutineName];

            foreach ((string key, string value) in parameters)
            {
                subroutine = subroutine.Replace($"{{{key}}}", value);
            }

            var subcontext = context.Copy();
            subcontext.Script.Clear();
            subcontext.ConditionStack.Clear();
            subcontext.ActionStack.Clear();
            subcontext.DataStack.Clear();
            subcontext.CurrentFileName = $"subroutine '{subroutineName}'";

            var transpiler = new Transpiler();
            var rules = transpiler.Transpile(subroutine, subcontext);

            context.Goals = subcontext.Goals;
            context.Timers = subcontext.Timers;
            context.Templates = subcontext.Templates;

            if (context.ConditionStack.Any())
            {
                foreach (var rule in rules)
                {
                    (rule as Defrule)?.Actions.AddRange(context.ActionStack);
                }
                context.AddToScriptWithJump(rules, Condition.JoinConditions("and", context.ConditionStack).Invert());
            }
            else
            {
                context.AddToScript(context.ApplyStacks(rules));
            }
        }
    }
}
