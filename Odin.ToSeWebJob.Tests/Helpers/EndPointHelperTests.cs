using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Odin.Data.Builders;
using Odin.Data.Core.Models;
using Odin.ToSeWebJob.Helpers;

namespace Odin.ToSeWebJob.Tests.Helpers
{
    [TestClass]
    public class EndPointHelperTests
    {
        private Service _service;

        [TestInitialize]
        public void TestInitialize()
        {
            _service = new Service();
            _service.ServiceType = new ServiceType();
            _service.Order = OrderBuilder.New(1).FirstOrDefault(); 
        }

        [TestMethod]
        public void GetEndPointForService_CategoryIsInitialConsultation_ShouldReturnFirstContact()
        {
            _service.ServiceType.Category = ServiceCategory.InitialConsultation;

            var endpoint = EndPointHelper.GetEndPointForService(_service);

            endpoint.Should().Be(EndPointHelper.FirstContactEndPoint);
        }

        [TestMethod]
        public void GetEndPointForService_CategoryIsWelcomePacket_SHouldReturnFirstContact()
        {
            _service.ServiceType.Category = ServiceCategory.WelcomePacket;

            var endpoint = EndPointHelper.GetEndPointForService(_service);

            endpoint.Should().Be(EndPointHelper.FirstContactEndPoint);
        }

        [TestMethod]
        public void GetEndPointForService_ProgramNameContainsBundledAndCategoryIsSettlingIn_ShouldReturnDestinationChecklist()
        {
            _service.ServiceType.Category = ServiceCategory.SettlingIn;
            _service.Order.ProgramName = "Test Bundled Program";

            var endpoint = EndPointHelper.GetEndPointForService(_service);

            endpoint.Should().Be(EndPointHelper.DestinationChecklistEndPoint);
        }

        [TestMethod]
        public void GetEndPointForService_ProgramNameDoesNotContainBundledAndCategoryIsSettlingIn_ShouldReturnSettlingIn()
        {
            _service.ServiceType.Category = ServiceCategory.SettlingIn;

            var endpoint = EndPointHelper.GetEndPointForService(_service);

            endpoint.Should().Be(EndPointHelper.SettlingInEndpoint);
        }
    }

}
