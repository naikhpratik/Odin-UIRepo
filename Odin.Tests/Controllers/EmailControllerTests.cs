﻿using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Odin.Controllers;
using Odin.Data.Core;
using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;
using System.Web.Mvc;
using System.Net;
using Odin.ViewModels.Mailers;

namespace Odin.Tests.Controllers
{
    [TestClass]
    public class EmailControllerTests
    {
        private EmailController _controller;
        private Mock<IMapper> _mockMapper;
        private Mock<IOrdersRepository> _mockRepository;
        private Mock<ITransfereesRepository> _mockEeRepository;
        private Mock<IServicesRepository> _mockServicesRepository;
        private Mock<IAppointmentsRepository> _mockAppointmentsRepository;
        private Mock<IHomeFindingPropertyRepository> _mockHomeFindingPropertyRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new Mock<IOrdersRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockEeRepository = new Mock<ITransfereesRepository>();
            _mockServicesRepository = new Mock<IServicesRepository>();
            _mockAppointmentsRepository = new Mock<IAppointmentsRepository>();
            _mockHomeFindingPropertyRepository = new Mock<IHomeFindingPropertyRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(u => u.Orders).Returns(_mockRepository.Object);
            mockUnitOfWork.SetupGet(u => u.Transferees).Returns(_mockEeRepository.Object);
            mockUnitOfWork.SetupGet(u => u.Services).Returns(_mockServicesRepository.Object);
            mockUnitOfWork.SetupGet(u => u.Appointments).Returns(_mockAppointmentsRepository.Object);
            mockUnitOfWork.SetupGet(u => u.HomeFindingProperties).Returns(_mockHomeFindingPropertyRepository.Object);
            _controller = new EmailController(mockUnitOfWork.Object, _mockMapper.Object);
        }
        [TestMethod]
        public void Index_Get_WhenCalled_Returns_Email_PartialView()
        {
            var orderId = "1";
            Order order = new Order() { Id = orderId, ConsultantId = "1" };

            _mockRepository.Setup(r => r.GetOrderById(orderId)).Returns(order);

            var result = _controller.EmailForm(orderId);
            result.Should().NotBeNull();
        }
        [TestMethod]
        public void Index_Get_BadId_WhenCalled_Returns_NotFound()
        {
            var orderId = "1";
            var result = _controller.EmailForm(orderId);

            result.Should().BeOfType<HttpStatusCodeResult>();
            Assert.AreEqual(((HttpStatusCodeResult)result).StatusCode, (int)HttpStatusCode.NotFound);
        }
        [TestMethod]
        public void Index_Send_WhenCalled_Returns_Email_PartialView()
        {
            var orderId = "1";
            Order order = new Order() { Id = orderId, ConsultantId = "1" };
            _mockRepository.Setup(r => r.GetOrderById(orderId)).Returns(order);
            EmailViewModel vm = new EmailViewModel();
            vm.id = "1";
            vm.Email = "faque@email.com";
            var result = _controller.SendEmail(vm);
            result.Should().NotBeNull();
        }
        [TestMethod]
        public void Index_Send_No_Recipient_WhenCalled_Returns_Null()
        {
            var orderId = "1";
            Order order = new Order() { Id = orderId, ConsultantId = "1" };            
            _mockRepository.Setup(r => r.GetOrderById(orderId)).Returns(order);
            EmailViewModel vm = new EmailViewModel();
            vm.id = "1";
            vm.Email = null;
            var result = _controller.SendEmail(vm);
            result.Should().BeNull();
        }
        [TestMethod]
        public void Index_Send_BadId_WhenCalled_Returns_NotFound()
        {
            var orderId = "1";
            var result = _controller.EmailForm(orderId);

            result.Should().BeOfType<HttpStatusCodeResult>();
            Assert.AreEqual(((HttpStatusCodeResult)result).StatusCode, (int)HttpStatusCode.NotFound);
        }
    }
}
