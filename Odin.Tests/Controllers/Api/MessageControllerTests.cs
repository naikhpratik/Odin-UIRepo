using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owen;
using Moq;
using Odin.Data.Core;
using Odin.Data.Core.Dtos;
using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;
using Odin.Tests.Extensions;
using System;
using System.Web.Http;
using System.Web;
using Microsoft.Owin;;


namespace Odin.Tests.Controllers.Api
{
    [TestClass]
    public class MessageControllerTests
    {
        private Odin.Controllers.Api.MessageController _controller;
        private Mock<IMessagesRepository> _mockMessageRepository;
        private Mock<IHomeFindingPropertyRepository> _mockHFPRepository;
        private Mock<IMapper> _mockMapper;
        private string _userId;
        private string _userName;


        [TestInitialize]
        public void TestInitialize()
        {
            _mockMessageRepository = new Mock<IMessagesRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockHFPRepository = new Mock<IHomeFindingPropertyRepository>();

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(p => p.HomeFindingProperties).Returns(_mockHFPRepository.Object);
            mockUnitOfWork.SetupGet(m => m.Messages).Returns(_mockMessageRepository.Object);

            _userId = "1";
            _userName = "TestUser";

            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://tempuri.org", null), new HttpResponse(null));
            var owinContext = new Mock<IOwinContext>();
            _controller = new Odin.Controllers.Api.MessageController(mockUnitOfWork.Object, _mockMapper.Object);
            _controller.MockCurrentUser(_userId, _userName);
            //_controller.Request.SetOwinContext(owinContext.Object);
        }
        [TestMethod]
        public void markMessagesAsRead_ValidUserNotification_ReturnOkWithMessageIsRead_True()
        {
            var propId = "1";
            HomeFindingProperty prop = new HomeFindingProperty() { Id = propId };
            _mockHFPRepository.Setup(r => r.GetHomeFindingPropertyById(propId)).Returns(prop);

            Message mess = new Message() { HomeFindingPropertyId = propId };            
            prop.Messages.Add(mess);

            _mockMessageRepository.Setup(r => r.GetMessagesByPropertyId(propId)).Returns(prop.Messages);

            var result = _controller.markMessageRead(propId) as IHttpActionResult;
            result.Should().BeOfType<System.Web.Http.Results.OkResult>();
            mess.IsRead.Should().Be(true);    
        }
        [TestMethod]
        public void InsertMessage_ValidProperty_ShouldAddMessage()
        {
            var propId = "1";
            HomeFindingProperty prop = new HomeFindingProperty() { Id = propId };
            _mockHFPRepository.Setup(r => r.GetHomeFindingPropertyById(propId)).Returns(prop);

            Message mess = new Message() { HomeFindingPropertyId = propId };
            prop.Messages.Add(mess);

            _mockMessageRepository.Setup(r => r.GetMessagesByPropertyId(propId)).Returns(prop.Messages);
            MessageDto dto = new MessageDto() { HomeFindingPropertyId = prop.Id, Id = null, MessageDate = DateTime.Now, MessageText = "Adding a new Message", Deleted = false };

            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new ApplicationUserManager(userStore.Object);

            var result = _controller.UpsertPropertyMessage(dto);

            prop.Messages.Count.Should().Be(1);
        }

        [TestMethod]
        public void InsertMessage_NoProperty_ShouldReturnNotFound()
        {  
            MessageDto dto = new MessageDto() { HomeFindingPropertyId = "-1", Id = null, MessageDate = DateTime.Now, MessageText = "Adding a new Message" };
            var result = _controller.UpsertPropertyMessage(dto);            
            result.Should().BeOfType<System.Web.Http.Results.NotFoundResult>();
        }
    }
}
