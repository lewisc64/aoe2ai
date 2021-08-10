using Language.ScriptItems;
using System.Linq;
using Xunit;

namespace Language.Tests
{
    public class DefconstTests
    {
        [Fact]
        public void ToString_Success()
        {
            var defconst = new Defconst("name", "5");
            Assert.Equal("(defconst name 5)", defconst.ToString());
        }
    }
}
