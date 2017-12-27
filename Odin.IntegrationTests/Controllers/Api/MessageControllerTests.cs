
using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using Odin.Controllers.Api;
using Odin.Data.Builders;
using Odin.Data.Core.Models;
using Odin.Data.Persistence;
using Odin.IntegrationTests.Extensions;
using Odin.IntegrationTests.TestAttributes;
using Odin.ViewModels.Orders.Transferee;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System;
using Odin.Data.Core.Dtos;
using System.Collections.Generic;

namespace Odin.IntegrationTests.Controllers.Api
{
    [TestFixture]
    public class MessageControllerTests : WebApiBaseTest
    {
        private MessageController SetUpMessageController()
        {
            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));
            var mapper = config.CreateMapper();
            var unitOfWork = new UnitOfWork(Context);
            return new MessageController(unitOfWork, mapper);
        }
 
        [Test, Isolated]
        public async Task InsertMessage_NoProperty_ShouldReturnNotFound()
        {
            // arrange
            var controller = SetUpMessageController();
            controller.MockCurrentUser(dsc.Id, dsc.UserName);
            // Act            
            MessageDto dto = new MessageDto() { HomeFindingPropertyId = "-1", Id = null, MessageDate = DateTime.Now, MessageText = "Adding a new Message" };
            var result = controller.UpsertPropertyMessage(dto);
            // Assert
            result.Should().BeOfType<System.Web.Http.Results.NotFoundResult>();
        }
        [Test, Isolated]
        public void InsertMessage_ValidProperty_ShouldAddMessage_No_Notification_If_Not_EE_MAN_CON()
        {
            // arrange
            var controller = SetUpMessageController();
            controller.MockCurrentUser(dsc.Id, dsc.UserName);

            Order order = BuildOrder(false);
            Context.Orders.Add(order);
            Context.SaveChanges();
            Context.Entry(order).Reload();
            
            //act
            var prop = order.HomeFinding.HomeFindingProperties.First();
            Message mess = new Message() { HomeFindingPropertyId = prop.Id };
            MessageDto dto = new MessageDto() { HomeFindingPropertyId = prop.Id, OrderId = order.Id };
            
            //assert
            var result = controller.UpsertPropertyMessage(dto);
            var rl = controller.User.IsInRole(UserRoles.Transferee);
            rl.Should().BeFalse();
            var rlC = controller.User.IsInRole(UserRoles.Consultant);
            rlC.Should().BeFalse();
            var rlM = controller.User.IsInRole(UserRoles.ProgramManager);
            rlM.Should().BeFalse();
            dsc.UserNotifications.Count().Should().Be(0);
            prop.Messages.Count.Should().Be(1);
        }

        [Test, Isolated]
        public void InsertMessage_ValidProperty_ShouldAddMessageAnd_Consultant_Notification_If_Transferee()
        {
            // arrange
            var controller = SetUpMessageController();
            controller.MockCurrentUserAndRole(dsc.Id, dsc.UserName, UserRoles.Transferee);

            Order order = BuildOrder(false);
            Context.Orders.Add(order);
            Context.SaveChanges();
            Context.Entry(order).Reload();
            
            //act
            var prop = order.HomeFinding.HomeFindingProperties.First();
            Message mess = new Message() { HomeFindingPropertyId = prop.Id };
            MessageDto dto = new MessageDto() { HomeFindingPropertyId = prop.Id, OrderId = order.Id };
            
            //assert
            var result = controller.UpsertPropertyMessage(dto);
            prop.Messages.Count.Should().Be(1);

            var rl = controller.User.IsInRole(UserRoles.Transferee);
            rl.Should().BeTrue();

            dsc.UserNotifications.Count().Should().Be(1);

            result.Should().BeOfType<System.Web.Http.Results.OkResult>();
        }

        [Test, Isolated]
        public void InsertMessage_ValidProperty_ShouldAddMessageAnd_2Notifications_If_Consultant()
        {
            // arrange
            var controller = SetUpMessageController();
            controller.MockCurrentUserAndRole(dsc.Id, dsc.UserName, UserRoles.Consultant);

            Order order = BuildOrder(false);
            Context.Orders.Add(order);
            Context.SaveChanges();
            Context.Entry(order).Reload();

            //act            
            var prop = order.HomeFinding.HomeFindingProperties.First();
            Message mess = new Message() { HomeFindingPropertyId = prop.Id };
            MessageDto dto = new MessageDto() { HomeFindingPropertyId = prop.Id, OrderId = order.Id };

            //assert
            var result = controller.UpsertPropertyMessage(dto);
            prop.Messages.Count.Should().Be(1);

            var rl = controller.User.IsInRole(UserRoles.Consultant);
            rl.Should().BeTrue();

            transferee.UserNotifications.Count().Should().Be(1);

            result.Should().BeOfType<System.Web.Http.Results.OkResult>();
        }

        [Test, Isolated]
        public void InsertMessage_ValidProperty_ShouldAddMessageAnd_2Notifications_If_Manager()
        {
            // arrange
            var controller = SetUpMessageController();
            controller.MockCurrentUserAndRole(dsc.Id, dsc.UserName, UserRoles.ProgramManager);

            Order order = BuildOrder(false);
            Context.Orders.Add(order);
            Context.SaveChanges();
            Context.Entry(order).Reload();
            
            //act            
            var prop = order.HomeFinding.HomeFindingProperties.First();
            Message mess = new Message() { HomeFindingPropertyId = prop.Id };
            MessageDto dto = new MessageDto() { HomeFindingPropertyId = prop.Id, OrderId = order.Id };
            
            //assert
            var result = controller.UpsertPropertyMessage(dto);
            prop.Messages.Count.Should().Be(1);

            var rl = controller.User.IsInRole(UserRoles.ProgramManager);
            rl.Should().BeTrue();

            dsc.UserNotifications.Count().Should().Be(1);

            transferee.UserNotifications.Count().Should().Be(1);

            result.Should().BeOfType<System.Web.Http.Results.OkResult>();
        }

        private Order BuildOrder(bool emptyProperties = true)
        {
            Order order = OrderBuilder.New().First();
            order.Transferee = transferee;
            order.ProgramManager = pm;
            order.Consultant = dsc;

            HomeFinding homeFinding = HomeFindingBuilder.New();

            if (emptyProperties)
            {
                // The builder makes a single property, but we want it empty
                homeFinding.HomeFindingProperties = new Collection<HomeFindingProperty>();
            }

            order.HomeFinding = homeFinding;

            return order;
        }
    }
}
