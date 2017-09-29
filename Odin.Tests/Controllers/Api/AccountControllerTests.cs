using System.Collections.Generic;
using System.Web.Http.Results;
using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Odin.Controllers.Api;
using Odin.Data.Core;
using Odin.Data.Core.Dtos;
using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;

namespace Odin.Tests.Controllers.Api
{
    [TestClass]
    public class AccountControllerTests
    {
        private AccountController _controller;
        private Mock<IUsersRepository> _mockRepository;
        private Mock<IMapper> _mockMapper;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new Mock<IUsersRepository>();
            _mockMapper = new Mock<IMapper>();

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(u => u.Users).Returns(_mockRepository.Object);

            _controller = new AccountController(_mockMapper.Object, mockUnitOfWork.Object);
        }

        [TestMethod]
        public void GetUsers_ValidRequest_ShouldReturnOkResult()
        {
            var appUser = new ApplicationUser {FirstName = "Test", LastName = "User", Email = "Test@test.com"};

            _mockRepository.Setup(r => r.GetUsersWithRole(UserRoles.Consultant)).Returns(new [] {appUser});

            var result = _controller.GetUsers();

            result.Should().BeOfType<OkNegotiatedContentResult<IEnumerable<UserDto>>>();
        }
        [TestMethod()]
        public void SendEmailConfirmationTokenAsyncTest()
        {
            Assert.Fail();
        }
    }
}
