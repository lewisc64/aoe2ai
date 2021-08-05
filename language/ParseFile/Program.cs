using System;
using System.Linq;
using System.IO;
using System.Reflection;
using Language;

namespace ParseFile
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3 || args.Contains("-help"))
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("ParseFile.exe INPUT_PATH OUTPUT_FOLDER_PATH AI_NAME");
                return;
            }

            var inputPath = new FileInfo(args[0]);
            var outputPath = new DirectoryInfo(args[1]);
            var name = args[2];

            var content = File.ReadAllText(inputPath.FullName);

            var transpiler = new Transpiler();

            var output = transpiler.Transpile(content, new TranspilerContext { CurrentFileName = inputPath.Name, CurrentPath = inputPath.DirectoryName });

            var aiFilePath = Path.Combine(outputPath.FullName, $"{name}.ai");
            var perFilePath = Path.Combine(outputPath.FullName, $"{name}.per");

            if (!File.Exists(aiFilePath))
            {
                File.Create(aiFilePath);
            }
            File.WriteAllText(perFilePath, ";Translated by https://github.com/lewisc64/aoe2ai\n" + string.Join("\n", output));
        }
    }
}
