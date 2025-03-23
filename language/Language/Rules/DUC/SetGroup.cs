using Language.ScriptItems;

namespace Language.Rules.DUC
{
    [ActiveRule]
    public class SetGroup : RuleBase
    {
        public override string Name => "DUC set group";

        public override string Help => "TODO";

        public override string Usage => "TODO";

        public SetGroup()
            : base(@"^\$set group (?<group>[^ ]+) to local list$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var group = data["group"].Value;

            var rule = new Defrule(
                new[]
                {
                    "true",
                },
                new[]
                {
                    $"up-modify-group-flag 0 c: {group}",
                    $"up-reset-group c: {group}",
                    $"up-create-group 0 0 c: {group}",
                    $"up-modify-group-flag 1 c: {group}",
                });

            context.AddToScript(context.ApplyStacks(rule));
        }
    }
}
