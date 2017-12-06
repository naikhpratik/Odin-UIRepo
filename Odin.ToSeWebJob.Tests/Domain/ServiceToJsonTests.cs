using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Odin.Data.Builders;
using Odin.Data.Core.Models;
using Odin.ToSeWebJob.Domain;
using ServicEngineImporter.Models;

namespace Odin.ToSeWebJob.Tests.Domain
{
    [TestClass]
    public class ServiceToJsonTests
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
        public void GetJsonDataForService_InitCon_ShouldCreateFirstContactJson()
        {
            _service.ServiceType.Category = ServiceCategory.InitialConsultation;
            _service.ServiceTypeId = 1;
            _service.CompletedDate = DateTime.Today.AddDays(-7);

            var jsonResult = ServiceToJson.GetJsonDataForService(_service);

            var firstContact = JsonConvert.DeserializeObject<FirstContact>(jsonResult);
            firstContact.FirstFaceToFaceMeetingDate.Should().Be(DateTime.Today.AddDays(-7));
        }

        [TestMethod]
        public void GetJsonDataForService_WelcomePacket_ShouldCreateFirstContactJson()
        {
            _service.ServiceType.Category = ServiceCategory.WelcomePacket;
            _service.ServiceTypeId = 2;
            _service.CompletedDate = DateTime.Today.AddDays(-7);

            var jsonResult = ServiceToJson.GetJsonDataForService(_service);

            var firstContact = JsonConvert.DeserializeObject<FirstContact>(jsonResult);
            firstContact.EstimatedFirstMeetingDate.Should().Be(DateTime.Today.AddDays(-7));
        }

        [TestMethod]
        public void GetJsonDataForService_BundledSettlingInField_ShouldCreateDestinationChecklist()
        {
            _service.Order.ProgramName = "Test Bundled Program";
            _service.ServiceType.Category = ServiceCategory.SettlingIn;
            _service.ServiceTypeId = 3;
            _service.CompletedDate = DateTime.Today.AddDays(-7);

            var jsonResult = ServiceToJson.GetJsonDataForService(_service);

            var destinationChecklist = JsonConvert.DeserializeObject<DestinationChecklist>(jsonResult);
            destinationChecklist.SocialSecurityRegistration.Should().Be(DateTime.Today.AddDays(-7));
        }

        [TestMethod]
        public void GetJsonDataForService_BundledAreaOrientationField_ShouldCreateDestinationChecklist()
        {
            _service.Order.ProgramName = "Test Bundled Program";
            _service.ServiceType.Category = ServiceCategory.AreaOrientation;
            _service.ServiceTypeId = 24;
            _service.CompletedDate = DateTime.Today.AddDays(-7);

            var jsonResult = ServiceToJson.GetJsonDataForService(_service);

            var destinationChecklist = JsonConvert.DeserializeObject<DestinationChecklist>(jsonResult);
            destinationChecklist.HousingNeighborhoods.Should().Be(DateTime.Today.AddDays(-7));
        }

        [TestMethod]
        public void GetJsonDataForService_SettlingInField_ShouldCreateSettlingInJson()
        {
            _service.ServiceType.Category = ServiceCategory.SettlingIn;
            _service.ServiceTypeId = 3;
            _service.CompletedDate = DateTime.Today.AddDays(-7);

            var jsonResult = ServiceToJson.GetJsonDataForService(_service);

            var settlingIn = JsonConvert.DeserializeObject<SettlingIn>(jsonResult);
            settlingIn.SocialSecurityRegistration = DateTime.Today.AddDays(-7);
        }

        [TestMethod]
        public void GetJsonDataForService_AreaOrientationField_ShouldCreateAreaOrientationJson()
        {
            _service.ServiceType.Category = ServiceCategory.AreaOrientation;
            _service.ServiceTypeId = 24;
            _service.CompletedDate = DateTime.Today.AddDays(-7);

            var jsonResult = ServiceToJson.GetJsonDataForService(_service);

            var areaOrientation = JsonConvert.DeserializeObject<AreaOrientation>(jsonResult);
            areaOrientation.HousingNeighborhoods.Should().Be(DateTime.Today.AddDays(-7));
        }
    }
}
