using Language.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Language
{
    public class Transpiler
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public List<IRule> Rules { get; } = new List<IRule>();

        public Transpiler()
        {
            Rules.AddRange(Snippet.GetSnippets());
            Rules.AddRange(RuleBase.GetRules());
        }

        public Script Transpile(string source)
        {
            return Transpile(source, new TranspilerContext());
        }

        public Script Transpile(string source, TranspilerContext context, bool suppressStackWarnings = false)
        {
            var withinTemplate = false;
            string templateName = null;
            string template = null;

            var lineNumber = 1;
            foreach (var line in Regex.Split(source, @"\r?\n").Select(x => x.Trim()))
            {
                var lineNoComments = line.Split("//").First().Trim();

                if (string.IsNullOrEmpty(lineNoComments))
                {
                    lineNumber++;
                    continue;
                }

                if (line.StartsWith("#template "))
                {
                    withinTemplate = true;
                    templateName = line.Split(' ').Last();
                    template = "";
                }
                else if (withinTemplate && line.StartsWith("#end template"))
                {
                    context.Templates[templateName] = template;
                    withinTemplate = false;
                }
                else if (withinTemplate)
                {
                    template += line + "\n";
                }
                else
                {
                    var matched = false;
                    foreach (var rule in Rules)
                    {
                        if (rule.Match(lineNoComments))
                        {
                            matched = true;
                            try
                            {
                                rule.Parse(lineNoComments, context);
                            }
                            catch (Exception ex)
                            {
                                Logger.Error($"Exception occurred while parsing a '{rule.Name}' statement in '{context.CurrentFileName}':{lineNumber}: {ex.Message}");
                            }
                            break;
                        }
                    }

                    if (!matched)
                    {
                        Logger.Warn($"Line {lineNumber} did not match: {line}");
                    }
                }

                lineNumber++;
            }

            if (!suppressStackWarnings)
            {
                if (context.ConditionStack.Any())
                {
                    Logger.Error($"Transpiling of '{context.CurrentFileName}' finished with a populated condition stack: {{ {string.Join(", ", context.ConditionStack)} }}");
                }

                if (context.ActionStack.Any())
                {
                    Logger.Error($"Transpiling of '{context.CurrentFileName}' finished with a populated action stack: {{ {string.Join(", ", context.ActionStack)} }}");
                }

                if (context.DataStack.Any())
                {
                    Logger.Error($"Transpiling of '{context.CurrentFileName}' finished with a populated internal data stack: {{ {string.Join(", ", context.DataStack)} }}");
                }
            }

            context.OptimizeScript();
            return context.Script;
        }
    }
}
