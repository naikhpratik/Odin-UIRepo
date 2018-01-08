using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Odin.Data.Core.Models;
using Odin.Data.Persistence;
using Odin.Data.Tests.Extensions;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

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
            var order = new Order() {Transferee = new Transferee(), DestinationCity = "Vancouver", ProgramManager = new Manager(), Consultant = consultant, ConsultantId = consultant.Id };

            SetupRepositoryWithSource(new[] {order});

            var orders = _ordersRepository.GetOrdersFor(consultant.Id + "-",UserRoles.Consultant);

            orders.Should().BeEmpty();
        }

        [TestMethod]
        public void GetOrdersFor_OrderIsCancelled_ShouldNotReturnOrder()
        {
            var consultant = new Consultant() { Id = "fake-user-id" };
            var order = new Order() { Transferee = new Transferee(), DestinationCity = "Vancouver", ProgramManager = new Manager(), Consultant = consultant, ConsultantId = consultant.Id, SeCustStatus = OrderStatus.Cancelled };

            SetupRepositoryWithSource(new[] { order });

            var orders = _ordersRepository.GetOrdersFor(consultant.Id,UserRoles.Consultant);

            orders.Should().HaveCount(0);
        }

        [TestMethod]
        public void GetOrdersFor_OrderIsClosed_ShouldNotReturnOrder()
        {
            var consultant = new Consultant() { Id = "fake-user-id" };
            var order = new Order() { Transferee = new Transferee(), DestinationCity = "Vancouver", ProgramManager = new Manager(), Consultant = consultant, ConsultantId = consultant.Id, SeCustStatus = OrderStatus.Closed };

            SetupRepositoryWithSource(new[] { order });

            var orders = _ordersRepository.GetOrdersFor(consultant.Id, UserRoles.Consultant);

            orders.Should().HaveCount(0);
        }

        [TestMethod]
        public void GetOrdersFor_OrderIsForUser_ShouldReturnOrder()
        {
            var consultant = new Consultant() { Id = "fake-user-id" };
            var order = new Order() { Transferee = new Transferee(), DestinationCity = "Vancouver", ProgramManager = new Manager(), Consultant = consultant, ConsultantId = consultant.Id};
            
            SetupRepositoryWithSource(new[] { order });

            var orders = _ordersRepository.GetOrdersFor(consultant.Id, UserRoles.Consultant);

            orders.Should().HaveCount(1);
        }

        [TestMethod]
        public void GetOrdersFor_OneOrderIsForUser_ShouldReturnCorrectOrder()
        {
            var consultant1 = new Consultant() {Id = "consultant1"};
            var consultant2 = new Consultant() { Id = "consultant2" };
            var order1 = new Order() { ConsultantId = consultant1.Id, Id = "1", Consultant = consultant1};
            var order2 = new Order() { ConsultantId = consultant2.Id, Id = "2", Consultant = consultant2};
            
            SetupRepositoryWithSource(new[] { order1, order2 });

            var orders = _ordersRepository.GetOrdersFor(consultant1.Id, UserRoles.Consultant);

            orders.Should().HaveCount(1);
            orders.FirstOrDefault()?.Id.Should().Be("1");
        }

        [TestMethod]
        public void GetOrderById_OrderWithIdExists_ShouldReturnCorrectOrder()
        {
            var orderId = "1";

            var order1 = new Order() { Id = "1"};
            var order2 = new Order() { Id = "2"};

            SetupRepositoryWithSource(new[] { order1, order2 });

            var order = _ordersRepository.GetOrderById(orderId);

            order.Id.Should().Be(orderId);
        }

        [TestMethod]
        public void GetOrderById_OrderWithIdDoesNotExist_ShouldReturnNull()
        {
            var orderId = "3";

            var order1 = new Order() { Id = "1" };
            var order2 = new Order() { Id = "2" };

            SetupRepositoryWithSource(new[] { order1, order2 });

            var order = _ordersRepository.GetOrderById(orderId);

            order.Should().Be(null);
        }
    }
}
