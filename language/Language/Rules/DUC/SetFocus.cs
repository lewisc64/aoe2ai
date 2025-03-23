using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules.DUC
{
    [ActiveRule]
    public class SetFocus : RuleBase
    {
        private static readonly Dictionary<string, string> FindTypes = new()
        {
            { "closest", "closest" },
            { "attacking", "attacker" },
            { "random", "random" },
        };

        public override string Name => "DUC set focus player";

        public override string Help => "TODO";

        public override string Usage => "TODO";

        public SetFocus()
            : base(@"^\$focus (?:(?<findtype>closest|attacking|random) (?<playertype>enemy|ally)|(?<myself>myself)|(?<gaia>gaia)|(?<target>target))$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);

            var rule = new Defrule();

            if (data["target"].Success)
            {
                rule.Actions.Add(new Action($"up-modify-sn sn-focus-player-number s:= sn-target-player-number"));
            }
            else if (data["gaia"].Success)
            {
                rule.Actions.Add(new Action($"up-modify-sn sn-focus-player-number c:= {Game.GaiaPlayerNumber}"));
            }
            else if (data["myself"].Success)
            {
                context.UsingVolatileGoal(goal =>
                {
                    rule.Actions.Add(new Action($"up-get-fact player-number 0 {goal}"));
                    rule.Actions.Add(new Action($"up-modify-sn sn-focus-player-number g:= {goal}"));
                });
            }
            else
            {
                var findType = data["findtype"].Value;
                var playerType = data["playertype"].Value;

                context.UsingVolatileGoal(goal =>
                {
                    rule.Actions.Add(new Action($"up-find-player {playerType} find-{FindTypes[findType]} {goal}"));
                    rule.Actions.Add(new Action($"up-modify-sn sn-focus-player-number g:= {goal}"));
                });
            }



            context.AddToScript(context.ApplyStacks(rule));
        }
    }
}
