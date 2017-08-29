using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Http.Results;
using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyDwellworks.Controllers.Api;
using MyDwellworks.Data.Core;
using MyDwellworks.Data.Core.Dtos;
using MyDwellworks.Data.Core.Models;
using MyDwellworks.Data.Core.Repositories;

namespace MyDwellworks.Tests.Controllers.Api
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
            _mockMapper.Setup(
                m => m.Map<IEnumerable<ApplicationUser>, IEnumerable<UserDto>>(It
                    .IsAny<List<ApplicationUser>>())).Returns(
                (IEnumerable<UserDto> userDtos) =>
                {
                    var dtos = new List<UserDto>
                    {
                        { new UserDto() {Email = "Test@test.com", Phone = "444-555-6666", UserName = "Test@test.com"} }
                    };
                    return dtos;
                });

            _mockMapper.Setup(
                m => m.Map<IEnumerable<ApplicationUser>, IEnumerable<UserDto>>(It
                    .IsAny<IEnumerable<ApplicationUser>>())).Returns(new List<UserDto>()
            {
                 new UserDto() {Email = "Test@test.com", Phone = "444-555-6666", UserName = "Test@test.com"} 
            });

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
    }
}
