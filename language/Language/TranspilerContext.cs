using Language.ScriptItems;
using System.Collections.Generic;
using System.Linq;

namespace Language
{
    public class TranspilerContext
    {
        public Stack<Condition> ConditionStack { get; } = new Stack<Condition>();

        public Stack<Action> ActionStack { get; } = new Stack<Action>();

        public Stack<object> DataStack { get; } = new Stack<object>();

        public List<IScriptItem> Script { get; } = new List<IScriptItem>();

        public IScriptItem ApplyStacks(IScriptItem item)
        {
            if (item is Defrule)
            {
                ((Defrule)item).Conditions.AddRange(ConditionStack.Select(x => x.Copy()));
                ((Defrule)item).Actions.AddRange(ActionStack.Select(x => x.Copy()));
            }
            return item;
        }

        public void AddToScript(IEnumerable<IScriptItem> items)
        {
            foreach (var item in items)
            {
                AddToScript(item);
            }
        }

        public void AddToScript(IScriptItem item)
        {
            if (item is Defrule)
            {
                if (!((Defrule)item).IgnoreStacks)
                {
                    ApplyStacks(item);
                }
                Script.Add(item);
            }
        }

        public void OptimizeScript()
        {
            foreach (var item in Script)
            {
                item.Optimize();
            }

            Script.RemoveAll(x => x.MarkedForDeletion);

            var i = 0;
            while (i < Script.Count - 1)
            {
                var rule = Script[i] as Defrule;
                var otherRule = Script[i + 1] as Defrule;
                if (rule != null && otherRule != null)
                {
                    if (rule.CanMergeWith(otherRule))
                    {
                        rule.MergeIn(otherRule);
                        rule.Optimize();
                        Script.RemoveAt(i + 1);
                    }
                    else
                    {
                        i++;
                    }
                }
            }
        }
    }
}
