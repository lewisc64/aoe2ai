using Language.ScriptItems;
using System;
using System.Text.RegularExpressions;

namespace Language.Rules
{
    [ActiveRule]
    public class Rule
    {
        private Regex _regex;

        public Rule(string regex)
        {
            _regex = new Regex(regex);
        }

        public Rule()
        {
            _regex = new Regex(@"^rule$");
        }

        public bool Match(string line)
        {
            return _regex.IsMatch(line);
        }

        public virtual void Parse(string line, TranspilerContext context)
        {
            context.AddToScript(new[] { new Defrule(new[] { "true" }, new[] { "do-nothing" }) });
        }

        protected GroupCollection GetData(string line)
        {
            return _regex.Match(line).Groups;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ActiveRule : Attribute
    {
        public int Priority { get; set; }

        public ActiveRule(int priority = 0)
        {
            Priority = priority;
        }
    }
}
