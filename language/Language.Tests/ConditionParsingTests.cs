using Language.ScriptItems;
using NUnit.Framework;

namespace Language.Tests
{
    [TestFixture]
    public class ConditionParsingTests
    {
        [TestCase("a", "(a)")]
        [TestCase("a or b", "(or (a) (b))")]
        [TestCase("a and b", "(and (a) (b))")]
        [TestCase("not a", "(not (a))")]
        [TestCase("a or b and c or d", "(or (a) (or (and (b) (c)) (d)))")]
        [TestCase("(a or b) and (c or d)", "(and (or (a) (b)) (or (c) (d)))")]
        [TestCase("(a or b)", "(or (a) (b))")]
        [TestCase("(((a)))", "(a)")]
        [TestCase("((a) or (((b))))", "(or (a) (b))")]
        [TestCase("not a or b", "(or (not (a)) (b))")]
        [TestCase("goal gl-or 1 or goal gl-and 1", "(or (goal gl-or 1) (goal gl-and 1))")]
        public void Parse_Success(string text, string expected)
        {
            Assert.AreEqual(expected, Condition.Parse(text).ToString());
        }
    }
}
