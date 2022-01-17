using Language.ScriptItems;
using System.Linq;

namespace Language.Rules
{
    [ActiveRule(10)]
    public class Order : RuleBase // TODO: prevent two being executed on the same rule pass.
    {
        public override string Name => "order";

        public override string Help => "Executes statements in order once every rule pass. Loops back to the beginning upon reaching the end.";

        public override string Example => "train archer-line => train skirmisher-line";

        public Order()
            : base(@"^(?<first>.+?) *=> *(?<second>.+)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);

            var first = data["first"].Value;
            var second = data["second"].Value;

            var transpiler = new Transpiler();

            var goalNumber = context.CreateGoal();

            var subcontext = context.Copy();
            subcontext.Script.Items.Clear();
            subcontext.CurrentFileName = $"{subcontext.CurrentFileName} -> order expression side 1";

            var firstRules = transpiler.Transpile(first, subcontext, suppressStackWarnings: true);

            subcontext = context.Copy();
            subcontext.Script.Items.Clear();
            subcontext.CurrentFileName = $"{subcontext.CurrentFileName} -> order expression side 2";

            var secondRules = transpiler.Transpile(second, subcontext, suppressStackWarnings: true);

            foreach (var rule in firstRules)
            {
                if (rule is Defrule)
                {
                    ((Defrule)rule).Conditions.Add(new Condition($"goal {goalNumber} 0"));
                }
            }
            ((Defrule)firstRules.Last()).Actions.Add(new Action($"set-goal {goalNumber} 1"));

            foreach (var rule in secondRules)
            {
                if (rule is Defrule)
                {
                    ((Defrule)rule).Conditions.Add(new Condition($"goal {goalNumber} 1"));
                }
            }
            ((Defrule)secondRules.Last()).Actions.Add(new Action($"set-goal {goalNumber} 0"));

            context.AddToScript(new[] { new Defrule(new[] { "true" }, new[] { $"set-goal {goalNumber} 0", "disable-self" }) }.Concat(secondRules).Concat(firstRules));
        }
    }
}
