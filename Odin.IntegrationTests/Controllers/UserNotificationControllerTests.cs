using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Odin.Controllers;
using Odin.Data;
using AutoMapper;
using NUnit.Framework;

using Odin.Data.Core.Models;
using Odin.Data.Persistence;
using Odin.Helpers;
using System.Linq;

namespace Odin.IntegrationTests.Controllers
{
    [TestClass]
    public class UserNotificationControllerTests
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
            var accountHelper = new AccountHelper(emailHelper);
            _controller = new OrdersController(new UnitOfWork(_context), mapper, accountHelper);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }




    }
}
