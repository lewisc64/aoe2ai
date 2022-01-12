using Language.ScriptItems;
using Xunit;

namespace Language.Tests
{
    public class CombinatoryConditionFormatTests
    {
        private ICombinatoryConditionFormat _oneLine;
        private ICombinatoryConditionFormat _indented;

        public CombinatoryConditionFormatTests()
        {
            _oneLine = new OneLineCondition();
            _indented = new IndentedCondition();
        }

        [Fact]
        public void Indented_Format_Success()
        {
            var conditions = new Condition[]
            {
                new Condition("condition 1"),
                new Condition("condition 2"),
            };

            string expected = @"(op
  (condition 1)
  (condition 2)
)";

            Assert.Equal(expected, _indented.Format("op", conditions));
        }

        [Fact]
        public void OneLine_Format_Success()
        {
            var conditions = new Condition[]
            {
                new Condition("condition 1"),
                new Condition("condition 2"),
            };

            string expected = @"(op(condition 1)(condition 2))";

            Assert.Equal(expected, _oneLine.Format("op", conditions));
        }
    }
}
