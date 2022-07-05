﻿using Language;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GenerateDocs
{
    class Program
    {
        static void Main(string[] args)
        {
            var transpiler = new Transpiler();

            var rules = transpiler.Rules;

            var ruleDocLines = new List<string>();
            ruleDocLines.Add("# Rules");
            ruleDocLines.Add("Below are all the rules and structures recognised by the parser.");

            foreach (var rule in rules.OrderBy(x => x.Name))
            {
                ruleDocLines.Add($"## {rule.Name}");

                if (!string.IsNullOrEmpty(rule.Help))
                {
                    ruleDocLines.Add(rule.Help);
                }

                if (!string.IsNullOrEmpty(rule.Usage))
                {
                    ruleDocLines.Add("### Usage");
                    ruleDocLines.Add("```");
                    ruleDocLines.Add(rule.Usage);
                    ruleDocLines.Add("```");
                }

                if (!string.IsNullOrEmpty(rule.Example))
                {
                    ruleDocLines.Add("### Example");
                    ruleDocLines.Add("```");
                    ruleDocLines.Add(rule.Example);
                    ruleDocLines.Add("```");
                }
            }

            File.WriteAllText(@"..\..\..\..\..\RULES.md", string.Join("\n", ruleDocLines));
        }
    }
}
