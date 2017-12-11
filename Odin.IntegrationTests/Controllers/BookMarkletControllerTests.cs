using AutoMapper;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Odin.Controllers;
using Odin.Data.Builders;
using Odin.Data.Core.Models;
using Odin.Data.Persistence;
using Odin.Domain;
using Odin.IntegrationTests.Extensions;
using Odin.IntegrationTests.TestAttributes;
using Odin.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Odin.IntegrationTests.Controllers
{
    [TestFixture]
    public class BookMarkletControllerTests
    {

        private BookMarkletController _controller;
        private ApplicationDbContext _context;
        private Manager _pm;
        private Consultant _dsc;
        private Transferee _transferee;
        private Mock<IBookMarkletHelper> _mockBookMarkletHelper;

        [SetUp]
        public void SetUp()
        {
            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));

            var mapper = config.CreateMapper();

            _context = new ApplicationDbContext();

            _transferee = _context.Transferees.First(u => u.UserName.Equals("odinee@dwellworks.com"));
            _pm = _context.Managers.First(u => u.UserName.Equals("odinpm@dwellworks.com"));

            _dsc = _context.Consultants.SingleOrDefault(u => u.Email.Equals("bm-integration@test.com"));
            if ( _dsc == null)
            {
                _dsc = new Consultant() { Email = "bm-integration@test.com", UserName = "bm-test" };
                _context.Consultants.Add(_dsc);
                _context.SaveChanges();
                _context.Entry(_dsc).Reload();
            }

            var queueStore = new QueueStore();
            _mockBookMarkletHelper = new Mock<IBookMarkletHelper>();

            _controller = new BookMarkletController(new UnitOfWork(_context), mapper, queueStore, _mockBookMarkletHelper.Object);
            _controller.MockCurrentUser(_dsc.Id, _dsc.UserName);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        // Tests
        [Test, Isolated]
        public void Index_ValidUrlHasOrders_ShowBmViewWithOrders()
        {
            // Arrange
            Order order = OrderBuilder.New().First();
            order.Transferee = _transferee;
            order.ProgramManager = _pm;
            order.Consultant = _dsc;
            _context.Orders.Add(order);
            _context.SaveChanges();
            _context.Entry(order).Reload();

            string url = "http://test.com";
            _mockBookMarkletHelper.Setup(r => r.IsValidUrl(url)).Returns(true);

            // Act
            var result = _controller.Index(url) as ViewResult;

            // Assert
            result.Should().NotBeNull();
            result.ViewName.Should().Be(String.Empty);
        }

        [Test, Isolated]
        public void Index_ValidUrlHasNoOrders_ShowError()
        {
            // Arrange
            string url = "http://test.com";
            _mockBookMarkletHelper.Setup(r => r.IsValidUrl(url)).Returns(true);

            // Act
            var result = _controller.Index(url) as ViewResult;

            // Assert
            result.Should().NotBeNull();
            result.ViewName.Should().Be("Error");
        }

        [Test, Isolated]
        public void Index_InValidUrlHasNoOrders_ShowError()
        {
            // Arrange
            string url = "http://test.com";
            _mockBookMarkletHelper.Setup(r => r.IsValidUrl(url)).Returns(false);

            // Act
            var result = _controller.Index(url) as ViewResult;

            // Assert
            result.Should().NotBeNull();
            result.ViewName.Should().Be("Error");
        }

        [Test, Isolated]
        public void Index_InValidUrlHasOrders_ShowError()
        {
            // Arrange
            Order order = OrderBuilder.New().First();
            order.Transferee = _transferee;
            order.ProgramManager = _pm;
            order.Consultant = _dsc;
            _context.Orders.Add(order);
            _context.SaveChanges();
            _context.Entry(order).Reload();

            string url = "http://test.com";
            _mockBookMarkletHelper.Setup(r => r.IsValidUrl(url)).Returns(false);

            // Act
            var result = _controller.Index(url) as ViewResult;

            // Assert
            result.Should().NotBeNull();
            result.ViewName.Should().Be("Error");
        }
    }
}
