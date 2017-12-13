using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Odin.Data.Core.Models;
using FluentAssertions;

namespace Odin.Data.Tests.Core.Models
{
    [TestClass]
    public class HomeFindingPropertyTests
    {
        [TestMethod]
        public void NewHomeFindingPropertyLikedIsNull()
        {
            HomeFindingProperty hfp = new HomeFindingProperty();
            hfp.Liked.Should().BeNull();
        }
    }
}