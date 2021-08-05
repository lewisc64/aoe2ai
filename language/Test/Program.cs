using Language;
using Language.ScriptItems;
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
#delay by 5 seconds
    resign
#end delay

#if current-age == dark-age
    #delay by 5 minutes
        resign
    #end delay
#end if

#delay by 2 hours
    resign
#end delay
", new TranspilerContext { CurrentPath = @"E:\coding\GitHub\aoe2bots\bots" })));
        }
    }
}
