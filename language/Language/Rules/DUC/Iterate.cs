using Language.ScriptItems;
using System.Linq;

namespace Language.Rules.DUC
{
    [ActiveRule]
    public class Iterate : RuleBase
    {
        public override string Name => "DUC iterate";

        public override string Help => "TODO";

        public override string Usage => "TODO";

        public Iterate()
            : base(@"^(?:#iterate (?:group (?<group>[^ ]+)|(?<object>[^ ]+))|#end iterate)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);

            if (line.StartsWith("#end"))
            {
                var looperRuleId = (string)context.DataStack.Pop();
                var iterateGoal = (int)context.DataStack.Pop();
                var iterateCondition = context.ConditionStack.Pop();

                var relativeJumpValue = -1;

                for (var i = context.Script.Items.FindIndex(x => x is Defrule rule && rule.Id == looperRuleId); i < context.Script.Items.Count; i++)
                {
                    if (context.Script.Items[i] is Defrule scriptRule)
                    {
                        scriptRule.Compressable = false;
                        scriptRule.Splittable = false;
                        relativeJumpValue -= 1;
                    }
                }

                var rule = new Defrule(
                    new[]
                    {
                        iterateCondition,
                    },
                    new[]
                    {
                        new Action($"up-modify-goal {iterateGoal} c:+ 1"),
                        new Action($"up-jump-rule {relativeJumpValue}"),
                    });

                context.FreeVolatileGoal(iterateGoal);

                context.AddToScript(context.ApplyStacks(rule));
            }
            else
            {
                var iterateGoal = context.CreateVolatileGoal();
                var localTotal = context.CreateConsecutiveGoals(4);

                string searchAction;
                if (data["group"].Success)
                {
                    searchAction = $"up-set-group search-local c: {data["group"].Value}";
                }
                else
                {
                    searchAction = $"up-find-local c: {data["object"].Value} c: 255";
                }

                var rules = new[]
                {
                    new Defrule(
                        new[]
                        {
                            "true",
                        },
                        new[]
                        {
                            $"set-goal {iterateGoal} 0",
                        }),
                    new Defrule(
                        new[]
                        {
                            "true",
                        },
                        new[]
                        {
                            "up-reset-search 1 1 0 0",
                            searchAction,
                            $"up-remove-objects search-local {Game.ObjectDataIndex} g:!= {iterateGoal}",
                            $"up-get-search-state {localTotal}"
                        })
                    {
                        Compressable = false,
                        Splittable = false,
                    },
                };

                context.AddToScript(context.ApplyStacks(rules));

                context.DataStack.Push(iterateGoal);
                context.DataStack.Push(rules.Last().Id);

                context.ConditionStack.Push(new Condition($"up-compare-goal {localTotal} c:!= 0"));
            }
        }
    }
}
