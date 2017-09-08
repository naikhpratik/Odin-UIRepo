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
using Odin.ViewModels;

namespace Odin.IntegrationTests.Controllers
{
    [TestFixture]
    public class OrdersControllerTests 
    {
        private OrdersController _controller;
        private ApplicationDbContext _context;

        [SetUp]
        public void SetUp()
        {
            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));

            var mapper = config.CreateMapper();

            _context = new ApplicationDbContext();
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
            var transferee = new Transferee();
            var order = new Order() { OriginCity = "ClevelandTest", Transferee = transferee};
            var dsc = _context.Users.SingleOrDefault(u => u.UserName.Equals("austin.emser@dwellworks.com"));
            _controller.MockCurrentUser(dsc.Id, dsc.UserName);
            order.Consultants.Add(new ConsultantAssignment() { Consultant = dsc, Order = order });

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
