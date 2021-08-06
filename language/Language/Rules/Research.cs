using Language.ScriptItems;
using System.Collections.Generic;
using System.Linq;

namespace Language.Rules
{
    [ActiveRule(-1)]
    public class Research : RuleBase
    {
        public override string Name => "research";

        public override string Help => "Sets up the rule to research the specified research.";

        public override string Usage => "research TECH_NAME with RESOURCE_NAME escrow";

        public override string Example => @"research ri-loom
research feudal-age with food and gold escrow
research blacksmith infantry upgrades";

        public static Dictionary<string, string[]> AllResearch => Game.GetResearches();

        public Research()
            : base($@"^research (?:(?<item>[^ ]+)|(?<category>{string.Join("|", AllResearch.Keys)}) upgrades)(?: with (?<escrowlist>(?:[^ ]+(?: and )?)*) escrow)?$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var item = data["item"].Value;
            var category = data["category"].Value;
            var escrowList = data["escrowlist"].Value;

            var conditions = new List<string>();
            var actions = new List<string>();

            if (!string.IsNullOrEmpty(escrowList))
            {
                conditions.Add("can-research-with-escrow {0}");
                foreach (var resource in escrowList.Split(" and "))
                {
                    actions.Add($"release-escrow {resource}");
                }
            }
            else
            {
                conditions.Add("can-research {0}");
            }

            actions.Add("research {0}");

            var rules = new List<Defrule>();

            foreach (var research in AllResearch.Keys.Contains(category) ? AllResearch[category] : new[] { item })
            {
                rules.Add(new Defrule(
                    conditions.Select(x => string.Format(x, research)),
                    actions.Select(x => string.Format(x, research))));
            }

            context.AddToScript(context.ApplyStacks(rules));
        }
    }
}
