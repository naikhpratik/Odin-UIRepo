using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Odin.Helpers;
using Odin.Data.Core;
using AutoMapper;
using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;


namespace Odin.Tests.Helpers
{
    [TestClass]
    public class ItineraryHelperTests
    {
        private ItineraryHelper _itineraryHelper;
        private Mock<IServicesRepository> _mockServicesRepository;
        private Mock<IAppointmentsRepository> _mockAppointmentsRepository;

        [TestInitialize]
        public void TestInitialize()
        {            
           
        }
        [TestMethod]
        public void Build_WhenCalled_Should_NotReturn_Null()
        {
            var mockMapper = new Mock<IMapper>();
            _mockServicesRepository = new Mock<IServicesRepository>();
            _mockAppointmentsRepository = new Mock<IAppointmentsRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(u => u.Services).Returns(_mockServicesRepository.Object);
            mockUnitOfWork.SetupGet(u => u.Appointments).Returns(_mockAppointmentsRepository.Object);
            _itineraryHelper = new ItineraryHelper(mockUnitOfWork.Object, mockMapper.Object);
            string orderId = "1";
            var result = _itineraryHelper.Build(orderId);
            result.Should().NotBeNull();
        }
        [TestMethod]
        public void Build_WhenCalled_NoServices_Or_Appointments_Should_Return_Empty_Itinerary()
        {
            var mockMapper = new Mock<IMapper>();
            _mockServicesRepository = new Mock<IServicesRepository>();
            _mockAppointmentsRepository = new Mock<IAppointmentsRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(u => u.Services).Returns(_mockServicesRepository.Object);
            mockUnitOfWork.SetupGet(u => u.Appointments).Returns(_mockAppointmentsRepository.Object);
            _itineraryHelper = new ItineraryHelper(mockUnitOfWork.Object, mockMapper.Object);
            string orderId = null;
            var result = _itineraryHelper.Build(orderId);
            result.Itinerary.GetEnumerator().MoveNext().Should().BeFalse();
        }
    }
}
