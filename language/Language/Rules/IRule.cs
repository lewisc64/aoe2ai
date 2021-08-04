using System;

namespace Language.Rules
{
    public interface IRule
    {
        string Name { get; }

        bool Match(string line);

        void Parse(string line, TranspilerContext context);
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
