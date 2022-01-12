using System;
using System.Collections.Generic;
using System.Text;

namespace Language.ScriptItems
{
    public interface IDefruleFormat
    {
        string Format(IEnumerable<Condition> conditions, IEnumerable<Action> actions);
    }

    public class IndentedDefrule : IDefruleFormat
    {
        public string Indentation { get; set; } = new string(' ', 4);

        public string LineSeparator { get; set; } = Environment.NewLine;

        public string Format(IEnumerable<Condition> conditions, IEnumerable<Action> actions)
        {
            var output = new StringBuilder("(defrule");
            foreach (var condition in conditions)
            {
                foreach (var line in condition.ToString().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    output.Append($"{LineSeparator}{Indentation}{line}");
                }
            }
            output.Append($"{LineSeparator}=>");
            foreach (var action in actions)
            {
                output.Append($"{LineSeparator}{Indentation}{action}");
            }
            output.Append($"{LineSeparator})");
            return output.ToString();
        }
    }

    public class OneLineDefrule : IDefruleFormat
    {
        public string Format(IEnumerable<Condition> conditions, IEnumerable<Action> actions)
        {
            return $"(defrule{string.Join(string.Empty, conditions)}=>{string.Join(string.Empty, actions)})";
        }
    }
}
