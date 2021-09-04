using Language.ScriptItems;

namespace Language.Rules
{
    [ActiveRule]
    public class SetConst : RuleBase
    {
        public override string Name => "set const";

        public override string Help => "Sets a constant. Can only be done once for every name.";

        public override string Usage => "const CONST_NAME = VALUE";

        public SetConst()
            : base(@"^const (?<name>[^ ]+) ?= ?(?<value>.+)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var name = data["name"].Value;
            var value = int.Parse(data["value"].Value);

            context.AddToScript(new Defconst<int>(name, value));
        }
    }
}
