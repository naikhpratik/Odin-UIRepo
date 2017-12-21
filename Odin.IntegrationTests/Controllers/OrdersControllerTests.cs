using AutoMapper;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Odin.Controllers;
using Odin.Data.Core.Models;
using Odin.Data.Helpers;
using Odin.Data.Persistence;
using Odin.Helpers;
using Odin.IntegrationTests.Extensions;
using Odin.IntegrationTests.TestAttributes;
using Odin.ViewModels.Orders.Transferee;
using System;
using System.Linq;
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
    }   
}
