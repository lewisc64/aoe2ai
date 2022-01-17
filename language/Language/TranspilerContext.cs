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

        public Script Script { get; set; } = new Script();

        public List<string> Goals { get; set; } = new List<string>();

        public List<string> Timers { get; set; } = new List<string>();

        public List<string> Constants { get; set; } = new List<string>();

        public Dictionary<string, string> Templates { get; set; } = new Dictionary<string, string>();

        public string CurrentPath { get; set; }

        public string RootPath { get; set; }

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
                Script.Items.Insert(0, CreateConstant(name, Goals.Count));
            }
            return Goals.Count;
        }

        public int CreateTimer(string name = null)
        {
            Timers.Add(name);
            if (name != null)
            {
                Script.Items.Insert(0, CreateConstant(name, Timers.Count));
            }
            return Timers.Count;
        }

        public Defconst<T> CreateConstant<T>(string name, T value)
        {
            if (Constants.Contains(name))
            {
                throw new System.InvalidOperationException($"Constant '{name}' has already been defined.");
            }
            Constants.Add(name);
            return new Defconst<T>(name, value);
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
            if (item.GetType().IsGenericType && item.GetType().GetGenericTypeDefinition() == typeof(Defconst<>))
            {
                Script.Items.Insert(0, item);
            }
            else
            {
                Script.Items.Add(item);
            }
        }

        public void OptimizeScript()
        {
            var i = 0;
            while (i < Script.Items.Count)
            {
                var rule = Script.Items[i] as Defrule;
                if (rule != null)
                {
                    if (rule.IsTooLong && rule.Splittable)
                    {
                        Script.Items.Insert(i + 1, rule.Split());
                    }
                    else
                    {
                        rule.Optimize();
                        i++;
                    }
                }
                else
                {
                    i++;
                }
            }

            Script.Items.RemoveAll(x => x.MarkedForDeletion);

            i = 0;
            while (i < Script.Items.Count - 1)
            {
                var rule = Script.Items[i] as Defrule;
                var otherRule = Script.Items[i + 1] as Defrule;
                if (rule != null && otherRule != null)
                {
                    if (rule.CanMergeWith(otherRule))
                    {
                        rule.MergeIn(otherRule);
                        rule.Optimize();
                        Script.Items.RemoveAt(i + 1);
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
                Script = Script.Copy(),
                Goals = new List<string>(Goals),
                Timers = new List<string>(Timers),
                Constants = new List<string>(Constants),
                Templates = new Dictionary<string, string>(Templates),
                RootPath = RootPath,
                CurrentPath = CurrentPath,
                CurrentFileName = CurrentFileName,
            };

            return result;
        }
    }
}
