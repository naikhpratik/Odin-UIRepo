using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using Odin.Controllers;
using Odin.Data.Core.Models;
using Odin.Data.Persistence;
using Odin.IntegrationTests.Extensions;
using Odin.IntegrationTests.TestAttributes;
using Odin.ViewModels;

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
            var order = new Order() { OriginCity = "ClevelandTest", Transferee = _transferee, Consultant = _dsc, ProgramManager = _pm, TrackingId = "123Test"};
            _controller.MockCurrentUser(_dsc.Id, _dsc.UserName);
            _context.Orders.Add(order);
            _context.SaveChanges();

            // Act
            var result = _controller.Index();
            var model = result.ViewData.Model as IEnumerable<OrderIndexViewModel>;

            // Assertion
            model.Should().NotBeNull();
            var newOrder = model.FirstOrDefault(o => o.OriginCity.Equals(order.OriginCity));
            newOrder.Should().NotBeNull();
        }

    }
}
