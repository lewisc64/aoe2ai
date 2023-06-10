using Language.Extensions;
using Language.ScriptItems;

namespace Language.Rules.DUC
{
    [ActiveRule]
    public class Find : RuleBase
    {
        public override string Name => "DUC find";

        public override string Help => "TODO";

        public override string Usage => "TODO";

        public Find()
            : base(@"^\$find (?<list>local|remote)(?: (?<amount>[^ ]+))? (?<object>[^ ]+)(?: within (?<distance>[^ ]+) tiles of target)?$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var listName = data["list"].Value;
            var objectName = data["object"].Value;
            var amount = data["amount"].Value.ReplaceIfNullOrEmpty("255");
            var distance = data["distance"].Value;

            var rule = new Defrule(
                new[]
                {
                    "true",
                },
                new[]
                {
                    "up-reset-filters",
                });

            if (!string.IsNullOrEmpty(distance))
            {
                rule.Actions.Add(new Action($"up-filter-distance c: -1 c: {distance}"));
            }
            rule.Actions.Add(new Action($"up-find-{listName} c: {objectName} c: {amount}"));

            context.AddToScript(context.ApplyStacks(rule));
        }
    }
}
