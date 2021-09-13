using System.Collections.Generic;

namespace Language.Rules
{
    public class SnippetCollection : Snippet
    {
        public IEnumerable<Snippet> Snippets { get; }

        public SnippetCollection(string trigger, params Snippet[] snippets)
            : base($@"^{trigger}$", null, null)
        {
            Name = trigger;
            Usage = trigger;
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
