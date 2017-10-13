using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using Odin.Controllers;
using Odin.Data.Core.Models;
using Odin.Data.Persistence;
using Odin.Helpers;
using Odin.IntegrationTests.Extensions;
using Odin.IntegrationTests.TestAttributes;
using Odin.ViewModels.Order.Index;
using System.Collections.Generic;
using System.Linq;

namespace Odin.IntegrationTests.Controllers
{
    [TestFixture]
    public class OrdersControllerTests 
    {
        private OrdersController _controller;
        private ApplicationDbContext _context;
        private Manager _pm;
        private Consultant _dsc;
        private Transferee _transferee;

        [SetUp]
        public void SetUp()
        {
            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));

            var mapper = config.CreateMapper();

            _context = new ApplicationDbContext();
            _transferee = _context.Transferees.First(u => u.UserName.Equals("odinee@dwellworks.com"));
            _dsc = _context.Consultants.First(u => u.UserName.Equals("odinconsultant@dwellworks.com"));
            _pm = _context.Managers.First(u => u.UserName.Equals("odinpm@dwellworks.com"));
            var emailHelper = new EmailHelper();            
            _controller = new OrdersController(new UnitOfWork(_context), mapper);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test, Isolated]
        public void Index_ValidRequest_ShouldReturnOrders()
        {
            // Arrange
            var order = new Order() { SeCustNumb = "867-5309", Transferee = _transferee, Consultant = _dsc, ProgramManager = _pm, TrackingId = "123Test"};
            ServiceType serviceType = _context.ServiceTypes.First();
            order.Services.Add(new Service() { ServiceTypeId = serviceType.Id, OrderId = order.Id });

            _controller.MockCurrentUser(_dsc.Id, _dsc.UserName);
            _context.Orders.Add(order);
            _context.SaveChanges();

            // Act
            var result = _controller.Index();
            var model = result.ViewData.Model as IEnumerable<OrderIndexViewModel>;

            // Assertion
            model.Should().NotBeNull();
            var newOrder = model.FirstOrDefault(o => o.SeCustNumb == order.SeCustNumb);
            newOrder.Should().NotBeNull();
            newOrder.Transferee.Should().NotBeNull();
            newOrder.ProgramManager.Should().NotBeNull();
            newOrder.Services.Count().Should().Be(1);
        }

        [Test, Isolated]
        public void Index_ValidRequestWithNoOrders_ShouldReturnNoOrders()
        {
            // Arrange
            var order = new Order() { SeCustNumb = "867-5309", Transferee = _transferee, Consultant = _dsc, ProgramManager = _pm, TrackingId = "123Test" };
            ServiceType serviceType = _context.ServiceTypes.First();
            order.Services.Add(new Service() { ServiceTypeId = serviceType.Id, OrderId = order.Id });

            _controller.MockCurrentUser(_dsc.Id, _dsc.UserName);
            _context.Orders.Add(order);
            _context.SaveChanges();

            // Act
            var result = _controller.Index();
            var model = result.ViewData.Model as IEnumerable<OrderIndexViewModel>;

            // Assertion
            model.Should().NotBeNull();
            var newOrder = model.FirstOrDefault(o => o.SeCustNumb == order.SeCustNumb);
            newOrder.Should().NotBeNull();
            newOrder.Transferee.Should().NotBeNull();
            newOrder.ProgramManager.Should().NotBeNull();
            newOrder.Services.Count().Should().Be(1);
        }

    }
}
