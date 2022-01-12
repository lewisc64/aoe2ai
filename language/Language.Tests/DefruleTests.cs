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
        public void Split_Success()
        {
            _rule.Conditions.Add(new Condition("cond"));
            _rule.Actions.Add(new Action("a"));
            _rule.Actions.Add(new Action("b"));
            _rule.Actions.Add(new Action("c"));
            _rule.Optimize();

            var otherRule = _rule.Split();

            Assert.Equal("cond", otherRule.Conditions.Single().Text);

            Assert.Single(_rule.Actions);
            Assert.Equal("a", _rule.Actions.First().Text);

            Assert.Equal(2, otherRule.Actions.Count);
            Assert.Equal("b", otherRule.Actions.First().Text);
            Assert.Equal("c", otherRule.Actions.Last().Text);
        }
    }
}
