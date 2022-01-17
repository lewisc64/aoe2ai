using System;
using System.Linq;
using System.Text;

namespace Language.ScriptItems.Formats
{
    public interface IConditionFormat : IScriptItemFormat
    {
        string Format(Condition condition);
    }

    public abstract class ConditionFormat : IConditionFormat
    {
        public bool CanBeAppliedTo(IScriptItem scriptItem)
        {
            return scriptItem is Condition || scriptItem is CombinatoryCondition;
        }

        public abstract string Format(Condition condition);
    }

    public class IndentedCondition : ConditionFormat
    {
        public string Indentation { get; set; } = new string(' ', 2);

        public string LineSeparator { get; set; } = Environment.NewLine;

        public override string Format(Condition condition)
        {
            var combCondition = condition as CombinatoryCondition;
            if (combCondition != null)
            {
                var output = new StringBuilder($"({combCondition.Text}");
                foreach (var subCondition in combCondition.Conditions)
                {
                    foreach (var line in Format(subCondition).Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        output.Append($"{LineSeparator}{Indentation}{line}");
                    }
                }
                output.Append(Environment.NewLine);
                output.Append(")");
                return output.ToString();
            }
            return $"({condition.Text})";
        }
    }

    public class OneLineCondition : ConditionFormat
    {
        public string Spacing { get; set; } = string.Empty;

        public override string Format(Condition condition)
        {
            var combCondition = condition as CombinatoryCondition;
            if (combCondition != null)
            {
                return $"({combCondition.Text}{Spacing}{string.Join(Spacing, combCondition.Conditions.Select(x => Format(x)))})";
            }
            return $"({condition.Text})";
        }
    }
}
