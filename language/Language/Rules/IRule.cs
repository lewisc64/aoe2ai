using System;

namespace Language.Rules
{
    public interface IRule
    {
        string Name { get; }

        string Help { get; }

        string Usage { get; }

        string Example { get; }

        bool Match(string line);

        void Parse(string line, TranspilerContext context);
    }
}
