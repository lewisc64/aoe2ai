using Language.ScriptItems;
using System.Linq;
using Xunit;

namespace Language.Tests
{
    public class DefruleTests
    {
        private Defrule _rule;

        public DefruleTests()
        {
            _rule = new Defrule();
        }

        [Fact]
        public void Optimize_DefaultRule_NoChange()
        {
            Assert.Single(_rule.Conditions);
            Assert.Equal("true", _rule.Conditions.Single().Text);

            Assert.Single(_rule.Actions);
            Assert.Equal("do-nothing", _rule.Actions.Single().Text);

            _rule.Optimize();

            Assert.Single(_rule.Conditions);
            Assert.Equal("true", _rule.Conditions.Single().Text);

            Assert.Single(_rule.Actions);
            Assert.Equal("do-nothing", _rule.Actions.Single().Text);
        }

        [Fact]
        public void Optimize_WithOthers_RemoveTrueAndDoNothing()
        {
            _rule.Conditions.Add(new Condition("condition"));
            _rule.Actions.Add(new Action("action"));
            _rule.Optimize();

            Assert.Single(_rule.Conditions);
            Assert.Equal("condition", _rule.Conditions.Single().Text);

            Assert.Single(_rule.Actions);
            Assert.Equal("action", _rule.Actions.Single().Text);
        }

        [Fact]
        public void SplitByActions_Success()
        {
            _rule.Conditions.Add(new Condition("cond"));
            _rule.Actions.Add(new Action("a"));
            _rule.Actions.Add(new Action("b"));
            _rule.Actions.Add(new Action("c"));
            _rule.Optimize();

            var rules = _rule.SplitByActions().ToArray();

            Assert.Equal("cond", rules[1].Conditions.Single().Text);

            Assert.Single(rules[0].Actions);
            Assert.Equal("a", rules[0].Actions.First().Text);

            Assert.Equal(2, rules[1].Actions.Count);
            Assert.Equal("b", rules[1].Actions.First().Text);
            Assert.Equal("c", rules[1].Actions.Last().Text);
        }

        [Fact]
        public void SplitByConditions_Success()
        {
            _rule.Conditions.Add(new Condition("a"));
            _rule.Conditions.Add(new Condition("b"));
            _rule.Conditions.Add(new Condition("c"));
            _rule.Actions.Add(new Action("action"));
            _rule.Optimize();

            var rules = _rule.SplitByConditions(1).ToArray();

            Assert.Single(rules[0].Conditions);
            Assert.Single(rules[0].Actions);
            Assert.Equal("true", rules[0].Conditions.Single().Text);
            Assert.Equal("set-goal 1 0", rules[0].Actions.Single().Text);

            Assert.Equal(2, rules[1].Conditions.Count);
            Assert.Single(rules[1].Actions);
            Assert.Equal("a", rules[1].Conditions.First().Text);
            Assert.Equal("b", rules[1].Conditions.Last().Text);
            Assert.Equal("set-goal 1 1", rules[1].Actions.Single().Text);

            Assert.Equal(2, rules[2].Conditions.Count);
            Assert.Single(rules[2].Actions);
            Assert.Equal("goal 1 1", rules[2].Conditions.First().Text);
            Assert.Equal("c", rules[2].Conditions.Last().Text);
            Assert.Equal("action", rules[2].Actions.Single().Text);
        }
    }
}
