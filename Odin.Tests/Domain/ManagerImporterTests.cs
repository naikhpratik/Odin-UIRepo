using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Odin.Data.Core;
using Odin.Data.Core.Dtos;
using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;
using Odin.Domain;

namespace Odin.Tests.Domain
{
    [TestClass]
    public class ManagerImporterTests
    {
        private Mock<IManagersRepository> _mockManagersRepository;
        private ManagerImporter _importer;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockManagersRepository = new Mock<IManagersRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(u => u.Managers).Returns(_mockManagersRepository.Object);
            var mockMapper = new Mock<IMapper>();

            _importer = new ManagerImporter(mockUnitOfWork.Object, mockMapper.Object);
        }

        [TestMethod]
        public void ImportManagers_ExistingManager_DoesNotAddManager()
        {
            var manager = new Manager();
            var managerDto = new ManagerDto
            {
                SeContactUid = 1234
            };
            var managersDto = new ManagersDto() {Managers = new List<ManagerDto> {managerDto}};
            _mockManagersRepository.Setup(r => r.GetManagerBySeContactUid(1234)).Returns(manager);

            _importer.ImportManagers(managersDto);

            _mockManagersRepository.Verify(m => m.Add(It.IsAny<Manager>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void ImportManagers_NewManager_AddsManager()
        {
            var manager = new Manager();
            var managerDto = new ManagerDto();
            var managersDto = new ManagersDto() { Managers = new List<ManagerDto> { managerDto } };

            _importer.ImportManagers(managersDto);

            _mockManagersRepository.Verify(m => m.Add(It.IsAny<Manager>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void ImportManagers_OneNewManager_CallsAddOnlyOnce()
        {
            var manager = new Manager();
            var managerDto1 = new ManagerDto() {SeContactUid = 1234};
            var managerDto2 = new ManagerDto();
            var managersDto = new ManagersDto() { Managers = new List<ManagerDto> { managerDto1,managerDto2 } };
            _mockManagersRepository.Setup(r => r.GetManagerBySeContactUid(1234)).Returns(manager);

            _importer.ImportManagers(managersDto);

            _mockManagersRepository.Verify(m => m.Add(It.IsAny<Manager>(), It.IsAny<string>()), Times.Once);
        }   
    }
}
