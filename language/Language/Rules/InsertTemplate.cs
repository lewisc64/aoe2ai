using Language.Extensions;
using Language.ScriptItems;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Language.Rules
{
    [ActiveRule]
    public class InsertTemplate : RuleBase
    {
        private static readonly Regex ParamRegex = new Regex(@"(?<name>[^\s(,]+?)\s*=\s*(?<value>(?:""(?:\\""|[^""])*""|[0-9]+))\s*");

        public override string Name => "insert template";

        public override string Help => "Inserts all the rules within a template. Can make replacements as a form of pre-processing.";

        public override string Usage => "insert TEMPLATE_NAME";

        public override IEnumerable<string> Examples => new[]
        {
            @"#template train-unit
    train {unit}
#end template

insert train-unit(unit=""archer-line"")",
        };

        public InsertTemplate()
            : base(@"^insert (?<name>[^ ()]+)(?:\(.+\))?$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var templateName = GetData(line)["name"].Value;

            if (!context.Templates.ContainsKey(templateName))
            {
                throw new System.InvalidOperationException($"Template '{templateName}' is not defined.");
            }

            var parameters = new Dictionary<string, string>();

            foreach (var match in ParamRegex.Matches(line).Cast<Match>())
            {
                var name = match.Groups["name"].Value;
                var value = match.Groups["value"].Value;

                if (value.StartsWith("\"") && value.EndsWith("\""))
                {
                    value = value.Substring(1, value.Length - 2);
                }
                parameters[name] = value.Replace("\\\"", "\"");
            }

            var template = context.Templates[templateName];

            foreach ((string key, string value) in parameters)
            {
                template = template.Replace($"{{{key}}}", value);
            }

            Script rules = null;

            context.UsingSubcontext(subcontext =>
            {
                subcontext.CurrentFileName = $"template '{templateName}'";

                var transpiler = new Transpiler();
                rules = transpiler.Transpile(template, subcontext);
            });

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
