using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNet.Identity;
using Moq;
using Odin.Data.Core;
using Odin.Data.Core.Dtos;
using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;
using Odin.Tests.Extensions;
using System;
using System.Web.Http;
using System.Web;
using Microsoft.Owin;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;

namespace Odin.Tests.Controllers.Api
{
    [TestClass]
    public class MessageControllerTests
    {
        private Odin.Controllers.Api.MessageController _controller;
        private Mock<IOrdersRepository> _mockRepository;
        private Mock<IConsultantsRepository> _mockConsultantsRepository;
        private Mock<ITransfereesRepository> _mockTransfereesRepository;
        private Mock<IMessagesRepository> _mockMessageRepository;
        private Mock<IHomeFindingPropertyRepository> _mockHFPRepository;
        private Mock<IUserNotificationRepository> _mockNotificationRepository;
        private Mock<IMapper> _mockMapper;
        private string _userId;
        private string _userName;


        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new Mock<IOrdersRepository>();
            _mockConsultantsRepository = new Mock<IConsultantsRepository>();
            _mockTransfereesRepository = new Mock<ITransfereesRepository>();
            _mockMessageRepository = new Mock<IMessagesRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockHFPRepository = new Mock<IHomeFindingPropertyRepository>();
            _mockNotificationRepository = new Mock<IUserNotificationRepository>();

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(u => u.Orders).Returns(_mockRepository.Object);
            mockUnitOfWork.SetupGet(c => c.Consultants).Returns(_mockConsultantsRepository.Object);
            mockUnitOfWork.SetupGet(t => t.Transferees).Returns(_mockTransfereesRepository.Object);
            mockUnitOfWork.SetupGet(p => p.HomeFindingProperties).Returns(_mockHFPRepository.Object);
            mockUnitOfWork.SetupGet(m => m.Messages).Returns(_mockMessageRepository.Object);
            mockUnitOfWork.SetupGet(n => n.UserNotifications).Returns(_mockNotificationRepository.Object);

            _userId = "1";
            _userName = "TestUser";

            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://tempuri.org", null), new HttpResponse(null));
            var owinContext = new Mock<IOwinContext>();
            _controller = new Odin.Controllers.Api.MessageController(mockUnitOfWork.Object, _mockMapper.Object);            
        }
        [TestMethod]
        public void markMessagesAsRead_ValidUserNotification_ReturnOkWithMessageIsRead_True()
        {
            _controller.MockCurrentUser(_userId, _userName);
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
        public void InsertMessage_ValidProperty_ShouldAddMessage_No_Notification_If_Not_EE_MAN_CON()
        {
            _controller.MockCurrentUser(_userId, _userName);
            var orderId = "1";
            Order order = new Order() { Id = orderId };
            var consultant = new Consultant() { SeContactUid = 1 };
            _mockRepository.Setup(o => o.GetOrderById(orderId)).Returns(order);
            var propId = "1";
            HomeFindingProperty prop = new HomeFindingProperty() { Id = propId };
            _mockHFPRepository.Setup(r => r.GetHomeFindingPropertyById(propId)).Returns(prop);
            Message mess = new Message() { HomeFindingPropertyId = propId };            
            MessageDto dto = new MessageDto() { HomeFindingPropertyId = propId, OrderId = orderId };
            
            var result = _controller.UpsertPropertyMessage(dto);
            var rl = _controller.User.IsInRole(UserRoles.Transferee);
            rl.Should().BeFalse();
            var rlC = _controller.User.IsInRole(UserRoles.Consultant);
            rlC.Should().BeFalse();
            var rlM = _controller.User.IsInRole(UserRoles.ProgramManager);
            rlM.Should().BeFalse();
            consultant.UserNotifications.Count().Should().Be(0);
            prop.Messages.Count.Should().Be(1);           
        }

        [TestMethod]
        public void InsertMessage_ValidProperty_ShouldAddMessageAnd_Consultant_Notification_If_Transferee()
        {
            _controller.MockCurrentUserAndRole(_userId, _userName, UserRoles.Transferee);

            var orderId = "1";
            Order order = new Order() { Id = orderId };
            var consultant = new Consultant() { SeContactUid = 1 };
            _mockConsultantsRepository.Setup(c => c.GetConsultantBySeContactUid(1)).Returns(consultant);
            _mockRepository.Setup(o => o.GetOrderById(orderId)).Returns(order);            
            order.Consultant = consultant;            

            var propId = "1";
            HomeFindingProperty prop = new HomeFindingProperty() { Id = propId };
            _mockHFPRepository.Setup(r => r.GetHomeFindingPropertyById(propId)).Returns(prop);
            Message mess = new Message() { HomeFindingPropertyId = propId };            
            MessageDto dto = new MessageDto(){ HomeFindingPropertyId = propId, OrderId = orderId };            

            var result = _controller.UpsertPropertyMessage(dto);
            prop.Messages.Count.Should().Be(1);

            var rl = _controller.User.IsInRole(UserRoles.Transferee);
            rl.Should().BeTrue();

            consultant.UserNotifications.Count().Should().Be(1);

            result.Should().BeOfType<System.Web.Http.Results.OkResult>();
        }

        [TestMethod]
        public void InsertMessage_ValidProperty_ShouldAddMessageAnd_2Notifications_If_Consultant()
        {
            _controller.MockCurrentUserAndRole("5", _userName, UserRoles.Consultant);

            var orderId = "1";
            Order order = new Order() { Id = orderId };
            var consultant = new Consultant() { SeContactUid = 1 };
            var tEmail = "odinTransferee@dwellworks.com";
            var transferee = new Transferee() { Id = "1", Email = tEmail };
            _mockConsultantsRepository.Setup(c => c.GetConsultantBySeContactUid(1)).Returns(consultant);
            _mockTransfereesRepository.Setup(t => t.GetTransfereeByEmail(tEmail)).Returns(transferee);
            _mockRepository.Setup(o => o.GetOrderById(orderId)).Returns(order);
            order.Consultant = consultant;
            order.Transferee = transferee;
            var propId = "1";
            HomeFindingProperty prop = new HomeFindingProperty() { Id = propId };
            _mockHFPRepository.Setup(r => r.GetHomeFindingPropertyById(propId)).Returns(prop);
            Message mess = new Message() { HomeFindingPropertyId = propId };
            MessageDto dto = new MessageDto() { HomeFindingPropertyId = propId, OrderId = orderId };

            var result = _controller.UpsertPropertyMessage(dto);
            prop.Messages.Count.Should().Be(1);

            var rl = _controller.User.IsInRole(UserRoles.Consultant);
            rl.Should().BeTrue();

            transferee.UserNotifications.Count().Should().Be(1);

            result.Should().BeOfType<System.Web.Http.Results.OkResult>();
        }

        [TestMethod]
        public void InsertMessage_ValidProperty_ShouldAddMessageAnd_2Notifications_If_Manager()
        {
            _controller.MockCurrentUserAndRole("9", _userName, UserRoles.ProgramManager);

            var orderId = "1";
            Order order = new Order() { Id = orderId };
            var consultant = new Consultant() { SeContactUid = 1 };
            var tEmail = "odinTransferee@dwellworks.com";
            var transferee = new Transferee() { Id = "1", Email= tEmail};
            _mockConsultantsRepository.Setup(c => c.GetConsultantBySeContactUid(1)).Returns(consultant);
            _mockTransfereesRepository.Setup(t => t.GetTransfereeByEmail(tEmail)).Returns(transferee);
            _mockRepository.Setup(o => o.GetOrderById(orderId)).Returns(order);
            order.Consultant = consultant;
            order.Transferee = transferee;
            var propId = "1";
            HomeFindingProperty prop = new HomeFindingProperty() { Id = propId };
            _mockHFPRepository.Setup(r => r.GetHomeFindingPropertyById(propId)).Returns(prop);
            Message mess = new Message() { HomeFindingPropertyId = propId };
            MessageDto dto = new MessageDto() { HomeFindingPropertyId = propId, OrderId = orderId };

            var result = _controller.UpsertPropertyMessage(dto);
            prop.Messages.Count.Should().Be(1);

            var rl = _controller.User.IsInRole(UserRoles.ProgramManager);
            rl.Should().BeTrue();

            consultant.UserNotifications.Count().Should().Be(1);

            transferee.UserNotifications.Count().Should().Be(1);

            result.Should().BeOfType<System.Web.Http.Results.OkResult>();
        }

        [TestMethod]
        public void InsertMessage_NoProperty_ShouldReturnNotFound()
        {
            _controller.MockCurrentUser(_userId, _userName);
            MessageDto dto = new MessageDto() { HomeFindingPropertyId = "-1", Id = null, MessageDate = DateTime.Now, MessageText = "Adding a new Message", OrderId="1" };
            var result = _controller.UpsertPropertyMessage(dto);            
            result.Should().BeOfType<System.Web.Http.Results.NotFoundResult>();
        }
    }
}
