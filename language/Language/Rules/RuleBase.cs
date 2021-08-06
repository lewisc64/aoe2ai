using System.Text.RegularExpressions;

namespace Language.Rules
{
    public abstract class RuleBase : IRule
    {
        private Regex _regex;

        public virtual string Name { get; set; } = "rule";

        public virtual string Help { get; set; } = string.Empty;

        public virtual string Usage { get; set; } = string.Empty;

        public virtual string Example { get; set; } = string.Empty;

        public RuleBase(string regex)
        {
            _regex = new Regex(regex);
        }

        public bool Match(string line)
        {
            return _regex.IsMatch(line);
        }

        public abstract void Parse(string line, TranspilerContext context);

        protected GroupCollection GetData(string line)
        {
            return _regex.Match(line).Groups;
        }
    }
}
