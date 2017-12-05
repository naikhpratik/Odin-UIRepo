using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Odin.Data.Extensions;

namespace Odin.Data.Tests.Extensions
{
    [TestClass]
    public class StringExtensionsTests
    { 
        [TestMethod]
        public void NullContains_StringIsNull_ShouldReturnFalse()
        {
            string str = null;

            var result = str.NullContains("test");

            result.Should().BeFalse();
        }

        [TestMethod]
        public void NullContains_ContainsSubString_ShouldReturnTrue()
        {
            string str = "This is a test";

            var result = str.NullContains("is a");

            result.Should().BeTrue();
        }

        [TestMethod]
        public void NullContains_DoesNotContainSubString_ShouldReturnFalse()
        {
            string str = "This is a test";

            var result = str.NullContains("not");

            result.Should().BeFalse();
        }
    }
}
