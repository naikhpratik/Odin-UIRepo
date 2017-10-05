using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Odin.Data.Core;
using Odin.Data.Core.Dtos;
using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;
using Odin.Domain;
using Odin.Interfaces;

namespace Odin.Tests.Domain
{
    [TestClass]
    public class ManagerImporterTests
    {
        private Mock<IManagersRepository> _mockManagersRepository;
        private Mock<IAccountHelper> _mockAccountHelper;
        private Mock<IMapper> _mockMapper;
        private ManagerImporter _importer;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockManagersRepository = new Mock<IManagersRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(u => u.Managers).Returns(_mockManagersRepository.Object);
            _mockMapper = new Mock<IMapper>();
            _mockAccountHelper = new Mock<IAccountHelper>();
            _importer = new ManagerImporter(mockUnitOfWork.Object, _mockMapper.Object, _mockAccountHelper.Object);
        }

        [TestMethod]
        public async Task ImportManagers_ExistingManager_DoesNotAddManager()
        {
            var manager = new Manager() {Id ="Test"};
            var managerDto = new ManagerDto
            {
                SeContactUid = 1234
            };
            var managersDto = new ManagersDto() {Managers = new List<ManagerDto> {managerDto}};
            _mockManagersRepository.Setup(r => r.GetManagerBySeContactUid(1234)).Returns(manager);
            _mockMapper.Setup(m => m.Map<ManagerDto, Manager>(It.IsAny<ManagerDto>())).Returns(manager);

            await _importer.ImportManagers(managersDto);

            _mockManagersRepository.Verify(m => m.Add(It.IsAny<Manager>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task ImportManagers_NewManager_AddsManager()
        {
            var manager = new Manager() {Id="Test"};
            var managerDto = new ManagerDto();
            var managersDto = new ManagersDto() { Managers = new List<ManagerDto> { managerDto } };
            _mockMapper.Setup(m => m.Map<ManagerDto, Manager>(It.IsAny<ManagerDto>())).Returns(manager);

            await _importer.ImportManagers(managersDto);

            _mockManagersRepository.Verify(m => m.Add(It.IsAny<Manager>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task ImportManagers_OneNewManager_CallsAddOnlyOnce()
        {
            var manager = new Manager() {Id="Test"};
            var managerDto1 = new ManagerDto() {SeContactUid = 1234};
            var managerDto2 = new ManagerDto();
            var managersDto = new ManagersDto() { Managers = new List<ManagerDto> { managerDto1,managerDto2 } };
            _mockManagersRepository.Setup(r => r.GetManagerBySeContactUid(1234)).Returns(manager);
            _mockMapper.Setup(m => m.Map<ManagerDto, Manager>(It.IsAny<ManagerDto>())).Returns(manager);

            await _importer.ImportManagers(managersDto);

            _mockManagersRepository.Verify(m => m.Add(It.IsAny<Manager>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task ImportManagers_NewManager_AccountHelperSendsEmail()
        {
            var manager = new Manager(){Id="Test"};
            var managerDto = new ManagerDto();
            var managersDto = new ManagersDto() { Managers = new List<ManagerDto> { managerDto } };
            _mockMapper.Setup(m => m.Map<ManagerDto, Manager>(It.IsAny<ManagerDto>())).Returns(manager);

            await _importer.ImportManagers(managersDto);

            _mockAccountHelper.Verify(a => a.SendEmailCreateTokenAsync(It.IsAny<string>()), Times.Once);
        }
    }
}
