﻿using Language.ScriptItems;
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

        public Dictionary<int, string> Goals { get; set; } = new Dictionary<int, string>();

        public Dictionary<string, int> DucPointGoalNameMap { get; set; } = new Dictionary<string, int>();

        public Stack<int> VolatileGoalNumbers { get; set; } = new Stack<int>();

        public Stack<int> VolatilePointGoalNumbers { get; set; } = new Stack<int>();

        public List<string> Timers { get; set; } = new List<string>();

        public List<string> Constants { get; set; } = new List<string>();

        public Dictionary<string, string> Templates { get; set; } = new Dictionary<string, string>();

        public string CurrentPath { get; set; }

        public string RootPath { get; set; }

        public string CurrentFileName { get; set; } = "unknown";

        public IEnumerable<IScriptItem> ApplyStacks(IEnumerable<IScriptItem> items, bool ignoreActionStack = false)
        {
            foreach (var item in items)
            {
                yield return ApplyStacks(item, ignoreActionStack: ignoreActionStack);
            }
        }

        public IScriptItem ApplyStacks(IScriptItem item, bool ignoreActionStack = false)
        {
            if (item is Defrule defrule)
            {
                defrule.Conditions.AddRange(ConditionStack.Select(x => x.Copy()));
                if (!ignoreActionStack)
                {
                    defrule.Actions.AddRange(ActionStack.Select(x => x.Copy()));   
                }
            }
            return item;
        }

        public void AddToScriptWithJump(IEnumerable<IScriptItem> items, Condition skipCondition)
        {
            AddToScript(new Defrule(
                new[] { skipCondition },
                new[] { new Action($"up-jump-rule {items.Count(x => x is Defrule)}") })
            { Compressable = false, Splittable = false });

            foreach (var item in items)
            {
                if (item is Defrule defrule)
                {
                    defrule.Compressable = false;
                    defrule.Splittable = false;
                }
            }

            AddToScript(items);
        }

        public int CreateConsecutiveGoals(int amount, int startId = 1)
        {
            for (int i = startId; i < Game.MaxGoals - amount; i++)
            {
                var range = Enumerable.Range(i, amount);
                if (range.All(x => !Goals.ContainsKey(x)))
                {
                    foreach (var goal in range)
                    {
                        Goals[goal] = null;
                    }
                    return i;
                }
            }
            throw new System.InvalidOperationException($"Unable to allocate {amount} unused consecutive goals.");
        }

        public int CreateGoal(string name = null, int startId = 1)
        {
            for (var i = startId; i <= Game.MaxGoals; i++)
            {
                if (!Goals.ContainsKey(i))
                {
                    Goals[i] = name;
                    if (name != null)
                    {
                        Script.Items.Insert(0, CreateConstant(name, i));
                    }
                    return i;
                }
            }
            throw new System.InvalidOperationException("There are not enough free goals.");
        }

        public int CreatePointGoal(string name = null)
        {
            var goalNumber = CreateConsecutiveGoals(2, startId: 41);
            if (name != null)
            {
                Script.Items.Insert(0, CreateConstant(name, goalNumber));
            }
            return goalNumber;
        }

        public int CreateDucPointGoal(string name)
        {
            var goalNumber = CreatePointGoal();
            DucPointGoalNameMap[name] = goalNumber;
            return goalNumber;
        }

        public int GetDucPointGoalNumber(string name)
        {
            if (!DucPointGoalNameMap.ContainsKey(name))
            {
                throw new System.ArgumentException($"DUC point {name} is not defined.");
            }
            return DucPointGoalNameMap[name];
        }

        public int CreateVolatileGoal()
        {
            return VolatileGoalNumbers.Any() ? VolatileGoalNumbers.Pop() : CreateGoal();
        }

        public void FreeVolatileGoal(int goalNumber)
        {
            VolatileGoalNumbers.Push(goalNumber);
        }

        public void FreeVolatileGoals(IEnumerable<int> goalNumbers)
        {
            foreach (var goalNumber in goalNumbers)
            {
                FreeVolatileGoal(goalNumber);
            }
        }

        public void UsingVolatileGoal(System.Action<int> callback)
        {
            int goal = CreateVolatileGoal();
            try
            {
                callback(goal);
            }
            finally
            {
                FreeVolatileGoal(goal);
            }
        }

        public int CreateVolatilePointGoal()
        {
            return VolatilePointGoalNumbers.Any() ? VolatilePointGoalNumbers.Pop() : CreatePointGoal();
        }

        public void FreeVolatilePointGoal(int goalNumberX)
        {
            VolatilePointGoalNumbers.Push(goalNumberX);
        }

        public void FreeVolatilePointGoals(IEnumerable<int> goalNumbers)
        {
            foreach (var goalNumberX in goalNumbers)
            {
                FreeVolatilePointGoal(goalNumberX);
            }
        }

        public void UsingVolatilePointGoal(System.Action<int> callback)
        {
            int goal = CreateVolatilePointGoal();
            try
            {
                callback(goal);
            }
            finally
            {
                FreeVolatilePointGoal(goal);
            }
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
                if (Script.Items[i] is Defrule rule)
                {
                    if (rule.IsTooLong && rule.Splittable)
                    {
                        if (rule.Conditions.Count > rule.Actions.Count)
                        {
                            Script.Items.RemoveAt(i);
                            Script.Items.InsertRange(i, rule.SplitByConditions(CreateGoal()));
                        }
                        else
                        {
                            Script.Items.RemoveAt(i);
                            Script.Items.InsertRange(i, rule.SplitByActions());
                        }
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
                if (Script.Items[i] is Defrule rule && Script.Items[i + 1] is Defrule otherRule)
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

        public void UsingSubcontext(System.Action<TranspilerContext> callback)
        {
            var subcontext = Copy();

            subcontext.ActionStack.Clear();
            subcontext.ConditionStack.Clear();
            subcontext.DataStack.Clear();
            subcontext.Script.Items.Clear();

            callback(subcontext);

            Constants = subcontext.Constants;
            Goals = subcontext.Goals;
            DucPointGoalNameMap = subcontext.DucPointGoalNameMap;
            VolatileGoalNumbers = subcontext.VolatileGoalNumbers;
            VolatilePointGoalNumbers = subcontext.VolatilePointGoalNumbers;
            Timers = subcontext.Timers;
            Templates = subcontext.Templates;
        }

        public TranspilerContext Copy()
        {
            return new TranspilerContext
            {
                ConditionStack = new Stack<Condition>(ConditionStack.Select(x => x.Copy())),
                ActionStack = new Stack<Action>(ActionStack.Select(x => x.Copy())),
                DataStack = new Stack<object>(DataStack),
                Script = Script.Copy(),
                Goals = new Dictionary<int, string>(Goals),
                DucPointGoalNameMap = new Dictionary<string, int>(DucPointGoalNameMap),
                VolatileGoalNumbers = new Stack<int>(VolatileGoalNumbers),
                VolatilePointGoalNumbers = new Stack<int>(VolatilePointGoalNumbers),
                Timers = new List<string>(Timers),
                Constants = new List<string>(Constants),
                Templates = new Dictionary<string, string>(Templates),
                RootPath = RootPath,
                CurrentPath = CurrentPath,
                CurrentFileName = CurrentFileName,
            };
        }
    }
}
