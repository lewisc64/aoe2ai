using Language.Rules;
using Language.ScriptItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Language
{
    public class Transpiler
    {
        public IEnumerable<Rule> Rules { get; } = new List<Rule>();

        public Transpiler()
        {
            Rules = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(ActiveRule)))
                .OrderByDescending(x => (int)x.GetCustomAttributesData().First(x => x.AttributeType == typeof(ActiveRule)).ConstructorArguments.First().Value)
                .Select(x => (Rule)Activator.CreateInstance(x));
        }

        public string Transpile(string source)
        {
            return string.Join("\n", Transpile(source, new TranspilerContext()));
        }

        public IEnumerable<IScriptItem> Transpile(string source, TranspilerContext context)
        {
            foreach (var line in source.Split(new[] { '\r', '\n' }).Select(x => x.Trim()))
            {
                foreach (var rule in Rules)
                {
                    if (rule.Match(line))
                    {
                        rule.Parse(line, context);
                    }
                }
            }

            context.OptimizeScript();

            return context.Script;
        }
    }
}
