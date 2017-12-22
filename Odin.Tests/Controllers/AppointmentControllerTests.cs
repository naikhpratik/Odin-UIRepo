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

namespace Odin.Tests.Controllers
{
    [TestClass]
    public class AppointmentControllerTests
    {
        private AppointmentController _controller;
        private Mock<IMapper> _mockMapper;
        private Mock<IOrdersRepository> _mockRepository;
        private Mock<ITransfereesRepository> _mockEeRepository;
        private Mock<IServicesRepository> _mockServicesRepository;
        private Mock<IAppointmentsRepository> _mockAppointmentsRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new Mock<IOrdersRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockEeRepository = new Mock<ITransfereesRepository>();
            _mockServicesRepository = new Mock<IServicesRepository>();
            _mockAppointmentsRepository = new Mock<IAppointmentsRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(u => u.Orders).Returns(_mockRepository.Object);
            mockUnitOfWork.SetupGet(u => u.Transferees).Returns(_mockEeRepository.Object);
            mockUnitOfWork.SetupGet(u => u.Services).Returns(_mockServicesRepository.Object);
            mockUnitOfWork.SetupGet(u => u.Appointments).Returns(_mockAppointmentsRepository.Object);
            _controller = new AppointmentController(mockUnitOfWork.Object, _mockMapper.Object);
        }
        [TestMethod]
        public void AppointmentPartial_Get_WhenCalled_Returns_Appointment_PartialView()
        {
            var appointmentId = "1";
            Appointment appointment = new Appointment() { Id = appointmentId};
            _mockAppointmentsRepository.Setup(r => r.GetAppointmentById(appointmentId)).Returns(appointment);
            var result = _controller.AppointmentPartial(appointmentId);
            result.Should().NotBeNull();
        }
        [TestMethod]
        public void AppointmentPartial_Get_BadId_WhenCalled_Returns_NotFound()
        {
            var appointmentId = "1";
            Appointment appointment = new Appointment() { Id = appointmentId };
            var result = _controller.AppointmentPartial(appointmentId);
            Assert.AreEqual(((HttpStatusCodeResult)result).StatusCode, (int)HttpStatusCode.NotFound);
        }        
    }
}
