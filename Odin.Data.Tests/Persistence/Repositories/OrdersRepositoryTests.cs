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
            var consultant = new Consultant() {Id = "fake-user-id" } ;
            var order = new Order() {Transferee = new Transferee(), DestinationCity = "Vancouver", ProgramManager = new Manager(), Consultant = consultant };

            SetupRepositoryWithSource(new[] {order});

            var orders = _ordersRepository.GetOrdersFor(consultant.Id + "-");

            orders.Should().BeEmpty();
        }

        [TestMethod]
        public void GetOrdersFor_OrderIsForUser_ShouldReturnOrder()
        {
            var consultant = new Consultant() { Id = "fake-user-id" };
            var order = new Order() { Transferee = new Transferee(), DestinationCity = "Vancouver", ProgramManager = new Manager(), Consultant = consultant };
            
            SetupRepositoryWithSource(new[] { order });

            var orders = _ordersRepository.GetOrdersFor(consultant.Id);

            orders.Should().HaveCount(1);
        }

        [TestMethod]
        public void GetOrdersFor_OneOrderIsForUser_ShouldReturnCorrectOrder()
        {
            var consultant1 = new Consultant() {Id = "consultant1"};
            var consultant2 = new Consultant() { Id = "consultant2" };
            var order1 = new Order() { Transferee = new Transferee(), Id = 1,  ProgramManager = new Manager(), Consultant = consultant1};
            var order2 = new Order() { Transferee = new Transferee(), Id = 2, ProgramManager = new Manager(), Consultant = consultant2};
            
            SetupRepositoryWithSource(new[] { order1, order2 });

            var orders = _ordersRepository.GetOrdersFor(consultant1.Id);

            orders.Should().HaveCount(1);
            orders.FirstOrDefault()?.Id.Should().Be(1);
        }
    }
}
