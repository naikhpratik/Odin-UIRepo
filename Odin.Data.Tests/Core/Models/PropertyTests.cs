using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Odin.Data.Core.Models;
using FluentAssertions;

namespace Odin.Data.Tests.Core.Models
{
    [TestClass]
    public class PropertyTests
    {
        [TestMethod]
        public void NewProperty_HasNonNullEmptyPhotosCollection()
        {
            Property property = new Property();

            property.Photos.Should().NotBeNull();
            property.Photos.Should().BeEmpty();
        }
    }
}
