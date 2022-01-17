using Language;
using Language.ScriptItems.Formats;
using NLog;
using NLog.Layouts;
using NLog.Targets;
using System;
using TextCopy;

namespace Snippeter
{
    static class Program
    {
        static void Main()
        {
            var config = new NLog.Config.LoggingConfiguration();
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, new ColoredConsoleTarget { Layout = new SimpleLayout("${message}") });
            LogManager.Configuration = config;

            Console.WriteLine("Welcome to the aoe2ai snippeter!");
            Console.WriteLine("Results will be automatically copied to the clipboard.\n");

            var clipboard = new Clipboard();
            var transpiler = new Transpiler();

            while (true)
            {
                Console.Write(">");
                var content = Console.ReadLine();

                var result = transpiler.Transpile(content).Render(new IFormatter[] { new IndentedDefrule(), new IndentedCondition() });
                if (!string.IsNullOrWhiteSpace(result))
                {
                    clipboard.SetText(result);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"\n{result}\n");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
        }
    }
}
