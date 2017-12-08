using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Odin.Data.Core;
using Odin.Data.Core.Dtos;
using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;
using Odin.Interfaces;
using Odin.Tests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Odin.Tests.Controllers.Api
{
    [TestClass]
    public class UserNotificationControllerTests
    {
        private Odin.Controllers.Api.UserNotificationController _controller;
        private Mock<IUserNotificationRepository> _mockRepository;
        private Mock<IMapper> _mockMapper;
        private string _userId;
        private string _userName;

        public UserNotificationControllerTests()
        {
            


        }
        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new Mock<IUserNotificationRepository>();
            _mockMapper = new Mock<IMapper>();

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(n => n.UserNotifications).Returns(_mockRepository.Object);
                        

            var mockQueueStore = new Mock<IQueueStore>();
            var mockAccountHelper = new Mock<IAccountHelper>();
            _controller = new Odin.Controllers.Api.UserNotificationController(mockUnitOfWork.Object, _mockMapper.Object, mockAccountHelper.Object);

            _userId = "1";
            _userName = "TestUser";
            _controller.MockCurrentUser(_userId, _userName);
        }

        [TestMethod]
        public void NotificationMarkAsRead_ValidUserNotification_ReturnOkWithuserNotificationId()
        {
            var unId = "1";

            UserNotification userNotification = new UserNotification() { Id = unId };
            _mockRepository.Setup(r => r.GetUserNotificationByNotificationId(_userId, unId)).Returns(userNotification);

            var result = _controller.NotificationMarkAsRead(unId) as IHttpActionResult;
            result.Should().BeOfType<System.Web.Http.Results.OkNegotiatedContentResult<string>>();
            userNotification.IsRead.Should().Be(true);
            ((OkNegotiatedContentResult<string>)result).Content.Should().NotBeNullOrEmpty();

        }

        [TestMethod]
        public void NotificationMarkAsRemoved_ValidUserNotification_ReturnOkWithuserNotificationId()
        {
            var unId = "1";

            UserNotification userNotification = new UserNotification() { Id = unId };
            _mockRepository.Setup(r => r.GetUserNotificationByNotificationId(_userId, unId)).Returns(userNotification);

            var result = _controller.NotificationMarkAsRemoved(unId) as IHttpActionResult;
            result.Should().BeOfType<System.Web.Http.Results.OkNegotiatedContentResult<string>>();
            userNotification.IsRemoved.Should().Be(true);
            ((OkNegotiatedContentResult<string>)result).Content.Should().NotBeNullOrEmpty();

        }
    }
}
