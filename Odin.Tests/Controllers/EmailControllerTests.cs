using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Odin.Controllers;
using Odin.Data.Core;
using Odin.Data.Core.Repositories;
using System.Web.Mvc;

namespace Odin.Tests.Controllers
{
    [TestClass]
    public class EmailControllerTests
    {
        private EmailController _controller;
        private Mock<IMapper> _mockMapper;
        private Mock<IOrdersRepository> _mockRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new Mock<IOrdersRepository>();
            _mockMapper = new Mock<IMapper>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(u => u.Orders).Returns(_mockRepository.Object);           
            _controller = new EmailController(mockUnitOfWork.Object, _mockMapper.Object);
        }
        [TestMethod]
        public void Index_WhenCalled_Returns_Email_PartialView()
        {   
            //var result = _controller.Index("1") as ActionResult;
            //Assert.AreEqual("Email", result.ToString());
        }
    }
}
