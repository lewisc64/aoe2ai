using System.Collections.Generic;
using System.Linq;

namespace Language.ScriptItems
{
    public class CombinatoryCondition : Condition
    {
        public static readonly ICombinatoryConditionFormat DefaultFormat = new IndentedCondition();

        public IEnumerable<Condition> Conditions { get; }

        public override int Length => Conditions.Select(x => x.Length).Sum();

        public ICombinatoryConditionFormat Format { get; set; } = DefaultFormat;

        public CombinatoryCondition(string text, IEnumerable<Condition> conditions)
            : base(text)
        {
            Conditions = conditions;
        }

        public override string ToString()
        {
            return Format.Format(Text, Conditions);
        }

        public override CombinatoryCondition Copy()
        {
            return new CombinatoryCondition(Text, Conditions.Select(x => x.Copy()));
        }
    }
}
