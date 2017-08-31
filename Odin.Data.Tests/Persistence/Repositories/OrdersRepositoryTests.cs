using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        private Mock<DbSet<ConsultantAssignment>> _mockAssignments;
        private OrdersRepository _ordersRepository;


        private void SetupRepositoryWithSource(IList<Order> source, IList<ConsultantAssignment> assignments = null)
        {
            _mockOrders = new Mock<DbSet<Order>>();
            _mockOrders.SetSource(source);
            var mockContext = new Mock<IApplicationDbContext>();
            mockContext.SetupGet(c => c.Orders).Returns(_mockOrders.Object);

            if (assignments != null)
            {
                _mockAssignments = new Mock<DbSet<ConsultantAssignment>>();
                _mockAssignments.SetSource(assignments);
                mockContext.SetupGet(c => c.ConsultantAssignments).Returns(_mockAssignments.Object);
            }

            _ordersRepository = new OrdersRepository(mockContext.Object);
        }

        [TestMethod]
        public void GetOrdersFor_OrderIsForDifferentUser_ShouldNotBeReturned()
        {
            var order = new Order() {Transferee = new Transferee(), DestinationCity = "Vancouver"};
            var assignment = new ConsultantAssignment() {Order = order, ConsultantId = "1"};

            SetupRepositoryWithSource(new[] {order}, new[] {assignment});

            var orders = _ordersRepository.GetOrdersFor(assignment.ConsultantId + "-");

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
    }
}
