﻿using Language;
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
        static void Main(string[] args)
        {
            if (args.Length < 3 || args.Contains("-help"))
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("parsefile.exe INPUT_PATH OUTPUT_FOLDER_PATH AI_NAME");
                return;
            }

            var inputPath = new FileInfo(args[0]);
            var outputPath = new DirectoryInfo(args[1]);
            var name = args[2];

            var content = File.ReadAllText(inputPath.FullName);

            var config = new NLog.Config.LoggingConfiguration();
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, new ColoredConsoleTarget { Layout = new SimpleLayout("${level:uppercase=true}|${logger}|${message}") });
            LogManager.Configuration = config;

            var transpiler = new Transpiler();
            var output = transpiler.Transpile(content, new TranspilerContext { CurrentFileName = inputPath.Name, RootPath = inputPath.DirectoryName, CurrentPath = inputPath.DirectoryName });

            var aiFilePath = Path.Combine(outputPath.FullName, $"{name}.ai");
            var perFilePath = Path.Combine(outputPath.FullName, $"{name}.per");

            if (!File.Exists(aiFilePath))
            {
                File.Create(aiFilePath);
                Console.WriteLine($"Saved to '{aiFilePath}'");
            }
            File.WriteAllText(perFilePath, ";Translated by https://github.com/lewisc64/aoe2ai\n" + string.Join("\n", output));
            Console.WriteLine($"Saved to '{perFilePath}'");
        }
    }
}
