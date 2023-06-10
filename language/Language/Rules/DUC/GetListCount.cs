using Language.ScriptItems;
using System.Linq;

namespace Language.Rules.DUC
{
    [ActiveRule]
    public class GetListCount : RuleBase
    {
        public override string Name => "DUC get list count";

        public override string Help => "TODO";

        public override string Usage => "TODO";

        public GetListCount()
            : base(@"^\$set goal (?<goal>[^ ]+) to (?<info>(?:local|remote) list length)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var goalName = data["goal"].Value;
            var info = data["info"].Value;

            if (context.Goals.Values.Contains(goalName))
            {
                throw new System.InvalidOperationException($"Goal name '{goalName}' is already in use.");
            }

            var localTotal = context.CreateGoal(info == "local list length" ? goalName : null);
            var localLast = context.CreateGoal();
            var remoteTotal = context.CreateGoal(info == "remote list length" ? goalName : null);
            var remoteLast = context.CreateGoal();

            if (localTotal != localLast - 1 || localTotal != remoteTotal - 2 || localTotal != remoteLast - 3)
            {
                throw new System.InvalidOperationException("Goals were not created consecutively.");
            }

            var rule = new Defrule(
                new[]
                {
                    "true",
                },
                new[]
                {
                    $"up-get-search-state {localTotal}",
                });

            context.AddToScript(context.ApplyStacks(rule));
        }
    }
}
