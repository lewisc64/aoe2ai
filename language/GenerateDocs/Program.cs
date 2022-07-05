using Language;
using NLog;
using NLog.Layouts;
using NLog.Targets;
using System.IO;
using System.Linq;
using System.Text;

namespace GenerateDocs
{
    public class Program
    {
        public static readonly DirectoryInfo Folder = new DirectoryInfo("..\\..\\..\\..\\..\\documentation");

        public static void Main(string[] args)
        {
            var config = new NLog.Config.LoggingConfiguration();
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, new ColoredConsoleTarget { Layout = new SimpleLayout("${message}") });
            LogManager.Configuration = config;

            var transpiler = new Transpiler();

            foreach (var file in Folder.EnumerateFiles())
            {
                if (file.Extension == ".md")
                {
                    file.Delete();
                }
            }

            foreach (var rule in transpiler.Rules)
            {
                var content = new StringBuilder();

                content.AppendLine($"# {rule.Name}");

                if (!string.IsNullOrEmpty(rule.Help))
                {
                    content.AppendLine(rule.Help);
                }

                if (!string.IsNullOrEmpty(rule.Usage))
                {
                    content.AppendLine("## Usage");
                    content.AppendLine("```");
                    content.AppendLine(rule.Usage);
                    content.AppendLine("```");
                }

                if (rule.Examples.Any())
                {
                    content.AppendLine("## Examples");
                    foreach (var example in rule.Examples)
                    {
                        var transpiled = transpiler.Transpile(example).Render();

                        content.AppendLine("```");
                        content.AppendLine(example);
                        content.AppendLine("```");
                        content.AppendLine("```");
                        content.AppendLine(transpiled);
                        content.AppendLine("```");
                        content.AppendLine("---");
                    }
                }

                File.WriteAllText(Path.Join(Folder.FullName, $"{rule.Name}.md").ToString(), content.ToString());
            }
        }
    }
}
