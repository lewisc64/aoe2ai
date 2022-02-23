using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules
{
    [ActiveRule]
    public class TargetPlayer : RuleBase
    {
        private static readonly Dictionary<string, string> FindTypes = new Dictionary<string, string>
        {
            { "winning", null },
            { "closest", "closest" },
            { "attacking", "attacker" },
        };

        public override string Name => "target player";

        public override string Help => "Sets sn-target-player-number and sn-focus-player-number.";

        public override string Usage => "target winning/closest/attacking enemy/ally";

        public TargetPlayer()
            : base(@"^target (?<findtype>winning|closest|attacking|random) (?<playertype>enemy|ally)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var findType = data["findtype"].Value;
            var playerType = data["playertype"].Value;

            var rule = new Defrule();

            if (findType != "winning")
            {
                context.UsingVolatileGoal(goal =>
                {
                    rule.Actions.Add(new Action($"up-find-player {playerType} find-{FindTypes[findType]} {goal}"));
                    rule.Actions.Add(new Action($"up-modify-sn sn-target-player-number g:= {goal}"));
                });
            }
            else
            {
                rule.Actions.Add(new Action("set-strategic-number sn-target-player-number 0"));
            }

            context.AddToScript(context.ApplyStacks(rule));
        }
    }
}
