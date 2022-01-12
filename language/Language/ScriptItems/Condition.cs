using System.Collections.Generic;
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

        public static Condition JoinConditions(string operation, IEnumerable<Condition> conditions)
        {
            if (conditions.Count() == 1)
            {
                return conditions.Single();
            }
            return new CombinatoryCondition(operation, new[] { conditions.First(), JoinConditions(operation, conditions.Skip(1)) });
        }

        public static Condition Parse(string text, ICombinatoryConditionFormat format = null)
        {
            if (format == null)
            {
                format = CombinatoryCondition.DefaultFormat;
            }

            const string boundaryRegex = @"(?<=[()\s])\b|\b(?=[()\s])";

            text = DebracketExpression(text.Trim());

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
                        return new CombinatoryCondition(binop, new[] { Parse(left, format), Parse(right, format) })
                        {
                            Format = format,
                        };
                    }
                }
            }

            foreach (var unop in unops)
            {
                var segments = Regex.Split(text, boundaryRegex);
                if (segments.First().Trim() == unop)
                {
                    return new CombinatoryCondition(unop, new[] { Parse(string.Join("", segments.Skip(1)), format) })
                    {
                        Format = format,
                    };
                }
            }

            return new Condition(text);
        }

        public static string DebracketExpression(string expression)
        {
            while (expression.StartsWith("(") && expression.EndsWith(")"))
            {
                var trimmed = string.Join("", expression.Skip(1).Reverse().Skip(1).Reverse());
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
                    expression = trimmed;
                }
                else
                {
                    break;
                }
            }
            return expression;
        }
    }

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
    }
}
