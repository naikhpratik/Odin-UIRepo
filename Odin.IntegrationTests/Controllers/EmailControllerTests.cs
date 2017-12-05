using AutoMapper;
using NUnit.Framework;
using Odin.Controllers;
using Odin.Data.Core.Models;
using Odin.Data.Persistence;
using Odin.ViewModels.Mailers;
using Odin.IntegrationTests.TestAttributes;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Odin.Data.Builders;

namespace Odin.IntegrationTests.Controllers
{
    [TestFixture]
    public class EmailControllerTests
    {
        private EmailController _controller;
        private ApplicationDbContext _context;
        private Transferee _transferee;

        [SetUp]
        public void SetUp()
        {
            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));

            var _mapper = config.CreateMapper();

            _context = new ApplicationDbContext();
            _transferee = _context.Transferees.First(u => u.UserName.Equals("odinee@dwellworks.com"));
            _controller = new EmailController(new UnitOfWork(_context), _mapper);
        }

        //[TearDown]
        //public void TearDown()
        //{
        //    _context.Dispose();
        //}

        // Tests       

        //[Test, Isolated]
        //public async Task SendEmail_AddTransfereeName_Attach_PDF()
        //{
        //    // Arrange
            
        //    var order = OrderBuilder.New().First();
        //    order.Transferee = _transferee;
            
        //    _context.Orders.Add(order);
            
        //    EmailViewModel msgModel = new EmailViewModel();
        //    msgModel.id = order.Id;
        //    msgModel.Name = "";
        //    msgModel.Email = _transferee.Email;
        //    var ret = _controller.Index(msgModel);
        //    ret.Should().NotBeNull();
        //}
    }
}
