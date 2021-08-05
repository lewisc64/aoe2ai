using Language;
using NLog;
using System;

namespace Aoe2AI
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new NLog.Config.LoggingConfiguration();
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, new NLog.Targets.ColoredConsoleTarget());
            LogManager.Configuration = config;

            var t = new Transpiler();
            Console.WriteLine(string.Join("\n", t.Transpile(@"

load ""jotting.aoe2ai""

", new TranspilerContext { CurrentPath = @"E:\coding\GitHub\aoe2bots\bots" })));
        }
    }
}
