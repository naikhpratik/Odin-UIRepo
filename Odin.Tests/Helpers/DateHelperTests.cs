using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Odin.Helpers;
using System;

namespace Odin.Tests.Helpers
{
    [TestClass]
    public class DateHelperTests
    {
        [TestMethod]
        public void GetViewFormat_DateWithValue_ReturnFormattedDate()
        {
            DateTime? date = Convert.ToDateTime("10/01/17");
            var result = DateHelper.GetViewFormat(date);
            result.Should().Be("01-Oct-2017");
        }

        [TestMethod]
        public void GetViewFormat_DateWithOutValue_ReturnEmptyString()
        {
            DateTime? date = null;
            var result = DateHelper.GetViewFormat(date);
            result.Should().Be(String.Empty);
        }

    }
}
