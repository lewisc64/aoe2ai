using Language.ScriptItems;

namespace Language.Rules.DUC
{
    [ActiveRule]
    public class DucObjectsAction : RuleBase
    {
        public override string Name => "DUC objects action";

        public override string Help => "TODO";

        public override string Usage => "TODO";

        public DucObjectsAction()
            : base(@"^\$(?<action>move|attack|attack ground|patrol|garrison|gather)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var action = data["action"].Value;

            var rule = new Defrule();

            if (action == "move")
            {
                rule.Actions.Add(new Action("up-target-objects 0 action-move -1 -1"));
            }
            else if (action == "patrol")
            {
                rule.Actions.Add(new Action("up-target-objects 0 action-patrol -1 -1"));
            }
            else if (action == "attack")
            {
                rule.Actions.Add(new Action("up-target-objects 0 action-default -1 -1"));
            }
            else if (action == "attack ground")
            {
                rule.Actions.Add(new Action("up-target-objects 0 action-ground -1 -1"));
            }
            else if (action == "garrison")
            {
                rule.Actions.Add(new Action("up-target-objects 0 action-garrison -1 -1"));
            }
            else if (action == "gather")
            {
                rule.Actions.Add(new Action("up-target-objects 0 action-gather -1 -1"));
            }

            context.AddToScript(context.ApplyStacks(rule));
        }
    }
}
