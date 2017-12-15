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
using Odin.ViewModels.Shared;

namespace Odin.Tests.Controllers
{
    [TestClass]
    public class MessageControllerTests
    {
        private MessageController _controller;
        private Mock<IMapper> _mockMapper;
        private Mock<IMessagesRepository> _mockRepository;
        private Mock<IHomeFindingPropertyRepository> _mockHFPRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new Mock<IMessagesRepository>();
            _mockHFPRepository = new Mock<IHomeFindingPropertyRepository>();
            _mockMapper = new Mock<IMapper>();           
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(u => u.Messages).Returns(_mockRepository.Object);
            mockUnitOfWork.SetupGet(u => u.HomeFindingProperties).Returns(_mockHFPRepository.Object);          
            _controller = new MessageController(mockUnitOfWork.Object, _mockMapper.Object);
        }
        [TestMethod]
        public void MessagePartial_Get_WhenCalled_Returns_Messages_PartialView()
        {
            var propertyId = "1";
            PropertyMessagesViewModel mess = new PropertyMessagesViewModel();
            mess.Id = propertyId;
            _mockRepository.Setup(r => r.GetMessagesByPropertyId(propertyId)).Returns(mess.messages);
            var result = _controller.MessagePartial(propertyId);
            result.Should().NotBeNull();
        }
        [TestMethod]
        public void AppointmentPartial_Get_BadId_WhenCalled_Returns_NotFound()
        {
            var propertyId = "1";
            PropertyMessagesViewModel mess = new PropertyMessagesViewModel();
            mess.Id = propertyId;
            var result = _controller.MessagePartial(propertyId);
            Assert.AreEqual(((HttpStatusCodeResult)result).StatusCode, (int)HttpStatusCode.NotFound);
        }
    }
}
