using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Odin.Controllers;

using Odin.Data.Core;
using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;
using Odin.Interfaces;
using Odin.Tests.Extensions;
using System.Net;
using System.Web.Mvc;
using Odin.ViewModels.Orders.Transferee;

namespace Odin.Tests.Controllers
{
    [TestClass]
    public class OrdersControllerTests
    {
        private OrdersController _controller;
        private string _userId;
        private Mock<IOrdersRepository> _mockRepository;
        private Mock<IServicesRepository> _mockServicesRepository;
        private Mock<IAppointmentsRepository> _mockAppointmentsRepository;
        private Mock<IServiceTypesRepository> _mockServiceTypesRepository;
        private Mock<INumberOfBathroomsTypesRepository> _mockNumberOfBathroomsTypesRepository;
        private Mock<IHousingTypesRepository> _mockHousingTypesRepository;
        private Mock<IAreaTypesRepository> _mockAreaTypesRepository;
        private Mock<ITransportationTypesRepository> _mockTransportationTypesRepository;
        private Mock<IDepositTypesRepository> _mockDepositTypesRepository;
        private Mock<IBrokerFeeTypesRepository> _mockBrokerFeeTypessRepository;
        private Mock<ITransfereesRepository> _mockTransfereesRepository;

        private Mock<IMapper> _mockMapper;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new Mock<IOrdersRepository>();
            _mockServicesRepository = new Mock<IServicesRepository>();
            _mockAppointmentsRepository = new Mock<IAppointmentsRepository>();
            _mockServiceTypesRepository = new Mock<IServiceTypesRepository>();
            _mockNumberOfBathroomsTypesRepository = new Mock<INumberOfBathroomsTypesRepository>();
            _mockHousingTypesRepository = new Mock<IHousingTypesRepository>();
            _mockAreaTypesRepository = new Mock<IAreaTypesRepository>();
            _mockTransportationTypesRepository = new Mock<ITransportationTypesRepository>();
            _mockDepositTypesRepository = new Mock<IDepositTypesRepository>();
            _mockBrokerFeeTypessRepository = new Mock<IBrokerFeeTypesRepository>();
            _mockTransfereesRepository = new Mock<ITransfereesRepository>();

            _mockMapper = new Mock<IMapper>();

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(u => u.Orders).Returns(_mockRepository.Object);
            mockUnitOfWork.SetupGet(u => u.Services).Returns(_mockServicesRepository.Object);
            mockUnitOfWork.SetupGet(u => u.Appointments).Returns(_mockAppointmentsRepository.Object);
            mockUnitOfWork.SetupGet(u => u.ServiceTypes).Returns(_mockServiceTypesRepository.Object);
            mockUnitOfWork.SetupGet(u => u.NumberOfBathrooms).Returns(_mockNumberOfBathroomsTypesRepository.Object);
            mockUnitOfWork.SetupGet(u => u.HousingTypes).Returns(_mockHousingTypesRepository.Object);
            mockUnitOfWork.SetupGet(u => u.AreaTypes).Returns(_mockAreaTypesRepository.Object);
            mockUnitOfWork.SetupGet(u => u.TransportationTypes).Returns(_mockTransportationTypesRepository.Object);
            mockUnitOfWork.SetupGet(u => u.DepositTypes).Returns(_mockDepositTypesRepository.Object);
            mockUnitOfWork.SetupGet(u => u.BrokerFeeTypes).Returns(_mockBrokerFeeTypessRepository.Object);
            mockUnitOfWork.SetupGet(u => u.Transferees).Returns(_mockTransfereesRepository.Object);

            var mockEmailHelper = new Mock<IEmailHelper>();
            var mockAccountHelper = new Mock<IAccountHelper>();
            _controller = new OrdersController(mockUnitOfWork.Object, _mockMapper.Object, mockAccountHelper.Object);
            _userId = "1";
            _controller.MockControllerContextForUser(_userId);

        }

        [TestMethod]
        public void Index_WhenCalled_ReturnsOk()
        {
            var result = _controller.Index() as ViewResult;

            result.Should().NotBeNull();
        }

        [TestMethod]
        public void Details_NoOrderWithGivenIdExists_ShouldReturnNotFound()
        {
            var orderId = "1";
            var result = _controller.DetailsPartial(orderId);

            result.Should().BeOfType<HttpStatusCodeResult>();
            Assert.AreEqual(((HttpStatusCodeResult)result).StatusCode, (int)HttpStatusCode.NotFound);
        }

        [TestMethod]
        public void Details_OrderConsultantIdIsNotCurrentUser_ShouldReturnUnauthorized()
        {
            var orderId = "1";

            Order order = new Order() { Id = orderId, ConsultantId = "2" };

            _mockRepository.Setup(r => r.GetOrderById(orderId)).Returns(order);

            var result = _controller.DetailsPartial(orderId);

            result.Should().BeOfType<HttpStatusCodeResult>();
            Assert.AreEqual(((HttpStatusCodeResult)result).StatusCode, (int)HttpStatusCode.Unauthorized);
        }

        [TestMethod]
        public void Details_ValidOrder_ShouldReturnOk()
        {
            var orderId = "1";
            var userId = "1";

            Order order = new Order() { Id = orderId, ConsultantId = userId };

            _mockRepository.Setup(r => r.GetOrderById(orderId)).Returns(order);
            OrdersTransfereeViewModel vm = new OrdersTransfereeViewModel();

            _mockMapper.Setup(o => o.Map<Order, OrdersTransfereeViewModel>(It.IsAny<Order>())).Returns(vm);

            var result = _controller.DetailsPartial(orderId) as PartialViewResult;
            result.Should().NotBeNull();
        }
        [TestMethod]
        public void Itinerary_ValidOrder_ShouldReturnOk()
        {
            var orderId = "1";

            Order order = new Order() { Id = orderId, ConsultantId = "1" };

            _mockRepository.Setup(r => r.GetOrderById(orderId)).Returns(order);

            var result = _controller.ItineraryPartial(orderId);
            result.Should().NotBeNull();
        }
        [TestMethod]
        public void Itinerary_NoTransfereeWithGivenIdExists_ShouldReturnNotFound()
        {
            var orderId = "1";
            var result = _controller.ItineraryPartial(orderId);

            result.Should().BeOfType<HttpStatusCodeResult>();
            Assert.AreEqual(((HttpStatusCodeResult)result).StatusCode, (int)HttpStatusCode.NotFound);
        }

        [TestMethod]
        public void HistoryPartial_InvalidOrderId_ShouldReturnOk()
        {
            //Arrange 
            var orderId = "1";
            Order order = new Order() { Id = orderId, ConsultantId = "1" };
            _mockRepository.Setup(r => r.GetOrderById(orderId)).Returns(order);


            //Action 
            var result = _controller.HistoryPartial(orderId);
            
            
            //Assert 
            result.Should().NotBeNull();
        }
    }
}