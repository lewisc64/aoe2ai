using System.Collections.Generic;

namespace Language.Rules
{
    public interface IRule
    {
        string Name { get; }

        string Help { get; }

        string Usage { get; }

        IEnumerable<string> Examples { get; }

        bool Match(string line);

        void Parse(string line, TranspilerContext context);
    }
}
