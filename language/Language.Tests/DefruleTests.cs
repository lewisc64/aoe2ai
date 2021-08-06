using Language.ScriptItems;
using NUnit.Framework;
using System.Linq;

namespace Language.Tests
{
    [TestFixture]
    public static class DefruleTests
    {
        [Test]
        public static void Split_Success()
        {
            var rule = new Defrule(new[] { "cond" }, new[] { "a", "b", "c" });
            var otherRule = rule.Split();

            Assert.AreEqual("cond", otherRule.Conditions.Single().Text);

            Assert.AreEqual(1, rule.Actions.Count);
            Assert.AreEqual("a", rule.Actions.First().Text);

            Assert.AreEqual(2, otherRule.Actions.Count);
            Assert.AreEqual("b", otherRule.Actions.First().Text);
            Assert.AreEqual("c", otherRule.Actions.Last().Text);
        }
    }
}
