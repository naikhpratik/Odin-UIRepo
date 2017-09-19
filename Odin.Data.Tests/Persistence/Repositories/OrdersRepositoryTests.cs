using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Odin.Data.Core.Models;
using Odin.Data.Persistence;
using Odin.Data.Tests.Extensions;

namespace Odin.Data.Tests.Persistence.Repositories
{
    [TestClass]
    public class OrdersRepositoryTests
    {
        private Mock<DbSet<Order>> _mockOrders;
        private OrdersRepository _ordersRepository;


        private void SetupRepositoryWithSource(IList<Order> source)
        {
            _mockOrders = new Mock<DbSet<Order>>();
            _mockOrders.SetSource(source);
            var mockContext = new Mock<IApplicationDbContext>();
            mockContext.SetupGet(c => c.Orders).Returns(_mockOrders.Object);

            _ordersRepository = new OrdersRepository(mockContext.Object);
        }

        [TestMethod]
        public void GetOrdersFor_OrderIsForDifferentUser_ShouldNotBeReturned()
        {
            var consultant = new ApplicationUser();
            var order = new Order() {Transferee = new ApplicationUser(), DestinationCity = "Vancouver", ProgramManager = new ApplicationUser(), Consultant = consultant };

            SetupRepositoryWithSource(new[] {order});

            var orders = _ordersRepository.GetOrdersFor(consultant. + "-");

            orders.Should().BeEmpty();
        }

        [TestMethod]
        public void GetOrdersFor_OrderIsForUser_ShouldReturnOrder()
        {
            var order = new Order() { Transferee = new Transferee(), DestinationCity = "Vancouver" };
            var assignment = new ConsultantAssignment() { Order = order, ConsultantId = "1" };

            SetupRepositoryWithSource(new[] { order }, new[] { assignment });

            var orders = _ordersRepository.GetOrdersFor(assignment.ConsultantId);

            orders.Should().HaveCount(1);
        }

        [TestMethod]
        public void GetOrdersFor_OneOrderIsForUser_ShouldReturnCorrectOrder()
        {
            var order1 = new Order() { Transferee = new Transferee(), Id = 1 };
            var order2 = new Order() { Transferee = new Transferee(), Id = 2 };
            var assignment1 = new ConsultantAssignment() { Order = order1, ConsultantId = "1" };
            var assignment2 = new ConsultantAssignment() {Order = order2, ConsultantId = "2"};

            SetupRepositoryWithSource(new[] { order1, order2 }, new[] { assignment1, assignment2 });

            var orders = _ordersRepository.GetOrdersFor(assignment1.ConsultantId);

            orders.Should().HaveCount(1);
            orders.FirstOrDefault()?.Id.Should().Be(1);
        }
    }
}
