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
            var value = data["value"].Value;

            if (value.StartsWith("\"") && value.EndsWith("\""))
            {
                context.AddToScript(context.CreateConstant(name, value.Substring(1, value.Length - 2).Replace("\\\"", "\"").Replace("\\\\", "\\")));
            }
            else
            {
                context.AddToScript(context.CreateConstant(name, int.Parse(value)));
            }
        }
    }
}
