using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Language.Extensions
{
    public static class StringExtensions
    {
        public static string ReplaceIfNullOrEmpty(this string s, string replacement)
        {
            return string.IsNullOrEmpty(s) ? replacement : s;
        }
    }
}
