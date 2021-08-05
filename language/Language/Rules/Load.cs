using Language.ScriptItems;
using System.IO;
using System.Linq;

namespace Language.Rules
{
    [ActiveRule]
    public class Load : RuleBase
    {
        public override string Name => "load";

        public Load()
            : base(@"^load ""(?<rel>[^""]+)""$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var relativePath = GetData(line)["rel"].Value;
            var path = new FileInfo(Path.Combine(context.CurrentPath, relativePath));

            var subcontext = context.Copy();
            subcontext.CurrentFileName = path.Name;
            // disabled for absolute pathing from main file.
            // subcontext.CurrentPath = path.DirectoryName;
            subcontext.ActionStack.Clear();
            subcontext.ConditionStack.Clear();
            subcontext.DataStack.Clear();
            subcontext.Script.Clear();

            var transpiler = new Transpiler();
            var rules = transpiler.Transpile(File.ReadAllText(path.FullName), subcontext);

            context.Goals = subcontext.Goals;
            context.Timers = subcontext.Timers;
            context.Subroutines = subcontext.Subroutines;

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
