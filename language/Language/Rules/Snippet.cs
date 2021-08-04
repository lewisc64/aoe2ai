using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules
{
    public class Snippet : RuleBase
    {
        private IEnumerable<string> Conditions { get; }

        private IEnumerable<string> Actions { get; }

        public Snippet(string trigger, IEnumerable<string> conditions, IEnumerable<string> actions)
            : base($@"^{trigger}$")
        {
            Name = trigger;
            Conditions = conditions;
            Actions = actions;
        }

        public override void Parse(string line, TranspilerContext context)
        {
            context.AddToScript(new Defrule(Conditions, Actions));
        }
    }
}
