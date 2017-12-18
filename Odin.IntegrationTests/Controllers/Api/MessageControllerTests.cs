
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
        public async Task InsertMessage_ValidProperty_ShouldAddMessage()
        {
            // Arrange
            Order order = BuildOrder(false);
            Context.Orders.Add(order);
            Context.SaveChanges();              

            // Act
            var controller = SetUpMessageController();
            controller.MockCurrentUser(dsc.Id, dsc.UserName);
            HomeFindingProperty property = order.HomeFinding.HomeFindingProperties.First();
            MessageDto dto = new MessageDto() { HomeFindingPropertyId = property.Id, Id = null, MessageDate = DateTime.Now, MessageText = "Adding a new Message", Deleted=false};
            var result = controller.UpsertPropertyMessage(dto);

            // Assert
            property.Messages.Count.Should().Be(1);
        }

        [Test, Isolated]
        public async Task InsertMessage_NoOrder_ShouldReturnNotFound()
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
