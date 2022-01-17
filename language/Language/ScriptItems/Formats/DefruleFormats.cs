using System;
using System.Linq;
using System.Text;

namespace Language.ScriptItems.Formats
{
    public interface IDefruleFormat : IFormatter
    {
        string Format(Defrule rule, IConditionFormat conditionFormat);
    }

    public abstract class DefruleFormat : IDefruleFormat
    {
        public bool CanBeAppliedTo(IScriptItem scriptItem)
        {
            return scriptItem is Defrule;
        }
        public abstract string Format(Defrule rule, IConditionFormat conditionFormat);
    }

    public class IndentedDefrule : DefruleFormat
    {
        public string Indentation { get; set; } = new string(' ', 4);

        public string LineSeparator { get; set; } = Environment.NewLine;

        public override string Format(Defrule rule, IConditionFormat conditionFormat)
        {
            var output = new StringBuilder("(defrule");
            foreach (var condition in rule.Conditions)
            {
                foreach (var line in conditionFormat.Format(condition).Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    output.Append($"{LineSeparator}{Indentation}{line}");
                }
            }
            output.Append($"{LineSeparator}=>");
            foreach (var action in rule.Actions)
            {
                output.Append($"{LineSeparator}{Indentation}({action.Text})");
            }
            output.Append($"{LineSeparator})");
            return output.ToString();
        }
    }

    public class OneLineDefrule : DefruleFormat
    {
        public override string Format(Defrule rule, IConditionFormat conditionFormat)
        {
            return $"(defrule{string.Join(string.Empty, rule.Conditions.Select(x => conditionFormat.Format(x)))}=>{string.Join(string.Empty, rule.Actions)})";
        }
    }
}
