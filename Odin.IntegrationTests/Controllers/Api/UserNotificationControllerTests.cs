using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using AutoMapper;
using Odin.Controllers.Api;
using Odin.Data.Builders;
using Odin.Data.Core.Dtos;
using Odin.Data.Core.Models;
using Odin.Data.Helpers;
using Odin.Data.Persistence;
using Odin.Domain;
using Odin.Extensions;
using Odin.IntegrationTests.Extensions;
using Odin.IntegrationTests.TestAttributes;
using Odin.Interfaces;
using Moq;
using System.Collections.Generic;
using FluentAssertions;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace Odin.IntegrationTests.Controllers.Api
{
    [TestFixture]
    public class UserNotificationControllerTests : WebApiBaseTest
    {
        private UserNotificationController SetUpUserNotificationController()
        {
            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));
            var mapper = config.CreateMapper();
            var unitOfWork = new UnitOfWork(Context);

            return new UserNotificationController(unitOfWork, mapper, new QueueStore());
        }

        [Test, Isolated]
        public void NotificationMarkAsRead_ValidMarkAsReadRequest_ShouldMarkAsRead()
        {
            //Arrange 
            var orders = OrderBuilder.New(1);
            orders.ForEach(o => o.ConsultantId = dsc.Id);
            orders.ForEach(o => o.TransfereeId = transferee.Id);
            orders.ForEach(o => o.ProgramManagerId = pm.Id);
            orders.ForEach(o => o.TrackingId = TokenHelper.NewToken());
            var testDateTime = new DateTime(1999, 6, 14);
            orders.ForEach(o => o.PreTripDate = testDateTime);



            Context.Orders.AddRange(orders);
            //Context.SaveChanges();

            var controller = SetUpUserNotificationController();
            controller.MockCurrentUser(dsc.Id, dsc.UserName);

            Notification notification = new Notification();
            notification.NotificationType = NotificationType.OrderCreated;
            notification.Message = "A New Order is Created - Test";
            notification.Title = "New Order Creation - Test";
            var order = orders[0];
            notification.OrderId = order.Id;

            UserNotification userNotification = new UserNotification(dsc, notification);

            Context.Notifications.Add(notification);
            Context.UserNotifications.Add(userNotification);

            Context.SaveChanges();

            //Act 
            var result = controller.NotificationMarkAsRead(userNotification.Id);

            //Assert 
            result.Should().BeOfType<OkNegotiatedContentResult<string>>();


        }

        [Test, Isolated]
        public void NotificationMarkAsRemoved_ValidMarkAsReadRequest_ShouldMarkAsRemoved()
        {
            //Arrange 
            var orders = OrderBuilder.New(1);
            orders.ForEach(o => o.ConsultantId = dsc.Id);
            orders.ForEach(o => o.TransfereeId = transferee.Id);
            orders.ForEach(o => o.ProgramManagerId = pm.Id);
            orders.ForEach(o => o.TrackingId = TokenHelper.NewToken());
            var testDateTime = new DateTime(1999, 6, 14);
            orders.ForEach(o => o.PreTripDate = testDateTime);



            Context.Orders.AddRange(orders);
            //Context.SaveChanges();

            var controller = SetUpUserNotificationController();
            controller.MockCurrentUser(dsc.Id, dsc.UserName);

            Notification notification = new Notification();
            notification.NotificationType = NotificationType.OrderCreated;
            notification.Message = "A New Order is Created - Test";
            notification.Title = "New Order Creation - Test";
            var order = orders[0];
            notification.OrderId = order.Id;

            UserNotification userNotification = new UserNotification(dsc, notification);

            Context.Notifications.Add(notification);
            Context.UserNotifications.Add(userNotification);

            Context.SaveChanges();

            //Act 
            var result = controller.NotificationMarkAsRemoved(userNotification.Id);

            //Assert 
            result.Should().BeOfType<OkNegotiatedContentResult<string>>();


        }
    }
}
