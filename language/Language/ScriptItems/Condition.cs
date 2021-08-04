using System.Linq;
using System.Text.RegularExpressions;

namespace Language.ScriptItems
{
    public class Condition : ICopyable<Condition>
    {
        public string Text { get; set; }

        public virtual int Length => 1;

        public Condition(string text)
        {
            Text = text;
        }

        public override string ToString()
        {
            return $"({Text})";
        }

        public virtual Condition Copy()
        {
            return new Condition(Text);
        }

        public static Condition Parse(string text)
        {
            const string boundaryRegex = @"(?<=[()\s])\b|\b(?=[()\s])";

            text = text.Trim();

            while (text.StartsWith("(") && text.EndsWith(")"))
            {
                var trimmed = string.Join("", text.Skip(1).Reverse().Skip(1).Reverse());
                var bracketLevel = 0;
                foreach (var c in trimmed)
                {
                    if (c == '(')
                    {
                        bracketLevel++;
                    }
                    else if (c == ')' && bracketLevel > 0)
                    {
                        bracketLevel--;
                    }
                }
                if (bracketLevel == 0)
                {
                    text = trimmed;
                }
                else
                {
                    break;
                }
            }

            var binops = new[] { "or", "nor", "xor", "and", "nand" };
            var unops = new[] { "not" };

            foreach (var binop in binops)
            {
                var segments = Regex.Split(text, boundaryRegex);

                var bracketLevel = 0;
                for (var i = 0; i < segments.Count(); i++)
                {
                    var segment = segments.ElementAt(i);
                    if (segment.Trim() == "(")
                    {
                        bracketLevel++;
                    }
                    else if (segment.Trim() == ")")
                    {
                        bracketLevel--;
                    }
                    else if (segment.Trim() == binop && bracketLevel == 0)
                    {
                        var left = string.Join("", segments.Take(i));
                        var right = string.Join("", segments.Skip(i + 1));
                        return new CombinatoryCondition(binop, new[] { Parse(left), Parse(right) });
                    }
                }
            }

            foreach (var unop in unops)
            {
                var segments = Regex.Split(text, boundaryRegex);
                if (segments.First().Trim() == unop)
                {
                    return new CombinatoryCondition(unop, new[] { Parse(string.Join("", segments.Skip(1))) });
                }
            }

            return new Condition(text);
        }
    }

    public static class ConditionExtensions
    {
        public static Condition Invert(this Condition condition)
        {
            return new CombinatoryCondition("not", new[] { condition });
        }
    }
}
