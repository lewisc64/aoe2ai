using Language.ScriptItems;
using System.Linq;

namespace Language.Extensions
{
    public static class ConditionExtensions
    {
        public static Condition Invert(this Condition condition)
        {
            if ((condition as CombinatoryCondition)?.Text == "not")
            {
                return ((CombinatoryCondition)condition).Conditions.Single();
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
