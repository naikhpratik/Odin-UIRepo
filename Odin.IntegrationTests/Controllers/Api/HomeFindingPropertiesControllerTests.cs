using AutoMapper;
using NUnit.Framework;
using Odin.Controllers.Api;
using Odin.Data.Persistence;
using Odin.IntegrationTests.TestAttributes;
using Odin.IntegrationTests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Odin.Extensions;
using Odin.Data.Core.Models;
using FluentAssertions;
using System.Net;
using Odin.Data.Builders;
using System.Collections.ObjectModel;
using Odin.ViewModels.Orders.Transferee;

namespace Odin.IntegrationTests.Controllers.Api
{
    [TestFixture]
    public class HomeFindingPropertiesControllerTests :WebApiBaseTest
    {
        private HomeFindingPropertiesController SetUpHomeFindingPropertiesController()
        {
            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));
            var mapper = config.CreateMapper();
            var unitOfWork = new UnitOfWork(Context);
            var controller = new HomeFindingPropertiesController(unitOfWork, mapper);
            controller.MockCurrentUser(dsc.Id, dsc.UserName);
            return controller;
        }

        // Tests
        [Test, Isolated]
        public async Task InsertProperty_ValidInsertRequest_CreatesRecords()
        {
            // Arrange
            Order order = BuildOrder();
            Context.Orders.Add(order);
            Context.SaveChanges();
            Context.Entry(order).Reload();

            HousingPropertyViewModel propertyVM = new HousingPropertyViewModel();
            propertyVM.PropertyStreet1 = "abc";
            propertyVM.PropertyStreet2 = "apt 123";
            propertyVM.OrderId = order.Id;

            // Act
            HomeFindingPropertiesController controller = SetUpHomeFindingPropertiesController();
            controller.Index(propertyVM);

            // Assert
            Context.Entry(order).Reload();
            order.HomeFinding.HomeFindingProperties.Count().Should().Be(1);

            HomeFindingProperty hfp = order.HomeFinding.HomeFindingProperties.First();
            Property property = hfp.Property;
            property.Street1.Should().Be(propertyVM.PropertyStreet1);
            property.Street2.Should().Be(propertyVM.PropertyStreet2);
        }

        private Order BuildOrder()
        {
            Order order = OrderBuilder.New().First();
            order.Transferee = transferee;
            order.ProgramManager = pm;
            order.Consultant = dsc;

            HomeFinding homeFinding = HomeFindingBuilder.New();
            // The builder makes a single property, but we want it empty
            homeFinding.HomeFindingProperties = new Collection<HomeFindingProperty>();
            order.HomeFinding = homeFinding;

            return order;
        }
    }
}
