using Language.ScriptItems;
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
            var conditions = new Condition[]
            {
                new Condition("condition 1"),
                new Condition("condition 2"),
            };

            var actions = new Action[]
            {
                new Action("action 1"),
                new Action("action 2"),
            };

            string expected = @"(defrule
    (condition 1)
    (condition 2)
=>
    (action 1)
    (action 2)
)";

            Assert.Equal(expected, _indented.Format(conditions, actions));
        }

        [Fact]
        public void OneLine_Format_Success()
        {
            var conditions = new Condition[]
            {
                new Condition("condition 1"),
                new Condition("condition 2"),
            };

            var actions = new Action[]
            {
                new Action("action 1"),
                new Action("action 2"),
            };

            string expected = @"(defrule(condition 1)(condition 2)=>(action 1)(action 2))";

            Assert.Equal(expected, _oneLine.Format(conditions, actions));
        }
    }
}
