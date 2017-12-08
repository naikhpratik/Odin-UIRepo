using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Odin.Controllers.Api;
using Odin.Data.Core;
using Odin.Data.Core.Dtos;
using Odin.Data.Core.Repositories;
using Odin.Interfaces;
using Odin.Tests.Extensions;
using System.Web.Http;

namespace Odin.Tests.Controllers.Api
{
    [TestClass]
    public class BookMarkletControllerTests
    {
        private BookMarkletController _controller;
        private Mock<IOrdersRepository> _mockRepository;
        private Mock<IMapper> _mockMapper;
        private Mock<IBookMarkletHelper> _mockBookMarkletHelper;
        private Mock<IQueueStore> _mockQueueStore;
        private string _userId;
        private string _userName;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new Mock<IOrdersRepository>();
            _mockMapper = new Mock<IMapper>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            _mockQueueStore = new Mock<IQueueStore>();
            mockUnitOfWork.SetupGet(u => u.Orders).Returns(_mockRepository.Object);
            _mockBookMarkletHelper = new Mock<IBookMarkletHelper>();
            _controller = new BookMarkletController(mockUnitOfWork.Object, _mockMapper.Object, _mockQueueStore.Object, _mockBookMarkletHelper.Object);

            _userId = "1";
            _userName = "TestUser";
            _controller.MockCurrentUser(_userId,_userName);
        }

        [TestMethod]
        public void Add_ValidDto_ShouldReturnDefaultView()
        {
            var dto = new BookMarkletDto()
            {
                PropertyUrl = "http://test.com",
                OrderId = "1"
            };

            var result = _controller.Add(dto) as IHttpActionResult;
            result.Should().BeOfType<System.Web.Http.Results.OkResult>();
        }

        [TestMethod]
        public void Add_BadDto_ShouldReturnErrorView()
        {
            var dto = new BookMarkletDto()
            {
                OrderId = "1"
            };

            var result = _controller.Add(dto) as IHttpActionResult;
            result.Should().BeOfType<System.Web.Http.Results.BadRequestResult>();
        }
    }
}
