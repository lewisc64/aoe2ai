using Language.ScriptItems;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Language.Rules
{
    public abstract class RuleBase : IRule
    {
        private Regex _regex;

        public virtual string Name { get; set; } = "rule";

        public virtual string Help { get; set; } = string.Empty;

        public virtual string Usage { get; set; } = string.Empty;

        public virtual string Example { get; set; } = string.Empty;

        public RuleBase(string regex)
        {
            _regex = new Regex(regex);
        }

        public bool Match(string line)
        {
            return _regex.IsMatch(line);
        }

        public abstract void Parse(string line, TranspilerContext context);

        protected GroupCollection GetData(string line)
        {
            return _regex.Match(line).Groups;
        }

        protected (IEnumerable<Defrule> Rules, Dictionary<int, string> GoalToResourceMap) CreateNonEscrowedResourceGoals(TranspilerContext context, IEnumerable<string> resources)
        {
            var rules = new List<Defrule>();
            var goalToResourceMap = new Dictionary<int, string>();

            foreach (var resource in resources)
            {
                var nonEscrowedAmountGoal = context.CreateGoal();
                var escrowAmountGoal = context.CreateGoal();

                rules.Add(new Defrule(
                    new[]
                    {
                        "true",
                    },
                    new[]
                    {
                        $"up-get-fact resource-amount {resource} {nonEscrowedAmountGoal}",
                        $"up-get-fact escrow-amount {resource} {escrowAmountGoal}",
                        $"up-modify-goal {nonEscrowedAmountGoal} g:- {escrowAmountGoal}",
                    }));

                goalToResourceMap[nonEscrowedAmountGoal] = resource;
            }

            return (rules, goalToResourceMap);
        }
    }
}
