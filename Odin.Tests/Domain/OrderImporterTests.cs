﻿using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Odin.Data.Core;
using Odin.Data.Core.Dtos;
using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;
using Odin.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Odin.Tests.Domain
{
    [TestClass]
    public class OrderImporterTests
    {
        private Mock<IManagersRepository> _mockManagersRepository;
        private OrderImporter _orderImporter;
        private Mock<IOrdersRepository> _mockOrdersRepository;
        private Mock<IConsultantsRepository> _mockConsultantsRepository;
        private Mock<ITransfereesRepository> _mockTransfereesRepository;
        private Mock<IServiceTypesRepository> _mockServiceTypeRepository;
        private Mock<IMapper> _mockMapper;        

        [TestInitialize]
        public void TestInitialize()
        {
            _mockOrdersRepository = new Mock<IOrdersRepository>();
            _mockManagersRepository = new Mock<IManagersRepository>();
            _mockConsultantsRepository = new Mock<IConsultantsRepository>();
            _mockTransfereesRepository = new Mock<ITransfereesRepository>();
            _mockServiceTypeRepository = new Mock<IServiceTypesRepository>();
            _mockMapper = new Mock<IMapper>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(u => u.Orders).Returns(_mockOrdersRepository.Object);
            mockUnitOfWork.SetupGet(u => u.Consultants).Returns(_mockConsultantsRepository.Object);
            mockUnitOfWork.SetupGet(u => u.Transferees).Returns(_mockTransfereesRepository.Object);
            mockUnitOfWork.SetupGet(u => u.Managers).Returns(_mockManagersRepository.Object);
            mockUnitOfWork.SetupGet(u => u.ServiceTypes).Returns(_mockServiceTypeRepository.Object);
            _orderImporter = new OrderImporter(mockUnitOfWork.Object, _mockMapper.Object);
        }

        [TestMethod]  
        public void ImportOrderTest_New()
        {
            var order = new Order() {Id = "1", TrackingId = "TestOrder" };
            var transferee = new TransfereeDto() { Email = "testorder@dwellworks.com" };
            var consultant = new ConsultantDto() { SeContactUid = 1};
            var programManager = new ProgramManagerDto() { SeContactUid = 1 };
            var manager = new Manager() { Id = "Test" };
            var managerDto = new ManagerDto
            {
                SeContactUid = 1
            };
            var managersDto = new ManagersDto() { Managers = new List<ManagerDto> { managerDto } };
            
            _mockManagersRepository.Setup(r => r.GetManagerBySeContactUid(1)).Returns(manager);
            _mockMapper.Setup(m => m.Map<ManagerDto, Manager>(It.IsAny<ManagerDto>())).Returns(manager);
            _mockTransfereesRepository.Setup(t => t.GetTransfereeByEmail(transferee.Email)).Returns(new Transferee() { Email = "testorder@dwellworks.com" });
            _mockConsultantsRepository.Setup(c => c.GetConsultantBySeContactUid(1)).Returns(new Consultant() {SeContactUid = 1 });

            var orderDto = new OrderDto();
            orderDto.Consultant = consultant;
            orderDto.Transferee = transferee;
            orderDto.ProgramManager = programManager;
            _mockMapper.Setup(o => o.Map<OrderDto, Order>(It.IsAny<OrderDto>())).Returns(order);            
            _orderImporter.ImportOrder(orderDto);
            _mockOrdersRepository.Verify(v => v.Add(It.IsAny<Order>()), Times.Once);
        }

        [TestMethod]
        public void ImportOrderTest_Update()
        {
            var order = new Order() { Id = "1", TrackingId = "TestOrder" };
            var transferee = new TransfereeDto() { Email = "testorder@dwellworks.com" };
            var consultant = new ConsultantDto() { SeContactUid = 1 };
            var programManager = new ProgramManagerDto() { SeContactUid = 1 };
            var manager = new Manager() { Id = "Test" };
            var managerDto = new ManagerDto
            {
                SeContactUid = 1
            };
            var managersDto = new ManagersDto() { Managers = new List<ManagerDto> { managerDto } };

            _mockManagersRepository.Setup(r => r.GetManagerBySeContactUid(1)).Returns(manager);
            _mockMapper.Setup(m => m.Map<ManagerDto, Manager>(It.IsAny<ManagerDto>())).Returns(manager);
            _mockTransfereesRepository.Setup(t => t.GetTransfereeByEmail(transferee.Email)).Returns(new Transferee() { Email = "testorder@dwellworks.com" });
            _mockConsultantsRepository.Setup(c => c.GetConsultantBySeContactUid(1)).Returns(new Consultant() { SeContactUid = 1 });
            _mockOrdersRepository.Setup(o => o.GetOrderByTrackingId("TestOrder")).Returns(order);
            var orderDto = new OrderDto();
            orderDto.Consultant = consultant;
            orderDto.Transferee = transferee;
            orderDto.ProgramManager = programManager;
            _mockMapper.Setup(o => o.Map<OrderDto, Order>(It.IsAny<OrderDto>())).Returns(order);
            _orderImporter.ImportOrder(orderDto);
            _mockOrdersRepository.Verify(v => v.Add(It.IsAny<Order>()), Times.Once);
        }

        [TestMethod]
        public void ImportOrder_WithServiceFlagAndNoServices_ShouldAddTwoServices()
        {
            var order = new Order() { Id = "1", TrackingId = "TestOrder", ServiceFlag = 3, IsInternational = false};
            var transferee = new TransfereeDto() { Email = "testorder@dwellworks.com" };
            var consultant = new ConsultantDto() { SeContactUid = 1 };
            var programManager = new ProgramManagerDto() { SeContactUid = 1 };
            var manager = new Manager() { Id = "Test" };
            var managerDto = new ManagerDto
            {
                SeContactUid = 1
            };
            var managersDto = new ManagersDto() { Managers = new List<ManagerDto> { managerDto } };

            _mockManagersRepository.Setup(r => r.GetManagerBySeContactUid(1)).Returns(manager);
            _mockMapper.Setup(m => m.Map<ManagerDto, Manager>(It.IsAny<ManagerDto>())).Returns(manager);
            _mockTransfereesRepository.Setup(t => t.GetTransfereeByEmail(transferee.Email)).Returns(new Transferee() { Email = "testorder@dwellworks.com" });
            _mockConsultantsRepository.Setup(c => c.GetConsultantBySeContactUid(1)).Returns(new Consultant() { SeContactUid = 1 });

            var cats = new List<ServiceCategory>
            {
                ServiceCategory.InitialConsultation,
                ServiceCategory.WelcomePacket
            };
            var ids = new List<int>();

            _mockServiceTypeRepository.Setup(st => st.GetDefaultServiceTypes(cats, ids, order.IsInternational)).Returns(new List<ServiceType>()
            {
                new ServiceType(){Id = 1, Category = ServiceCategory.InitialConsultation},
                new ServiceType(){Id = 2, Category = ServiceCategory.WelcomePacket}
            });

            var orderDto = new OrderDto();
            orderDto.Consultant = consultant;
            orderDto.Transferee = transferee;
            orderDto.ProgramManager = programManager;
            _mockMapper.Setup(o => o.Map<OrderDto, Order>(It.IsAny<OrderDto>())).Returns(order);
            _orderImporter.ImportOrder(orderDto);
            _mockOrdersRepository.Verify(v => v.Add(It.IsAny<Order>()), Times.Once);

            order.Services.Count.Should().Be(2);
            order.Services.SingleOrDefault(st => st.ServiceType.Category == ServiceCategory.InitialConsultation)
                .Should().NotBeNull();
            order.Services.SingleOrDefault(st => st.ServiceType.Category == ServiceCategory.WelcomePacket)
                .Should().NotBeNull();
        }

        [TestMethod]
        public void ImportOrder_WithServiceFlagAndNoNewServices_ShouldAddTwoServices()
        {
            var order = new Order() { Id = "1", TrackingId = "TestOrder", ServiceFlag = 3, IsInternational = false };
            var transferee = new TransfereeDto() { Email = "testorder@dwellworks.com" };
            var consultant = new ConsultantDto() { SeContactUid = 1 };
            var programManager = new ProgramManagerDto() { SeContactUid = 1 };
            var manager = new Manager() { Id = "Test" };
            var managerDto = new ManagerDto
            {
                SeContactUid = 1
            };
            var managersDto = new ManagersDto() { Managers = new List<ManagerDto> { managerDto } };

            _mockManagersRepository.Setup(r => r.GetManagerBySeContactUid(1)).Returns(manager);
            _mockMapper.Setup(m => m.Map<ManagerDto, Manager>(It.IsAny<ManagerDto>())).Returns(manager);
            _mockTransfereesRepository.Setup(t => t.GetTransfereeByEmail(transferee.Email)).Returns(new Transferee() { Email = "testorder@dwellworks.com" });
            _mockConsultantsRepository.Setup(c => c.GetConsultantBySeContactUid(1)).Returns(new Consultant() { SeContactUid = 1 });

            var cats = new List<ServiceCategory>();
            var ids = new List<int>();

            _mockServiceTypeRepository.Setup(st => st.GetDefaultServiceTypes(cats, ids, order.IsInternational)).Returns(new List<ServiceType>());

            var orderDto = new OrderDto();
            orderDto.Consultant = consultant;
            orderDto.Transferee = transferee;
            orderDto.ProgramManager = programManager;
            _mockMapper.Setup(o => o.Map<OrderDto, Order>(It.IsAny<OrderDto>())).Returns(order);
            _orderImporter.ImportOrder(orderDto);
            _mockOrdersRepository.Verify(v => v.Add(It.IsAny<Order>()), Times.Once);

            order.Services.Count.Should().Be(0);
        }
    }
}
