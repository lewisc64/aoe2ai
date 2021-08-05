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
#subroutine archer-response
    #respond to archer-line
        train 10 {unit}
    #end respond
#end subroutine

call archer-response(unit=""monk"")

chat to all ""hi""

#if current-age == feudal-age
    call archer-response(unit=""skirmisher-line"")
    call archer-response(unit=""scout-cavalry-line"")
#else
    call archer-response(unit=""scorpion-line"")
#end if
call archer-response(unit=""mangonel-line"")
", new TranspilerContext { CurrentPath = @"E:\coding\GitHub\aoe2bots\bots" })));
        }
    }
}
