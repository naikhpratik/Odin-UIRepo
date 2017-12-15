
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
                

        //[Test, Isolated]
        //public async Task InsertMessage_ValidProperty_ShouldAddMessage()
        //{
        //    // Arrange
        //    Order order = BuildOrder();
        //    Context.Orders.Add(order);
        //    Context.SaveChanges();
        //    Context.Entry(order).Reload();

        //    HomeFindingProperty property = new HomeFindingProperty();
        //    property.Deleted = false;            
        //    Context.HomeFindingProperties.Add(property);
        //    Context.SaveChanges();
        //    Context.Entry(property).Reload();

        //    // Act
        //    var controller = SetUpMessageController();
        //    controller.MockCurrentUser(dsc.Id, dsc.UserName);
        //    MessageDto dto = new MessageDto() { PropertyId = property.Id, Id = null, MessageDate = DateTime.Now, MessageText = "Adding a new Message", Deleted=false};
        //    var result = controller.UpsertPropertyMessage(dto);

        //    // Assert
        //    Context.Entry(property).Reload();
        //    property.Messages.Count.Should().Be(1);
        //}

        //[Test, Isolated]
        //public async Task InsertMessage_NoOrder_ShouldReturnNotFound()
        //{
        //    // arrange
        //    var controller = SetUpMessageController();
        //    controller.MockCurrentUser(dsc.Id, dsc.UserName);
        //    // Act            
        //    MessageDto dto = new MessageDto() { PropertyId = "-1", Id = null, MessageDate = DateTime.Now, MessageText = "Adding a new Message" };
        //    var result = controller.UpsertPropertyMessage(dto);
        //    // Assert
        //    result.Should().BeOfType<System.Web.Http.Results.NotFoundResult>();
        //}
        //private Order BuildOrder(bool emptyProperties = true)
        //{
        //    Order order = OrderBuilder.New().First();
        //    order.Transferee = transferee;
        //    order.ProgramManager = pm;
        //    order.Consultant = dsc;

        //    HomeFinding homeFinding = HomeFindingBuilder.New();

        //    if (emptyProperties)
        //    {
        //        // The builder makes a single property, but we want it empty
        //        homeFinding.HomeFindingProperties = new Collection<HomeFindingProperty>();
        //    }

        //    order.HomeFinding = homeFinding;

        //    return order;
        //}
    }
}
