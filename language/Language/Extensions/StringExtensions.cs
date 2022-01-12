using System;
using System.Collections.Generic;
using System.Linq;

namespace Language.Extensions
{
    public static class StringExtensions
    {
        public static string ReplaceIfNullOrEmpty(this string s, string replacement)
        {
            return string.IsNullOrEmpty(s) ? replacement : s;
        }

        public static string Wrap(this string text, int length)
        {
            var outputLines = new List<string>();
            foreach (var line in text.Split(Environment.NewLine))
            {
                if (line.Length > length)
                {
                    var segments = new List<string>();
                    var currentLength = 0;
                    foreach (var segment in GetWrappingSegments(line))
                    {
                        if (segment.Length + 1 + currentLength > length)
                        {
                            outputLines.Add(string.Join(" ", segments));
                            segments.Clear();
                            segments.Add(segment);
                            currentLength = segment.Length;
                        }
                        else
                        {
                            segments.Add(segment);
                            currentLength += segment.Length + 1;
                        }
                    }
                    if (segments.Any())
                    {
                        outputLines.Add(string.Join(" ", segments));
                    }
                }
                else
                {
                    outputLines.Add(line);
                }
            }
            return string.Join(Environment.NewLine, outputLines);
        }

        private static IEnumerable<string> GetWrappingSegments(string line)
        {
            return line.Split(" ")
                .Aggregate(new List<string>(), (acc, segment) =>
                {
                    if (acc.Any() && acc.Last().Count(x => x == '"') % 2 == 1)
                    {
                        acc[acc.Count - 1] += " " + segment;
                    }
                    else
                    {
                        acc.Add(segment);
                    }
                    return acc;
                });
        }
    }
}
