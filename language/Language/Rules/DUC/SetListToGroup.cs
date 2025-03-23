using Language.ScriptItems;

namespace Language.Rules.DUC
{
    [ActiveRule]
    public class SetListToGroup : RuleBase
    {
        public override string Name => "DUC set list to group";

        public override string Help => "TODO";

        public override string Usage => "TODO";

        public SetListToGroup()
            : base(@"^\$set (?<list>local|remote) list to group (?<group>[^ ]+)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var group = data["group"].Value;
            var list = data["list"].Value;

            var rule = new Defrule(
                new[]
                {
                    "true",
                },
                new[]
                {
                    $"up-set-group search-{list} c: {group}",
                });

            context.AddToScript(context.ApplyStacks(rule));
        }
    }
}
