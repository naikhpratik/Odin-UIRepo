using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using Odin.Data.Builders;
using Odin.Data.Core.Dtos;
using Odin.Data.Core.Models;
using Odin.Data.Helpers;
using Odin.Data.Persistence;
using Odin.Domain;
using Odin.IntegrationTests.Helpers;

namespace Odin.IntegrationTests.Domain
{
    [TestFixture]
    public class OrderImporterTests
    {
        private ApplicationDbContext context;
        private OrderImporter orderImporter;
        private ConsultantDto consultantDto;
        private Consultant consultant;
        private ProgramManagerDto programManagerDto;
        private Manager programManager;
        private TransfereeDto transfereeDto;
        private Transferee transferee;
        private IMapper mapper;

        [SetUp]
        public void SetUp()
        {
            context = new ApplicationDbContext();

            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));
            mapper = config.CreateMapper();

            transferee = context.Transferees.First(u => u.UserName.Equals("odinee@dwellworks.com"));
            transfereeDto = mapper.Map<Transferee, TransfereeDto>(transferee);
            consultant = context.Consultants.First(u => u.UserName.Equals("odinconsultant@dwellworks.com"));
            consultantDto = mapper.Map<Consultant, ConsultantDto>(consultant);
            programManager = context.Managers.First(u => u.UserName.Equals("odinpm@dwellworks.com"));
            programManagerDto = mapper.Map<Manager, ProgramManagerDto>(programManager);

            orderImporter = new OrderImporter(new UnitOfWork(context), mapper);
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }

        [Test, Isolated]
        public void ImportOrder_NewTransferee_CreatesNewTransferee()
        {
            // Arrange 
            var orderDto = OrderHelper.CreateOrderDto(consultantDto, programManagerDto, TransfereeDtoBuilder.New(), TokenHelper.NewToken());
            orderDto.Transferee.Email = "Testnew@test.com";

            // Act
            orderImporter.ImportOrder(orderDto);
            var order = context.Orders.SingleOrDefault(o => o.TrackingId.Equals(orderDto.TrackingId));
            var transferee = context.Transferees.SingleOrDefault(t => t.Email.Equals(orderDto.Transferee.Email));

            // Assert
            order.Should().NotBeNull();
            transferee.Should().NotBeNull();
        }

        [Test, Isolated]
        public void ImportOrder_ExistingTransferee_DoesNotCreateDuplicateTransferee()
        {
            // Arrange
            var orderDto = OrderHelper.CreateOrderDto(consultantDto, programManagerDto, transfereeDto, TokenHelper.NewToken());

            // Act
            orderImporter.ImportOrder(orderDto);
            var transferee = context.Transferees.Where(t => t.Email.Equals(transfereeDto.Email));

            // Assert
            transferee.Count().Should().Be(1);
        }

        [Test, Isolated]
        public void ImportOrder_NewOrderExistingTransferee_AddsOrderToTransferee()
        {
            // Arrange
            var order = OrderHelper.CreateOrder(consultant, manager: programManager, transferee: transferee,
                TrackingId: TokenHelper.NewToken());
            context.Orders.Add(order);
            context.SaveChanges();
            var orderDto = OrderHelper.CreateOrderDto(consultantDto, programManagerDto, transfereeDto, TokenHelper.NewToken());


            // Act
            orderImporter.ImportOrder(orderDto);
            var orders = context.Transferees.Where(t => t.Email.Equals(order.Transferee.Email)).Include(t => t.Orders).Select(t => t.Orders);
            
            // Assert
            orders.Count().Should().Be(2);
        }

        // If transferee is old and has orders it adds order to transferee and does not add transferee
        // If transferee is new it adds the transferee
        // If Order is new it inserts the order
        // If order is old it updates the order
        // if there is a new DSC on the order it goes through a new DSC process and adds dsc to DB and order
        // if there is an old DSC on an order it goes through an email DSC process, does not add new DSC to database
        // if there order is old but the DSC is new, it goes through a DSC reassign process
        // if the order is old and the PM is new, it reassigns the Program Manager

    }
}
