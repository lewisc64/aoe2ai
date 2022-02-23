using Language.Extensions;
using Language.ScriptItems;
using System.IO;
using System.Linq;

namespace Language.Rules
{
    [ActiveRule]
    public class Load : RuleBase
    {
        public override string Name => "load";

        public override string Help => "Loads another aoe2ai file. Tries to load relatively from the current file first, then an absolute path.";

        public override string Usage => "load \"PATH\"";

        public Load()
            : base(@"^load ""(?<rel>[^""]+)""$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var inputPath = GetData(line)["rel"].Value;

            FileInfo path;

            if (context.CurrentPath is not null && File.Exists(Path.Combine(context.CurrentPath, inputPath)))
            {
                path = new FileInfo(Path.Combine(context.CurrentPath, inputPath));
            }
            else if (Path.IsPathFullyQualified(inputPath) && File.Exists(inputPath))
            {
                path = new FileInfo(inputPath);
            }
            else
            {
                throw new System.ArgumentException($"File '{inputPath}' does not exist.");
            }

            var content = File.ReadAllText(path.FullName);

            Script rules = null;

            context.UsingSubcontext(subcontext =>
            {
                subcontext.CurrentFileName = path.Name;
                subcontext.CurrentPath = path.DirectoryName;

                var transpiler = new Transpiler();
                rules = transpiler.Transpile(content, subcontext);
            });

            if (context.ConditionStack.Any())
            {
                foreach (var rule in rules)
                {
                    (rule as Defrule)?.Actions.AddRange(context.ActionStack);
                }
                context.AddToScriptWithJump(rules, Condition.JoinConditions("and", context.ConditionStack).Invert());
            }
            else
            {
                context.AddToScript(context.ApplyStacks(rules));
            }
        }
    }
}
