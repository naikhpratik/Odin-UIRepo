using System;
using System.Web.Http.Results;
using System.Web.Mvc;
using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Odin.Controllers;
using Odin.Data.Builders;
using Odin.Data.Core;
using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;
using Odin.Tests.Extensions;
using Odin.ViewModels;
using Odin.Interfaces;

namespace Odin.Tests.Controllers
{
    [TestClass]
    public class OrdersControllerTests
    {
        private OrdersController _controller;
        private string _userId;
        private Mock<IOrdersRepository> _mockRepository;
        private Mock<IMapper> _mockMapper;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new Mock<IOrdersRepository>();
            _mockMapper = new Mock<IMapper>();

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(u => u.Orders).Returns(_mockRepository.Object);
            var mockEmailHelper = new Mock<IEmailHelper>();
            _controller = new OrdersController(mockUnitOfWork.Object, _mockMapper.Object);
            _userId = "1";
            _controller.MockControllerContextForUser(_userId);
        }

        [TestMethod]
        public void Index_WhenCalled_ReturnsOk()
        {
            var result = _controller.Index() as ViewResult;

            result.Should().NotBeNull();
        }
    }
}
