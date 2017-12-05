using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Odin.Controllers;
using Odin.Data.Core;
using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;
using System.Web.Mvc;
using System.Net;

namespace Odin.Tests.Controllers
{
    [TestClass]
    public class EmailControllerTests
    {
        private EmailController _controller;
        private Mock<IMapper> _mockMapper;
        private Mock<IOrdersRepository> _mockRepository;
        private Mock<ITransfereesRepository> _mockEeRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new Mock<IOrdersRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockEeRepository = new Mock<ITransfereesRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(u => u.Orders).Returns(_mockRepository.Object);
            mockUnitOfWork.SetupGet(u => u.Transferees).Returns(_mockEeRepository.Object);
            _controller = new EmailController(mockUnitOfWork.Object, _mockMapper.Object);
        }
        [TestMethod]
        public void Index_WhenCalled_Returns_Email_PartialView()
        {
            var orderId = "1";
            Order order = new Order() { Id = orderId, ConsultantId = "1" };

            _mockRepository.Setup(r => r.GetOrderById(orderId)).Returns(order);

            var result = _controller.Index(orderId);
            result.Should().NotBeNull();
        }
        [TestMethod]
        public void Index_BadId_WhenCalled_Returns_NotFound()
        {
            var orderId = "1";
            var result = _controller.Index(orderId);

            result.Should().BeOfType<HttpStatusCodeResult>();
            Assert.AreEqual(((HttpStatusCodeResult)result).StatusCode, (int)HttpStatusCode.NotFound);
        }
    }
}
