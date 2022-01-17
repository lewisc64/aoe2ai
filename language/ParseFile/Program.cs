using Language;
using Language.ScriptItems;
using Language.ScriptItems.Formats;
using NLog;
using NLog.Layouts;
using NLog.Targets;
using System;
using System.IO;
using System.Linq;

namespace ParseFile
{
    class Program
    {
        public static void Main(string[] args)
        {
            var parsedArgs = new ArgParser(args);

            if (parsedArgs.PositionalArguments.Count != 3 || parsedArgs.PositionalArguments.Contains("help"))
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("  parsefile.exe INPUT_PATH OUTPUT_FOLDER_PATH AI_NAME");
                Console.WriteLine("Flags:");
                Console.WriteLine("  --minify");
                return;
            }

            var inputPath = new FileInfo(parsedArgs.PositionalArguments[0]);
            var outputPath = new DirectoryInfo(parsedArgs.PositionalArguments[1]);
            var name = parsedArgs.PositionalArguments[2];

            var minify = parsedArgs.Flags.Contains("--minify");

            var content = File.ReadAllText(inputPath.FullName);

            var config = new NLog.Config.LoggingConfiguration();
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, new ColoredConsoleTarget { Layout = new SimpleLayout("${level:uppercase=true}|${logger}|${message}") });
            LogManager.Configuration = config;

            var transpiler = new Transpiler();
            var context = new TranspilerContext
            {
                CurrentFileName = inputPath.Name,
                RootPath = inputPath.DirectoryName,
                CurrentPath = inputPath.DirectoryName,
            };

            var output = transpiler.Transpile(content, context);
            string outputContent;

            if (minify)
            {
                output.InsertLineBreaks = false;
                outputContent = output.Render(new IScriptItemFormat[] { new OneLineCondition(), new OneLineDefrule() });
            }
            else
            {
                outputContent = output.Render();
            }

            var aiFilePath = Path.Combine(outputPath.FullName, $"{name}.ai");
            var perFilePath = Path.Combine(outputPath.FullName, $"{name}.per");

            if (!File.Exists(aiFilePath))
            {
                File.Create(aiFilePath);
                Console.WriteLine($"Saved to '{aiFilePath}'");
            }
            File.WriteAllText(perFilePath, $";Translated by https://github.com/lewisc64/aoe2ai{Environment.NewLine}{outputContent}");
            Console.WriteLine($"Saved to '{perFilePath}'");

            Analyze(context);
        }

        private static void Analyze(TranspilerContext context)
        {
            Console.WriteLine($"There are {context.Script.Count(x => x is Defrule)} rules.");
            Console.WriteLine($"{context.Goals.Count} goals and {context.Timers.Count} timers were used.");
        }
    }
}
