using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Odin.Data.Core;
using Odin.Data.Core.Dtos;
using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;
using Odin.Data.Persistence;
using Odin.Domain;
using Odin.Data.Tests.Extensions;

namespace Odin.Tests.Domain
{
    [TestClass]
    public class ConsultantImporterTests
    {
        private ConsultantImporter _importer;
        private Mock<IConsultantsRepository> _mockConsultantRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockConsultantRepository = new Mock<IConsultantsRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(u => u.Consultants).Returns(_mockConsultantRepository.Object);
            var mockMapper = new Mock<IMapper>();
            
            _importer = new ConsultantImporter(mockUnitOfWork.Object, mockMapper.Object);
        }

        [TestMethod]
        public void ImportConsultants_NewConsultant_AddConsultant()
        {
            var consultant = new Consultant();
            var consultantDto = new ConsultantImportDto();
            var consultantsDto = new ConsultantsDto() {Consultants = new List<ConsultantImportDto> {consultantDto}};

            _importer.ImportConsultants(consultantsDto);

            _mockConsultantRepository.Verify(m => m.Add(It.IsAny<Consultant>()), Times.Once);
        }

        [TestMethod]
        public void ImportConsultant_ExistingConsultant_DoesNotCallAddConsultant()
        {
            var consultant = new Consultant();
            consultant.SeContactUid = 1234;
            var consultantDto = new ConsultantImportDto();
            var consultantsDto = new ConsultantsDto() { Consultants = new List<ConsultantImportDto> { consultantDto } };
            _mockConsultantRepository.Setup(r => r.GetConsultantBySeContactUid(It.IsAny<int>())).Returns((consultant));

            _importer.ImportConsultants(consultantsDto);

            _mockConsultantRepository.Verify(m => m.Add(It.IsAny<Consultant>()), Times.Never);
        }

        [TestMethod]
        public void ImportConsultants_OneNewConsultant_CallsAddOnlyOnce()
        {
            var consultant = new Consultant();
            var consultantDto = new ConsultantImportDto(){SeContactUid = 1234};
            var consultantDto2 = new ConsultantImportDto();
            var consultantsDto = new ConsultantsDto() { Consultants = new List<ConsultantImportDto> { consultantDto, consultantDto2 } };
            _mockConsultantRepository.Setup(r => r.GetConsultantBySeContactUid(1234)).Returns((consultant));

            _importer.ImportConsultants(consultantsDto);

            _mockConsultantRepository.Verify(m => m.Add(It.IsAny<Consultant>()), Times.Once);
        }
    }
}
