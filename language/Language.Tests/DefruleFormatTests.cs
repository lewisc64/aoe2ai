using Language.ScriptItems;
using Language.ScriptItems.Formats;
using Xunit;

namespace Language.Tests
{
    public class DefruleFormatTests
    {
        private IDefruleFormat _oneLine;
        private IDefruleFormat _indented;

        public DefruleFormatTests()
        {
            _oneLine = new OneLineDefrule();
            _indented = new IndentedDefrule();
        }

        [Fact]
        public void Indented_Format_Success()
        {
            var conditions = new[]
            {
                new Condition("condition 1"),
                new Condition("condition 2"),
            };

            var actions = new[]
            {
                new Action("action 1"),
                new Action("action 2"),
            };

            string expected = "(defrule\n    (condition 1)\n    (condition 2)\n=>\n    (action 1)\n    (action 2)\n)";

            Assert.Equal(expected, _indented.Format(new Defrule(conditions, actions), new OneLineCondition()));
        }

        [Fact]
        public void OneLine_Format_Success()
        {
            var conditions = new[]
            {
                new Condition("condition 1"),
                new Condition("condition 2"),
            };

            var actions = new[]
            {
                new Action("action 1"),
                new Action("action 2"),
            };

            string expected = @"(defrule(condition 1)(condition 2)=>(action 1)(action 2))";

            Assert.Equal(expected, _oneLine.Format(new Defrule(conditions, actions), new OneLineCondition()));
        }
    }
}
