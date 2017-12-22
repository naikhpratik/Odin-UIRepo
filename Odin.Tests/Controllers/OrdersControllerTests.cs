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
using Odin.ViewModels.Orders.Transferee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

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
        private Mock<IHomeFindingPropertyRepository> _mockHFPropertyRepository;

        private Mock<IMapper> _mockMapper;
        private IMapper mapper;

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
            _mockHFPropertyRepository = new Mock<IHomeFindingPropertyRepository>();
            _mockMapper = new Mock<IMapper>();

            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));
            mapper = config.CreateMapper();

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
            mockUnitOfWork.SetupGet(u => u.HomeFindingProperties).Returns(_mockHFPropertyRepository.Object);

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
        
        [TestMethod]
        public void Properties_NoPropertiesWithAnyOptionExist_Count_ShouldBeZero()
        {
            var orderId = "1";
            var userId = "1";
            var order = new Order() { Id = orderId, ConsultantId = userId };
            _mockRepository.Setup(r => r.GetOrderFor(orderId, userId)).Returns(order);
            order.HomeFinding = new HomeFinding() { Id = orderId, Deleted = false };
            
            HousingViewModel viewModel = new HousingViewModel(order, mapper, "AllViewings");
            viewModel.Properties.Count().Should().Be(0);
        }
        [TestMethod]
        public void Properties_NoPropertiesWithGivenOptionExist_Count_ShouldBeZero()
        {
            var orderId = "1";
            var userId = "1";
            var order = new Order() { Id = orderId, ConsultantId = userId};
            _mockRepository.Setup(r => r.GetOrderFor(orderId, userId)).Returns(order);
            order.HomeFinding = new HomeFinding() { Id = orderId, Deleted = false };
            order.HomeFinding.HomeFindingProperties.Add(new HomeFindingProperty() { Id = "1", ViewingDate = DateTime.Now });
            HousingViewModel viewModel = new HousingViewModel(order, mapper, "NoViewings");
            viewModel.Properties.Count().Should().Be(0);
        }
        [TestMethod]
        public void Properties_TwoPropertiesWithGivenOptionExists_Count_ShouldBe2()
        {
            var orderId = "1";
            var userId = "1";
            var order = new Order() { Id = orderId, ConsultantId = userId };
            _mockRepository.Setup(r => r.GetOrderFor(orderId, userId)).Returns(order);
            order.HomeFinding = new HomeFinding() { Id = orderId, Deleted = false };
            
            HomeFindingProperty p1 = new HomeFindingProperty();
            p1.Deleted = false;
            p1.Property = new Property();
            p1.ViewingDate = DateTime.Now.AddDays(10);
            order.HomeFinding.HomeFindingProperties.Add(p1);

            HomeFindingProperty p2 = new HomeFindingProperty();
            p2.Deleted = false;
            p2.Property = new Property();
            p2.ViewingDate = DateTime.Now.AddDays(20);
            order.HomeFinding.HomeFindingProperties.Add(p2);

            HousingViewModel viewModel = new HousingViewModel(order, mapper, "ViewingsOnly");
            //var result = _controller.PropertiesPartialPDF(orderId, "ViewingsOnly");
            viewModel.Properties.Count().Should().Be(2);
        }
        [TestMethod]
        public void Properties_TwoPropertiesWithOneViewingOptionExists_Count_ShouldBe1()
        {
            var orderId = "1";
            var userId = "1";
            var order = new Order() { Id = orderId, ConsultantId = userId };
            _mockRepository.Setup(r => r.GetOrderFor(orderId, userId)).Returns(order);
            order.HomeFinding = new HomeFinding() { Id = orderId, Deleted = false };

            HomeFindingProperty p1 = new HomeFindingProperty();
            p1.Deleted = false;
            p1.Property = new Property();
            p1.ViewingDate = DateTime.Now.AddDays(10);
            order.HomeFinding.HomeFindingProperties.Add(p1);

            HomeFindingProperty p2 = new HomeFindingProperty();
            p2.Deleted = false;
            p2.Property = new Property();
            order.HomeFinding.HomeFindingProperties.Add(p2);

            HousingViewModel viewModel = new HousingViewModel(order, mapper, "ViewingsOnly");
            //var result = _controller.PropertiesPartialPDF(orderId, "ViewingsOnly");
            viewModel.Properties.Count().Should().Be(1);
        }

        [TestMethod]
        public void DashboardViewModel_SelectedServices_ShouldHaveServices()
        {
            var vm = new DashboardViewModel();

            var service = new Service()
            {
                ServiceType = new ServiceType(){Category = ServiceCategory.AreaOrientation,ActionLabel = "Area Orientation"},
                Selected = true
            };

            var otherService = new Service()
            {
                ServiceType = new ServiceType() { Category = ServiceCategory.AreaOrientation, ActionLabel = "Area Orientation 2" },
                Selected = true
            };

            var compService = new Service()
            {
                ServiceType = new ServiceType() { Category = ServiceCategory.SettlingIn, ActionLabel = "Settling in" },
                Selected = true,
                CompletedDate = DateTime.Now
            };

            var notService = new Service()
            {
                ServiceType = new ServiceType() { Category = ServiceCategory.SettlingIn, ActionLabel = "Settling in" },
                Selected = false,
                CompletedDate = DateTime.Now
            };

            vm.Services = new List<Service>(){service,otherService,compService,notService};

            vm.CompletedServiceCount.Should().Be(1);
            vm.TotalServiceCount.Should().Be(3);
            vm.PercentComplete.Should().Be(33);
            vm.ServiceCategories.Count().Should().Be(2);
        }
    }
}