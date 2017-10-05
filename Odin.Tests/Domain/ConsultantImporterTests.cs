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
using Odin.Interfaces;

namespace Odin.Tests.Domain
{
    [TestClass]
    public class ConsultantImporterTests
    {
        private ConsultantImporter _importer;
        private Mock<IConsultantsRepository> _mockConsultantRepository;
        private Mock<IAccountHelper> _mockAccountHelper;
        private Mock<IMapper> _mockMapper;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockConsultantRepository = new Mock<IConsultantsRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(u => u.Consultants).Returns(_mockConsultantRepository.Object);
            _mockMapper = new Mock<IMapper>();
            _mockAccountHelper = new Mock<IAccountHelper>();
            _importer = new ConsultantImporter(mockUnitOfWork.Object, _mockMapper.Object, _mockAccountHelper.Object);
        }

        [TestMethod]
        public async Task ImportConsultants_NewConsultant_AddConsultant()
        {
            var consultant = new Consultant() {Id = "Test"};
            var consultantDto = new ConsultantImportDto();
            var consultantsDto = new ConsultantsDto() {Consultants = new List<ConsultantImportDto> {consultantDto}};
            _mockMapper.Setup(m => m.Map<ConsultantImportDto, Consultant>(It.IsAny<ConsultantImportDto>()))
                .Returns(consultant);

            await _importer.ImportConsultants(consultantsDto);

            _mockConsultantRepository.Verify(m => m.Add(It.IsAny<Consultant>()), Times.Once);
        }

        [TestMethod]
        public async Task ImportConsultant_ExistingConsultant_DoesNotCallAddConsultant()
        {
            var consultant = new Consultant {SeContactUid = 1234, Id="Test"};
            var consultantDto = new ConsultantImportDto();
            var consultantsDto = new ConsultantsDto() { Consultants = new List<ConsultantImportDto> { consultantDto } };
            _mockConsultantRepository.Setup(r => r.GetConsultantBySeContactUid(It.IsAny<int>())).Returns((consultant));
            _mockMapper.Setup(m => m.Map<ConsultantImportDto, Consultant>(It.IsAny<ConsultantImportDto>()))
                .Returns(consultant);

            await _importer.ImportConsultants(consultantsDto);

            _mockConsultantRepository.Verify(m => m.Add(It.IsAny<Consultant>()), Times.Never);
        }

        [TestMethod]
        public async Task ImportConsultants_OneNewConsultant_CallsAddOnlyOnce()
        {
            var consultant = new Consultant() {Id="Test"};
            var consultantDto = new ConsultantImportDto(){SeContactUid = 1234};
            var consultantDto2 = new ConsultantImportDto();
            var consultantsDto = new ConsultantsDto() { Consultants = new List<ConsultantImportDto> { consultantDto, consultantDto2 } };
            _mockConsultantRepository.Setup(r => r.GetConsultantBySeContactUid(1234)).Returns((consultant));
            _mockMapper.Setup(m => m.Map<ConsultantImportDto, Consultant>(It.IsAny<ConsultantImportDto>()))
                .Returns(consultant);

            await _importer.ImportConsultants(consultantsDto);

            _mockConsultantRepository.Verify(m => m.Add(It.IsAny<Consultant>()), Times.Once);
        }

        [TestMethod]
        public async Task ImportConsultants_NewConsultant_SendsEmail()
        {
            var consultant = new Consultant() {Id="Test"};
            var consultantDto = new ConsultantImportDto();
            var consultantsDto = new ConsultantsDto() { Consultants = new List<ConsultantImportDto> { consultantDto } };
            _mockMapper.Setup(m => m.Map<ConsultantImportDto, Consultant>(It.IsAny<ConsultantImportDto>()))
                .Returns(consultant);

            await _importer.ImportConsultants(consultantsDto);

            _mockAccountHelper.Verify(a => a.SendEmailCreateTokenAsync(It.IsAny<string>()), Times.Once);
        }
    }
}
