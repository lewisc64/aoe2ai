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
set up basics
lure boars
"));
        }
    }
}
