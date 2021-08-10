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
        public void ToString_Success()
        {
            _rule.Conditions.Add(new Condition("condition 1"));
            _rule.Conditions.Add(new Condition("condition 2"));
            _rule.Actions.Add(new Action("action 1"));
            _rule.Actions.Add(new Action("action 2"));
            _rule.Optimize();

            string expected = @"(defrule
    (condition 1)
    (condition 2)
=>
    (action 1)
    (action 2)
)";

            Assert.Equal(expected, _rule.ToString());
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
