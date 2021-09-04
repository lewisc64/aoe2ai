using Language.ScriptItems;
using Xunit;

namespace Language.Tests
{
    public class ConditionParsingTests
    {
        [Theory]
        [InlineData("a", "(a)")]
        [InlineData("a or b", "(or (a) (b))")]
        [InlineData("a and b", "(and (a) (b))")]
        [InlineData("not a", "(not (a))")]
        [InlineData("a or b and c or d", "(or (a) (or (and (b) (c)) (d)))")]
        [InlineData("(a or b) and (c or d)", "(and (or (a) (b)) (or (c) (d)))")]
        [InlineData("(a or b)", "(or (a) (b))")]
        [InlineData("(((a)))", "(a)")]
        [InlineData("((a) or (((b))))", "(or (a) (b))")]
        [InlineData("not a or b", "(or (not (a)) (b))")]
        [InlineData("goal gl-or 1 or goal gl-and 1", "(or (goal gl-or 1) (goal gl-and 1))")]
        public void Parse_Success(string text, string expected)
        {
            Assert.Equal(expected, Condition.Parse(text).ToString());
        }

        [Theory]
        [InlineData("(a)", "a")]
        [InlineData("(((a)))", "a")]
        [InlineData("(a) or (a)", "(a) or (a)")]
        [InlineData("((a) or (a))", "(a) or (a)")]
        [InlineData("(((a) or a) or ((a)))", "((a) or a) or ((a))")]
        public void Debracket_Success(string text, string expected)
        {
            Assert.Equal(expected, Condition.DebracketExpression(text).ToString());
        }
    }
}
