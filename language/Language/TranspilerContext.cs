using Language.ScriptItems;
using System.Collections.Generic;
using System.Linq;

namespace Language
{
    public class TranspilerContext : ICopyable<TranspilerContext>
    {
        public Stack<Condition> ConditionStack { get; set; } = new Stack<Condition>();

        public Stack<Action> ActionStack { get; set; } = new Stack<Action>();

        public Stack<object> DataStack { get; set; } = new Stack<object>();

        public List<IScriptItem> Script { get; set; } = new List<IScriptItem>();

        public List<string> Goals { get; set; } = new List<string>();

        public List<string> Timers { get; set; } = new List<string>();

        public Dictionary<string, string> Subroutines { get; set; } = new Dictionary<string, string>();
        
        public string CurrentPath { get; set; }

        public string CurrentFileName { get; set; } = "unknown";

        public IEnumerable<IScriptItem> ApplyStacks(IEnumerable<IScriptItem> items)
        {
            foreach (var item in items)
            {
                yield return ApplyStacks(item);
            }
        }

        public IScriptItem ApplyStacks(IScriptItem item)
        {
            if (item is Defrule)
            {
                ((Defrule)item).Conditions.AddRange(ConditionStack.Select(x => x.Copy()));
                ((Defrule)item).Actions.AddRange(ActionStack.Select(x => x.Copy()));
            }
            return item;
        }

        public void AddToScriptWithJump(IEnumerable<IScriptItem> items, Condition skipCondition)
        {
            AddToScript(new Defrule(
                new[] { skipCondition },
                new[] { new Action($"up-jump-rule {items.Count(x => x is Defrule)}") })
            { Compressable = false, Splittable = false });

            foreach (var rule in items)
            {
                if (rule is Defrule)
                {
                    ((Defrule)rule).Compressable = false;
                    ((Defrule)rule).Splittable = false;
                }
            }

            AddToScript(items);
        }

        public int CreateGoal(string name = null)
        {
            Goals.Add(name);
            if (name != null)
            {
                Script.Insert(0, new Defconst(name, Goals.Count));
            }
            return Goals.Count;
        }

        public int CreateTimer(string name = null)
        {
            Timers.Add(name);
            if (name != null)
            {
                Script.Insert(0, new Defconst(name, Timers.Count));
            }
            return Timers.Count;
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
            if (item is Defconst)
            {
                Script.Insert(0, item);
            }
            else
            {
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
                else
                {
                    i++;
                }
            }
        }

        public TranspilerContext Copy()
        {
            var result = new TranspilerContext
            {
                ConditionStack = new Stack<Condition>(ConditionStack.Select(x => x.Copy())),
                ActionStack = new Stack<Action>(ActionStack.Select(x => x.Copy())),
                DataStack = new Stack<object>(DataStack),
                Script = new List<IScriptItem>(Script),
                Goals = new List<string>(Goals),
                Timers = new List<string>(Timers),
                Subroutines = new Dictionary<string, string>(Subroutines),
                CurrentPath = CurrentPath,
                CurrentFileName = CurrentFileName,
            };

            return result;
        }
    }
}
