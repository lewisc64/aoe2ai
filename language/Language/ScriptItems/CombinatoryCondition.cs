using System.Collections.Generic;
using System.Linq;

namespace Language.ScriptItems
{
    public class CombinatoryCondition : Condition
    {
        IEnumerable<Condition> Conditions { get; }

        public override int Length => Conditions.Select(x => x.Length).Sum();

        public CombinatoryCondition(string text, IEnumerable<Condition> conditions)
            : base(text)
        {
            Conditions = conditions;
        }

        public override string ToString()
        {
            return $"({Text} {string.Join(" ", Conditions)})";
        }

        public override CombinatoryCondition Copy()
        {
            return new CombinatoryCondition(Text, Conditions.Select(x => x.Copy()));
        }
    }
}
