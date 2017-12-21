using AutoMapper;
using NUnit.Framework;
using Odin.Controllers;
using Odin.Data.Core.Models;
using Odin.Data.Persistence;
using Odin.Helpers;
using System.Linq;
using Odin.IntegrationTests.TestAttributes;
using Moq;
using Odin.Data.Helpers;
using System;
using Odin.IntegrationTests.Extensions;
using System.Web.Mvc;
using Odin.ViewModels.Orders.Transferee;
using FluentAssertions;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Odin.ViewModels.Orders;
using Odin.ViewModels.Orders.Index;
using Odin.ViewModels.Shared;
using System.Collections.Generic;

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
            var accountHelper = new AccountHelper(emailHelper);
            _controller = new OrdersController(new UnitOfWork(_context), mapper, accountHelper);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test, Isolated]
        public void HistoryPartial_InvalidOrderId_ShouldReturnOk_Integration()
        {
            // Arrange

            var orders = Odin.Data.Builders.OrderBuilder.New(1);
            orders.ForEach(o => o.ConsultantId = _dsc.Id);
            orders.ForEach(o => o.TransfereeId = _transferee.Id);
            orders.ForEach(o => o.ProgramManagerId = _pm.Id);
            orders.ForEach(o => o.TrackingId = TokenHelper.NewToken());
            var testDateTime = new DateTime(1999, 6, 14);
            orders.ForEach(o => o.PreTripDate = testDateTime);


            var _mockBookMarkletHelper = new Mock<Controller>();
            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));
            var mapper = config.CreateMapper();
            var unitOfWork = new UnitOfWork(_context);
            var emailHelper = new EmailHelper();
            var accountHelper = new AccountHelper(emailHelper);

            _controller = new OrdersController(unitOfWork, mapper, accountHelper);
            _context.Orders.AddRange(orders);
            _controller.MockCurrentUser(_dsc.Id, _dsc.UserName);

            Notification notification = new Notification();
            notification.NotificationType = NotificationType.OrderCreated;
            notification.Message = "A New Order is Created - Test";
            notification.Title = "New Order Creation - Test";
            var order = orders[0];
            notification.OrderId = order.Id;

            UserNotification userNotification = new UserNotification(_dsc, notification);

            _context.Notifications.Add(notification);
            _context.UserNotifications.Add(userNotification);
            _context.SaveChanges();

            HistoryViewModel historyViewModel = new HistoryViewModel();
            historyViewModel.NotificationMessage = notification.Message;

            historyViewModel.NotificationOrderId = notification.OrderId;
            historyViewModel.NotificationTitle = notification.Title;

            historyViewModel.IsRead = false;
            historyViewModel.IsRemoved = false;
            historyViewModel.NotificationUserNotificationId = notification.Id;


            historyViewModel.NotificationNotificationType = NotificationType.OrderCreated;

            //act 
            var result = _controller.HistoryPartial(order.Id) as PartialViewResult;


            //assert
            result.Model.Equals(historyViewModel);

        }

        [Test, Isolated]
        public void Index_WhenCalled_ShouldReturnOK()
        {
            //creating 2 orders 
            var orders = Odin.Data.Builders.OrderBuilder.New(2);
            orders.ForEach(o => o.ConsultantId = _dsc.Id);
            orders.ForEach(o => o.TransfereeId = _transferee.Id);
            orders.ForEach(o => o.ProgramManagerId = _pm.Id);
            orders.ForEach(o => o.TrackingId = TokenHelper.NewToken());
            var testDateTime = new DateTime(1999, 6, 14);
            orders.ForEach(o => o.PreTripDate = testDateTime);

            //blah 
            var _mockBookMarkletHelper = new Mock<Controller>();
            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));
            var mapper = config.CreateMapper();
            var unitOfWork = new UnitOfWork(_context);
            var emailHelper = new EmailHelper();
            var accountHelper = new AccountHelper(emailHelper);

            _controller = new OrdersController(unitOfWork, mapper, accountHelper);
            _context.Orders.AddRange(orders);
            _controller.MockCurrentUser(_dsc.Id, _dsc.UserName);

            //creating managers 
            var managerStore = new UserStore<Manager>(new ApplicationDbContext());
            var managerManager = new UserManager<Manager>(managerStore);

            
            string _odinPmUserName1 = "odinpm@dwellworks.com";
            string _odinPmUserName2 = "pratikpm@dwellworks.com";

            var pmUser1 = managerManager.FindByName(_odinPmUserName1);
            if (pmUser1 == null)
            {
                var newPm = new Manager()
                {
                    UserName = _odinPmUserName1,
                    FirstName = "Odin",
                    LastName = "Pm",
                    Email = _odinPmUserName1,
                    PhoneNumber = "2166824239"
                };
                managerManager.Create(newPm, "OdinOdin5$");
                managerManager.AddToRole(newPm.Id, UserRoles.ProgramManager);
            }

            var pmUser2 = managerManager.FindByName(_odinPmUserName2);
            if (pmUser2 == null)
            {
                var newPm = new Manager()
                {
                    UserName = _odinPmUserName2,
                    FirstName = "Pratik",
                    LastName = "Pm",
                    Email = _odinPmUserName2,
                    PhoneNumber = "21689578545"
                };
                managerManager.Create(newPm, "OdinOdin5$");
                managerManager.AddToRole(newPm.Id, UserRoles.ProgramManager);
            }

            //relating orders and managers 
           // orders[0].ProgramManager = pmUser1;
           // orders[1].ProgramManager = pmUser1;

            //putting manages in DB
            _context.Orders.AddRange(orders);
            //_context.Orders.Add(orders[1]);
           // _context.Managers.Add(pmUser1);
           // _context.Managers.Add(pmUser2);
            _context.SaveChanges();

            ManagerViewModel mngrvms1 = new ManagerViewModel();
            mngrvms1.FirstName = pmUser1.FirstName;
            mngrvms1.Id = pmUser1.Id;
            mngrvms1.LastName = pmUser1.LastName;
            mngrvms1.phoneNumber = pmUser1.PhoneNumber;
            mngrvms1.Email = pmUser1.Email;

            //Creating view models 
            OrdersIndexViewModel ordersIndexViewModel1 = new OrdersIndexViewModel();
            ordersIndexViewModel1.Id = orders[0].Id;
           
            ordersIndexViewModel1.ProgramManager = mngrvms1;

            //OrdersIndexViewModel ordersIndexViewModel2 = new OrdersIndexViewModel();
            //ordersIndexViewModel2.Id = orders[1].Id;
            //ordersIndexViewModel2.ProgramManager.FirstName = pmUser1.FirstName;
            //ordersIndexViewModel2.ProgramManager.LastName = pmUser1.LastName;
            //ordersIndexViewModel2.ProgramManager.Email = pmUser1.Email;
            //ordersIndexViewModel2.ProgramManager.phoneNumber = pmUser1.PhoneNumber;

            //OrdersIndexViewModel ordersIndexViewModel2 = new OrdersIndexViewModel();

            //ordersIndexViewModel2.ProgramManager.FirstName = pmUser2.FirstName;
            //ordersIndexViewModel2.ProgramManager.LastName = pmUser2.LastName;
            //ordersIndexViewModel2.ProgramManager.Email = pmUser2.Email;
            //ordersIndexViewModel2.ProgramManager.phoneNumber = pmUser2.PhoneNumber;

            Enumerable.Repeat(orders[0], 1);


            ManagerViewModel mngrvms2 = new ManagerViewModel();
            mngrvms2.FirstName = pmUser2.FirstName;
            mngrvms2.Id = pmUser2.Id;
            mngrvms2.LastName = pmUser2.LastName;
            mngrvms2.phoneNumber = pmUser2.PhoneNumber;
            mngrvms2.Email = pmUser2.Email;

            OrderIndexManagerViewModel orderIndexManagerViewModel = new OrderIndexManagerViewModel(Enumerable.Repeat(ordersIndexViewModel1, 1), Enumerable.Repeat(mngrvms1, 1));
            //IEnumerable<ManagerViewModel> oivms1 = new IEnumerable<ManagerViewModel>();

            //act 
            var OrdersIndexViewModelres = _controller.Index(pmUser1.Id) as ViewResult;

            //assert
            //foreach (var vms in OrdersIndexViewModelres.Model.Equals())
            //{

            //}
            OrdersIndexViewModelres.Model.Equals(orderIndexManagerViewModel);
        }
        //Built with expectation that order index would be populated with a view model.  Currently it is not.  Leaving in case this changes in the future.
        //[Test, Isolated]
        //public void Index_ValidRequest_ShouldReturnOrders()
        //{
        //    // Arrange
        //    var order = new Order() { SeCustNumb = "867-5309", Transferee = _transferee, Consultant = _dsc, ProgramManager = _pm, TrackingId = "123Test"};
        //    ServiceType serviceType = _context.ServiceTypes.First();
        //    order.Services.Add(new Service() { ServiceTypeId = serviceType.Id, OrderId = order.Id });

        //    _controller.MockCurrentUser(_dsc.Id, _dsc.UserName);
        //    _context.Orders.Add(order);
        //    _context.SaveChanges();

        //    // Act
        //    var result = _controller.Index();
        //    var model = result.ViewData.Model as IEnumerable<OrdersIndexViewModel>;

        //    // Assertion
        //    model.Should().NotBeNull();
        //    var newOrder = model.FirstOrDefault(o => o.SeCustNumb == order.SeCustNumb);
        //    newOrder.Should().NotBeNull();
        //    newOrder.Transferee.Should().NotBeNull();
        //    newOrder.ProgramManager.Should().NotBeNull();
        //    newOrder.Services.Count().Should().Be(1);
        //}

        //Built with expectation that order index would be populated with a view model.  Currently it is not.  Leaving in case this changes in the future.
        //[Test, Isolated]
        //public void Index_ValidRequestWithNoOrders_ShouldReturnNoOrders()
        //{
        //    // Arrange
        //    var order = new Order() { SeCustNumb = "867-5309", Transferee = _transferee, Consultant = _dsc, ProgramManager = _pm, TrackingId = "123Test" };
        //    ServiceType serviceType = _context.ServiceTypes.First();
        //    order.Services.Add(new Service() { ServiceTypeId = serviceType.Id, OrderId = order.Id });

        //    _controller.MockCurrentUser(_dsc.Id, _dsc.UserName);
        //    _context.Orders.Add(order);
        //    _context.SaveChanges();

        //    // Act
        //    var result = _controller.Index();
        //    var model = result.ViewData.Model as IEnumerable<OrdersIndexViewModel>;

        //    // Assertion
        //    model.Should().NotBeNull();
        //    var newOrder = model.FirstOrDefault(o => o.SeCustNumb == order.SeCustNumb);
        //    newOrder.Should().NotBeNull();
        //    newOrder.Transferee.Should().NotBeNull();
        //    newOrder.ProgramManager.Should().NotBeNull();
        //    newOrder.Services.Count().Should().Be(1);
        //}

        [Test, Isolated]
        public void Properties_TwoProperties_ViewingDate_Is_Set_WithGivenOptionExists_Count_ShouldBe2()
        {
            //arrange

            var order = new Order() { SeCustNumb = "867-5309", Transferee = _transferee, Consultant = _dsc, ProgramManager = _pm, TrackingId = "123Test" };
            order.HomeFinding = new HomeFinding();
            _controller.MockCurrentUser(_dsc.Id, _dsc.UserName);
            _context.Orders.Add(order);
            _context.SaveChanges();

            //act
            HomeFindingProperty p1 = new HomeFindingProperty();
            p1.Deleted = false;
            p1.Property = new Property();
            p1.ViewingDate = DateTime.Now.AddDays(10);
            order.HomeFinding.HomeFindingProperties.Add(p1);

            HomeFindingProperty p2 = new HomeFindingProperty();
            p2.Deleted = false;
            p2.Property = new Property();
            p2.ViewingDate = DateTime.Now.AddDays(20);
            order.HomeFinding.HomeFindingProperties.Add(p2);

            //assert            
            var result = _controller.PropertiesPartialPDF(order.Id, "ViewingsOnly");
            Assert.IsTrue(result.GetType().ToString().Contains("Rotativa.ViewAsPdf"));
        }
        [Test, Isolated]
        public void Properties_TwoProperties_ViewingDate_Not_Set_WithGivenOptionExists_Count_ShouldBe2()
        {
            //arrange

            var order = new Order() { SeCustNumb = "867-5309", Transferee = _transferee, Consultant = _dsc, ProgramManager = _pm, TrackingId = "123Test" };
            order.HomeFinding = new HomeFinding();
            _controller.MockCurrentUser(_dsc.Id, _dsc.UserName);
            _context.Orders.Add(order);
            _context.SaveChanges();

            //act
            HomeFindingProperty p1 = new HomeFindingProperty();
            p1.Deleted = false;
            p1.Property = new Property();
            order.HomeFinding.HomeFindingProperties.Add(p1);

            HomeFindingProperty p2 = new HomeFindingProperty();
            p2.Deleted = false;
            p2.Property = new Property();
            order.HomeFinding.HomeFindingProperties.Add(p2);

            //assert            
            var result = _controller.PropertiesPartialPDF(order.Id, "ViewingsOnly");
            result.Should().BeOfType<HttpNotFoundResult>();
        }
    }
}
