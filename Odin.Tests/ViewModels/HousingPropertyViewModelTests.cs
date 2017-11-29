using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Odin.ViewModels.Orders.Transferee;
using FluentAssertions;

namespace Odin.Tests.ViewModels
{
    [TestClass]
    public class HousingPropertyViewModelTests
    {
        [TestMethod]
        public void NewHousingPropertyViewModelHasEmptyPhotos()
        {
            HousingPropertyViewModel viewModel = new HousingPropertyViewModel();

            viewModel.UploadedPhotos.Should().NotBeNull();
            viewModel.UploadedPhotos.Should().BeEmpty();
        }
    }
}
