using Language.Extensions;
using Language.ScriptItems.Formats;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Language.ScriptItems
{
    public class Defrule : IScriptItem
    {
        public static int MaxRuleSize = 32;

        public string Id { get; } = Guid.NewGuid().ToString();

        public bool Compressable { get; set; } = true;

        public bool Splittable { get; set; } = true;

        public bool MarkedForDeletion { get; set; } = false;

        public List<Condition> Conditions { get; }

        public List<Action> Actions { get; }

        public int Length
        {
            get
            {
                return LengthOfConditions + LengthOfActions;
            }
        }

        public int LengthOfConditions
        {
            get
            {
                return Conditions.Select(x => x.Length).Sum();
            }
        }

        public int LengthOfActions
        {
            get
            {
                return Actions.Select(x => x.Length).Sum();
            }
        }

        public bool IsTooLong => Length > MaxRuleSize;

        public Defrule()
        {
            Conditions = new List<Condition>() { new Condition("true") };
            Actions = new List<Action>() { new Action("do-nothing") };
        }

        public Defrule(IEnumerable<Condition> conditions, IEnumerable<Action> actions)
        {
            Conditions = conditions.ToList();
            Actions = actions.ToList();
        }

        public Defrule(IEnumerable<string> conditions, IEnumerable<string> actions)
        {
            Conditions = conditions.Select(x => new Condition(x)).ToList();
            Actions = actions.Select(x => new Action(x)).ToList();
        }

        public override string ToString()
        {
            return new OneLineDefrule().Format(this, new OneLineCondition());
        }

        public void Optimize()
        {
            var i = 0;
            while (i < Conditions.Count)
            {
                if (Conditions[i] is CombinatoryCondition combCondition && combCondition.Text == "and")
                {
                    Conditions.RemoveAt(i);
                    Conditions.InsertRange(i, combCondition.Conditions);
                }
                else
                {
                    i++;
                }
            }
            if (Conditions.Count > 1)
            {
                Conditions.RemoveAll(x => x.Text == "true");
                if (!Conditions.Any())
                {
                    Conditions.Add(new Condition("true"));
                }
            }
            if (Actions.Count > 1)
            {
                Actions.RemoveAll(x => x.Text == "do-nothing");
                if (!Actions.Any())
                {
                    Actions.Add(new Action("do-nothing"));
                }
            }
            while (Actions.Count(x => x.Text == "disable-self") >= 2)
            {
                Actions.Remove(Actions.First(x => x.Text == "disable-self"));
            }
            if (IsTooLong)
            {
                throw new InvalidOperationException($"Rule is overlength. Length: {Length}, Maximum length: {MaxRuleSize}.");
            }
        }

        public IEnumerable<Defrule> SplitByActions()
        {
            if (!Splittable)
            {
                throw new InvalidOperationException("Rule is not splittable.");
            }

            var filteredActions = Actions
                .Where(x => x.Text != "disable-self" && x.Text != "do-nothing")
                .ToList();

            if (filteredActions.Count <= 1)
            {
                throw new InvalidOperationException("Not enough actions to split.");
            }

            var rule1 = new Defrule(Conditions.Select(x => x.Copy()), filteredActions.GetRange(0, filteredActions.Count / 2))
            {
                Compressable = false,
            };
            var rule2 = new Defrule(Conditions.Select(x => x.Copy()), filteredActions.GetRange(filteredActions.Count / 2, filteredActions.Count - filteredActions.Count / 2))
            {
                Compressable = false,
            };

            if (Actions.Any(x => x.Text == "disable-self"))
            {
                rule1.Actions.Add(new Action("disable-self"));
                rule2.Actions.Add(new Action("disable-self"));
            }

            return new[] { rule1, rule2 };
        }

        public IEnumerable<Defrule> SplitByConditions(int goalNumber)
        {
            if (!Splittable)
            {
                throw new InvalidOperationException("Rule is not splittable.");
            }

            var filteredConditions = Conditions
                .Where(x => x.Text != "true" && x.Text != "false")
                .ToList();

            if (filteredConditions.Count <= 1)
            {
                throw new InvalidOperationException("Not enough top-level conditions to split.");
            }

            var firstHalf = filteredConditions.GetRange(0, filteredConditions.Count / 2 + 1);
            var secondHalf = filteredConditions.GetRange(filteredConditions.Count / 2 + 1, filteredConditions.Count - filteredConditions.Count / 2 - 1);

            return new[]
            {
                new Defrule(new[] { "true" }, new[] { $"set-goal {goalNumber} 0" })
                {
                    Compressable = false,
                    Splittable = false,
                },
                new Defrule(firstHalf.Select(x => x.Copy()), new[] { new Action($"set-goal {goalNumber} 1") })
                {
                    Compressable = false,
                    Splittable = false,
                },
                new Defrule(new[] { new Condition($"goal {goalNumber} 1") }.Concat(secondHalf), Actions.Select(x => x.Copy()))
                {
                    Compressable = false,
                    Splittable = false,
                },
            };
        }

        public void MergeIn(Defrule rule)
        {
            if (!Compressable)
            {
                throw new ArgumentException("This rule is not compressable.");
            }
            if (!rule.Compressable)
            {
                throw new ArgumentException("The rule being merged in is not compressable.");
            }
            if (string.Join("", Conditions) != string.Join("", rule.Conditions))
            {
                throw new ArgumentException("The provided rule has different conditions, unable to merge.");
            }
            if (Length + rule.LengthOfActions > MaxRuleSize)
            {
                throw new ArgumentException("The provided rule has too many actions to be merged.");
            }
            if (Actions.Any(x => x.Text == "disable-self") != rule.Actions.Any(x => x.Text == "disable-self"))
            {
                throw new ArgumentException("Rules with the action 'disable-self' cannot be merged.");
            }
            Actions.AddRange(rule.Actions);
            rule.Actions.Clear();
            rule.Actions.Add(new Action("do-nothing"));
        }

        public bool CanMergeWith(Defrule rule)
        {
            return string.Join("", Conditions) == string.Join("", rule.Conditions)
                && Length + rule.LengthOfActions <= MaxRuleSize
                && Actions.Any(x => x.Text == "disable-self") == rule.Actions.Any(x => x.Text == "disable-self")
                && Compressable
                && rule.Compressable;
        }
    }
}
