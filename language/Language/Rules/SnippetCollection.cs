using System.Collections.Generic;

namespace Language.Rules
{
    public class SnippetCollection : RuleBase
    {
        private IEnumerable<Snippet> Snippets { get; }

        public SnippetCollection(string trigger, params Snippet[] snippets)
            : base($@"^{trigger}$")
        {
            Name = trigger;
            Snippets = snippets;
        }

        public override void Parse(string line, TranspilerContext context)
        {
            foreach (var snippet in Snippets)
            {
                snippet.Parse(line, context);
            }
        }
    }
}
