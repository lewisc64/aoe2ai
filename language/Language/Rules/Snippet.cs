using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules
{
    public class Snippet : RuleBase
    {
        protected virtual IEnumerable<string> Conditions { get; }

        protected virtual IEnumerable<string> Actions { get; }

        public Snippet(string trigger, IEnumerable<string> conditions, IEnumerable<string> actions)
            : base($@"^{trigger}$")
        {
            Name = trigger;
            Example = trigger;
            Conditions = conditions;
            Actions = actions;
        }

        public override void Parse(string line, TranspilerContext context)
        {
            context.AddToScript(context.ApplyStacks(new Defrule(Conditions, Actions)));
        }
    }
}
