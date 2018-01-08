using AutoMapper;
using FluentAssertions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Moq;
using NUnit.Framework;
using Odin.Controllers;
using Odin.Data.Core.Models;
using Odin.Data.Helpers;
using Odin.Data.Persistence;
using Odin.Helpers;
using Odin.IntegrationTests.Extensions;
using Odin.IntegrationTests.TestAttributes;
using Odin.ViewModels.Orders.Index;
using Odin.ViewModels.Orders.Transferee;
using Odin.ViewModels.Shared;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

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
            _controller.MockCurrentUserAndRole(_dsc.Id, _dsc.UserName,UserRoles.Consultant);

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
            historyViewModel.Message = notification.Message;

            historyViewModel.OrderId = notification.OrderId;
            historyViewModel.Title = notification.Title;

            historyViewModel.Id = notification.Id;

            //act 
            var result = _controller.HistoryPartial(order.Id) as PartialViewResult;


            //assert
            result.Model.Equals(historyViewModel);

        }

        [Test, Isolated]
        public void Index_WhenCalled_ShouldReturnOK()
        {

            //Initial
            var _mockBookMarkletHelper = new Mock<Controller>();
            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));
            var mapper = config.CreateMapper();
            var unitOfWork = new UnitOfWork(_context);
            var emailHelper = new EmailHelper();
            var accountHelper = new AccountHelper(emailHelper);
            
            //creating 2 orders 
            var orders = Odin.Data.Builders.OrderBuilder.New(2);
            orders.ForEach(o => o.ConsultantId = _dsc.Id);
            orders.ForEach(o => o.TransfereeId = _transferee.Id);
            orders.ForEach(o => o.ProgramManagerId = _pm.Id);
            orders.ForEach(o => o.TrackingId = TokenHelper.NewToken());
            var testDateTime = new DateTime(1999, 6, 14);
            orders.ForEach(o => o.PreTripDate = testDateTime);

            _controller = new OrdersController(unitOfWork, mapper, accountHelper);
            _context.Orders.AddRange(orders);
            _controller.MockCurrentUser(_dsc.Id, _dsc.UserName);

            //creating managers 
            var managerStore = new UserStore<Manager>(new ApplicationDbContext());
            var managerManager = new UserManager<Manager>(managerStore);

            //Manager 1
            string _odinPmUserName1 = "odinpm@dwellworks.com";
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
                pmUser1 = managerManager.FindByName(_odinPmUserName1);
            }

            //Manager 2
            string _odinPmUserName2 = "pratikpm@dwellworks.com";
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
                pmUser2 = managerManager.FindByName(_odinPmUserName2);
            }
            
            //putting Orders in DB
            _context.Orders.AddRange(orders);
            _context.SaveChanges();

            //ManagerViewModels
            ManagerViewModel mngrvms1 = new ManagerViewModel();
            mngrvms1.FirstName = pmUser1.FirstName;
            mngrvms1.Id = pmUser1.Id;
            mngrvms1.LastName = pmUser1.LastName;
            mngrvms1.phoneNumber = pmUser1.PhoneNumber;
            mngrvms1.Email = pmUser1.Email;

            ManagerViewModel mngrvms2 = new ManagerViewModel();
            mngrvms2.FirstName = pmUser2.FirstName;
            mngrvms2.Id = pmUser2.Id;
            mngrvms2.LastName = pmUser2.LastName;
            mngrvms2.phoneNumber = pmUser2.PhoneNumber;
            mngrvms2.Email = pmUser2.Email;


            //Creating view models 
            OrdersIndexViewModel ordersIndexViewModel1 = new OrdersIndexViewModel();
            ordersIndexViewModel1.Id = orders[0].Id;
            ordersIndexViewModel1.ProgramManager = mngrvms1;

            OrdersIndexViewModel ordersIndexViewModel2 = new OrdersIndexViewModel();
            ordersIndexViewModel1.Id = orders[1].Id;
            ordersIndexViewModel1.ProgramManager = mngrvms2;

            
            OrderIndexManagerViewModel orderIndexManagerViewModel = new OrderIndexManagerViewModel(Enumerable.Repeat(ordersIndexViewModel1, 1), Enumerable.Repeat(mngrvms1, 1));
            orderIndexManagerViewModel.Managers = Enumerable.Repeat(mngrvms2, 1);
            orderIndexManagerViewModel.OrdersIndexVm = Enumerable.Repeat(ordersIndexViewModel2, 1);
           
            //act 
            var OrdersIndexViewModelres = _controller.Index(pmUser1.Id) as ViewResult;

            //assert
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
            _controller.MockCurrentUserAndRole(_dsc.Id, _dsc.UserName,UserRoles.Consultant);
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
        public void PropertiesTransferee_TwoProperties_ViewingDate_Is_Set_WithGivenOptionExists_Count_ShouldBe2()
        {
            //arrange

            var order = new Order() { SeCustNumb = "867-5309", Transferee = _transferee, Consultant = _dsc, ProgramManager = _pm, TrackingId = "123Test" };
            order.HomeFinding = new HomeFinding();
            _controller.MockCurrentUserAndRole(_transferee.Id, _transferee.UserName,UserRoles.Transferee);
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
            _controller.MockCurrentUserAndRole(_dsc.Id, _dsc.UserName, UserRoles.Consultant);
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

        [Test, Isolated]
        public void PropertiesTransferee_TwoProperties_ViewingDate_Not_Set_WithGivenOptionExists_Count_ShouldBe2()
        {
            //arrange

            var order = new Order() { SeCustNumb = "867-5309", Transferee = _transferee, Consultant = _dsc, ProgramManager = _pm, TrackingId = "123Test" };
            order.HomeFinding = new HomeFinding();
            _controller.MockCurrentUserAndRole(_transferee.Id, _transferee.UserName,UserRoles.Transferee);
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

        [Test, Isolated]
        public void Dashboard_NoOrder_ShouldBeNotFound()
        {
            //arrange
            _controller.MockCurrentUserAndRole(_transferee.Id, _transferee.UserName, UserRoles.Transferee);
           

            //assert            
            var result = _controller.DashboardPartial("Not an order!");
            result.Should().BeOfType<HttpStatusCodeResult>();

            var codeResult = result as HttpStatusCodeResult;
            codeResult.StatusCode.Should().Be((int) HttpStatusCode.NotFound);
        }

        [Test, Isolated]
        public void Dashboard_Order_ShouldBeFound()
        {
            var order = new Order() { SeCustNumb = "867-5309", Transferee = _transferee, Consultant = _dsc, ProgramManager = _pm, TrackingId = "123Test", DestinationCity = "integration city"};
            _context.Orders.Add(order);
            _context.SaveChanges();

            //arrange
            _controller.MockCurrentUserAndRole(_transferee.Id, _transferee.UserName, UserRoles.Transferee);

            //assert            
            var result = _controller.DashboardPartial(order.Id);
            result.Should().BeOfType<PartialViewResult>();
        }
    }   
}
