using Language.ScriptItems;
using System.Collections.Generic;
using System.Linq;

namespace Language.Extensions
{
    public static class ConditionExtensions
    {
        private static readonly Dictionary<string, string> InvertMappings = new Dictionary<string, string>
        {
            { "and", "nand" },
            { "nand", "and" },
            { "or", "nor" },
            { "nor", "or" },
        };

        public static Condition Invert(this Condition condition)
        {
            var combCondition = condition as CombinatoryCondition;
            if (combCondition?.Text == "not")
            {
                return ((CombinatoryCondition)condition).Conditions.Single();
            }
            else if (combCondition != null && InvertMappings.ContainsKey(combCondition?.Text))
            {
                return new CombinatoryCondition(InvertMappings[combCondition.Text], combCondition.Conditions);
            }
            return new CombinatoryCondition("not", new[] { condition });
        }

        public static Condition DeMorgans(this Condition condition)
        {
            var combCondition = condition as CombinatoryCondition;
            if (combCondition == null || combCondition.Text != "and" && combCondition.Text != "or")
            {
                throw new System.ArgumentException("Must be a combinatory condition and/or.");
            }
            if (combCondition.Conditions.Count() != 2)
            {
                throw new System.ArgumentException("Must have two sub conditions.");
            }
            return new CombinatoryCondition(combCondition.Text == "and" ? "or" : "and", combCondition.Conditions.Select(x => x.Invert())).Invert();
        }
    }
}
