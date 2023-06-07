using Language.ScriptItems;

namespace Language.Rules.DUC
{
    [ActiveRule]
    public class OrderList : RuleBase
    {
        public override string Name => "DUC order list";

        public override string Help => "TODO";

        public override string Usage => "TODO";

        public OrderList()
            : base(@"^\$order (?<list>local|remote) list by (?<objectdata>[^ ]+)(?: (?<direction>asc(?:ending)?|desc(?:ending)?))?$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var list = data["list"].Value;
            var objectData = data["objectdata"].Value;
            var isAscending = string.IsNullOrEmpty(data["direction"].Value) ? true : data["direction"].Value.StartsWith("asc");

            var rule = new Defrule(
                new[]
                {
                    "true",
                },
                new[]
                {
                    $"up-clean-search search-{list} {objectData} {(isAscending ? 1 : 2)}",
                });

            context.AddToScript(context.ApplyStacks(rule));
        }
    }
}
