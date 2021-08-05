using Language.ScriptItems;
using System.Linq;

namespace Language.Rules
{
    [ActiveRule(10)]
    public class Order : RuleBase // TODO: prevent two being executed on the same rule pass.
    {
        public Order()
            : base(@"^(?<first>.+?) *=> *(?<second>.+)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);

            var first = data["first"].Value;
            var second = data["second"].Value;

            var goalNumber = context.CreateGoal();

            var subcontext = context.Copy();
            subcontext.Script.Clear();

            var transpiler = new Transpiler();

            var firstRules = transpiler.Transpile(first, subcontext);
            subcontext.Script.Clear();

            var secondRules = transpiler.Transpile(second, subcontext);

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
