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
            result.Should().Be("10/1/2017");
            // !!!: This is not a great test, as it is dependant on the current culture settings. Revisit this later.
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
