using Language.ScriptItems;
using Xunit;

namespace Language.Tests
{
    public class DefconstTests
    {
        [Fact]
        public void ToString_Success()
        {
            var defconstInt = new Defconst<int>("number", 5);
            Assert.Equal("(defconst number 5)", defconstInt.ToString());

            var defconstString = new Defconst<string>("string", "test");
            Assert.Equal("(defconst string \"test\")", defconstString.ToString());
        }
    }
}
