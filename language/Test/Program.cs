using Language;
using Language.ScriptItems;
using System;

namespace Aoe2AI
{
    class Program
    {
        static void Main(string[] args)
        {
            var t = new Transpiler();
            Console.WriteLine(t.Transpile(@"

#if a
    rule
#else if b
    rule
#else if c
    rule
#else
    rule
#end if
rule
#if current-age == feudal-age
    rule
#end if
"));
        }
    }
}
