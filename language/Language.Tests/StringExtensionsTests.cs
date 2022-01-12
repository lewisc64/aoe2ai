using Language.Extensions;
using System;
using Xunit;

namespace Language.Tests
{
    public class StringExtensionsTests
    {
        public StringExtensionsTests()
        {
        }

        [Theory]
        [InlineData("", "REPLACEMENT")]
        [InlineData(null, "REPLACEMENT")]
        [InlineData(" ", " ")]
        [InlineData("important", "important")]
        public void ReplaceNullIfEmpty_Success(string text, string expected)
        {
            Assert.Equal(expected, text.ReplaceIfNullOrEmpty("REPLACEMENT"));
        }

        [Fact]
        public void Wrap_Success()
        {
            var text = "chunk chunk chunk chunk chunk";
            var wrapped = text.Wrap(text.Length / 2);

            Assert.Equal($"chunk chunk{Environment.NewLine}chunk chunk{Environment.NewLine}chunk", wrapped);
        }

        [Fact]
        public void Wrap_NoSplitQuotes()
        {
            var text = "chunk \"chunk chunk\" chunk chunk";
            var wrapped = text.Wrap(text.Length / 2);

            Assert.Equal($"chunk{Environment.NewLine}\"chunk chunk\"{Environment.NewLine}chunk chunk", wrapped);
        }
    }
}
