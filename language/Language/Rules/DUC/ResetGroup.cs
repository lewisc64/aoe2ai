using Language.ScriptItems;

namespace Language.Rules.DUC
{
    [ActiveRule]
    public class ResetGroup : RuleBase
    {
        public override string Name => "DUC reset group";

        public override string Help => "TODO";

        public override string Usage => "TODO";

        public ResetGroup()
            : base(@"^\$reset group (?<group>[^ ]+)$")
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
                });

            context.AddToScript(context.ApplyStacks(rule));
        }
    }
}
