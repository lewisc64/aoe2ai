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
                Console.WriteLine("  --rule-length=16");
                Console.WriteLine("  --rule-length=32");
                return;
            }

            var inputPath = new FileInfo(parsedArgs.PositionalArguments[0]);
            var outputPath = new DirectoryInfo(parsedArgs.PositionalArguments[1]);
            var name = parsedArgs.PositionalArguments[2];

            var minify = parsedArgs.Flags.Contains("--minify");

            if (parsedArgs.Flags.Any(x => x.StartsWith("--rule-length=")))
            {
                Defrule.MaxRuleSize = int.Parse(parsedArgs.Flags.Last(x => x.StartsWith("--rule-length=")).Split("=").Last());
            }

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

            AddFastRulePassSkip(context);
            var output = transpiler.Transpile(content, context);

            string outputContent;

            if (minify)
            {
                output.InsertLineBreaks = false;
                outputContent = output.Render(new IFormatter[] { new OneLineCondition(), new OneLineDefrule() });
            }
            else
            {
                outputContent = output.Render();
            }

            Save(name, outputPath, outputContent);
            Analyze(context);
        }

        private static void AddFastRulePassSkip(TranspilerContext context)
        {
            var timeGoal = context.CreateVolatileGoal();
            var previousTimeGoal = context.CreateGoal();

            var rules = new[]
            {
                new Defrule(
                    new[]
                    {
                        "true",
                    },
                    new[]
                    {
                        $"up-get-precise-time 0 {timeGoal}",
                        $"up-modify-goal {timeGoal} g:- {previousTimeGoal}",
                        $"up-modify-goal {previousTimeGoal} g:+ {timeGoal}",
                    })
                {
                    Compressable = false,
                    Splittable = false,
                },
                new Defrule(
                    new[]
                    {
                        $"up-compare-goal {timeGoal} c:< 300",
                    },
                    new[]
                    {
                        "up-jump-rule 10000",
                    })
                {
                    Compressable = false,
                    Splittable = false,
                },
            };

            context.FreeVolatileGoal(timeGoal);
            context.AddToScript(rules);
        }

        private static void Save(string name, DirectoryInfo path, string content)
        {
            var aiFilePath = Path.Combine(path.FullName, $"{name}.ai");
            var perFilePath = Path.Combine(path.FullName, $"{name}.per");

            if (!File.Exists(aiFilePath))
            {
                File.Create(aiFilePath);
                Console.WriteLine($"Saved to '{aiFilePath}'");
            }
            File.WriteAllText(perFilePath, $";Translated by https://github.com/lewisc64/aoe2ai{Environment.NewLine}{content}");
            Console.WriteLine($"Saved to '{perFilePath}'");
        }

        private static void Analyze(TranspilerContext context)
        {
            Console.WriteLine($"There are {context.Script.Count(x => x is Defrule)} rules.");
            Console.WriteLine($"{context.Goals.Count} goals and {context.Timers.Count} timers were used.");
        }
    }
}
