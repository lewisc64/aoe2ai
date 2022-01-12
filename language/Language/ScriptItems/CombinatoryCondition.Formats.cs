using System;
using System.Collections.Generic;
using System.Text;

namespace Language.ScriptItems
{
    public interface ICombinatoryConditionFormat
    {
        string Format(string text, IEnumerable<Condition> conditions);
    }

    public class IndentedCondition : ICombinatoryConditionFormat
    {
        public string Indentation { get; set; } = new string(' ', 2);

        public string LineSeparator { get; set; } = Environment.NewLine;

        public string Format(string text, IEnumerable<Condition> conditions)
        {
            var output = new StringBuilder($"({text}");
            foreach (var condition in conditions)
            {
                foreach (var line in condition.ToString().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    output.Append($"{LineSeparator}{Indentation}{line}");
                }
            }
            output.Append(Environment.NewLine);
            output.Append(")");
            return output.ToString();
        }
    }

    public class OneLineCondition : ICombinatoryConditionFormat
    {
        public string Spacing { get; set; } = string.Empty;

        public string Format(string text, IEnumerable<Condition> conditions)
        {
            return $"({text}{Spacing}{string.Join(Spacing, conditions)})";
        }
    }
}
