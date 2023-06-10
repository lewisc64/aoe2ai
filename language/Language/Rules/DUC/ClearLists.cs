using Language.ScriptItems;

namespace Language.Rules.DUC
{
    [ActiveRule]
    public class ClearLists : RuleBase
    {
        public override string Name => "DUC clear lists";

        public override string Help => "TODO";

        public override string Usage => "TODO";

        public ClearLists()
            : base(@"^\$clear(?: (?<list>local|remote))?$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var listName = data["list"].Value;

            var clearLocal = string.IsNullOrEmpty(listName) || listName == "local";
            var clearRemote = string.IsNullOrEmpty(listName) || listName == "remote";

            var rule = new Defrule(
                new[]
                {
                    "true",
                },
                new[]
                {
                    $"up-reset-search {(clearLocal ? 1 : 0)} {(clearLocal ? 1 : 0)} {(clearRemote ? 1 : 0)} {(clearRemote ? 1 : 0)}",
                });

            context.AddToScript(context.ApplyStacks(rule));
        }
    }
}
