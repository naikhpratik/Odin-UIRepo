

using System.Collections.Generic;
using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using Odin.Controllers;
using Odin.Data.Builders;
using Odin.Data.Core.Models;
using Odin.Data.Persistence;
using Odin.IntegrationTests.Extensions;
using Odin.IntegrationTests.TestAttributes;
using Odin.ViewModels.Orders.Transferee;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using Odin.Domain;
using Odin.IntegrationTests.Helpers;
using Microsoft.WindowsAzure.Storage.Blob;
using System;

namespace Odin.IntegrationTests.Controllers
{
    [TestFixture]
    public class HomeFindingPropertiesControllerTests :WebApiBaseTest
    {
        private MapperConfiguration config;
        private IMapper mapper;
        private UnitOfWork unitOfWork;
        private ImageStore imageStore;

        private HomeFindingPropertiesController SetUpHomeFindingPropertiesController()
        {
            config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));
            mapper = config.CreateMapper();
            unitOfWork = new UnitOfWork(Context);
            imageStore = new ImageStore();
            var controller = new HomeFindingPropertiesController(unitOfWork, mapper, imageStore);
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

        [Test, Isolated]
        public async Task InsertProperty_WithPhotos_AddsPhotosToBlobStorage()
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
            var stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("Odin.IntegrationTests.Resources.odin_login.png");
            var postedFile = new HttpPostedFileBaseTesting(stream, "image/png", "odin_login.png");
            var postedFile2 = new HttpPostedFileBaseTesting(stream, "image/png", "odin_login.png");
            propertyVM.UploadedPhotos = new List<HttpPostedFileBase> { postedFile, postedFile2 };

            // Act
            HomeFindingPropertiesController controller = SetUpHomeFindingPropertiesController();
            controller.Index(propertyVM);

            // Assert
            Context.Entry(order).Reload();
            HomeFindingProperty hfp = order.HomeFinding.HomeFindingProperties.First();
            Property property = hfp.Property;

            property.Photos.Count().Should().Be(2);

            Photo propertyPhoto = property.Photos.First();
            ICloudBlob imageReference = imageStore.ImageBlobFor(propertyPhoto.StorageId);

            imageReference.Should().NotBeNull();

            // Cleanup so we don't flood the azure container
            foreach(Photo p in property.Photos)
            {
                ICloudBlob imageBlob = imageStore.ImageBlobFor(p.StorageId);
                imageBlob.Delete();
            }
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
