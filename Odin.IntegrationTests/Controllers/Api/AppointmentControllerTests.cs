using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using Odin.Controllers.Api;
using Odin.Data.Builders;
using Odin.Data.Core.Dtos;
using Odin.Data.Helpers;
using Odin.Data.Persistence;
using Odin.Domain;
using Odin.IntegrationTests.Extensions;
using Odin.IntegrationTests.TestAttributes;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Odin.IntegrationTests.Controllers.Api
{
    [TestFixture]
    public class AppointmentControllerTests : WebApiBaseTest
    {
        private AppointmentController SetUpAppointmentController()
        {
            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));
            var mapper = config.CreateMapper();
            var unitOfWork = new UnitOfWork(Context);
            return new AppointmentController(unitOfWork, mapper);
        }
                

        [Test, Isolated]
        public async Task InsertAppointment_ValidOrder_ShouldAddAppointment()
        {
            // arrange
            var order = OrderBuilder.New().First();
            order.TrackingId = TokenHelper.NewToken();
            order.Transferee = transferee;
            order.Consultant = dsc;
            order.ProgramManager = pm;

            Context.Orders.Add(order);
            Context.SaveChanges();
            Context.Entry(order).Reload();

            // Act
            var controller = SetUpAppointmentController();
            controller.MockCurrentUser(dsc.Id, dsc.UserName);
            AppointmentDto dto = new AppointmentDto() { OrderId = order.Id, Id = null, ScheduledDate = DateTime.Now, Description = "Adding a new appointment" };
            var result = controller.UpsertItineraryAppointment(dto);

            // Assert
            Context.Entry(order).Reload();
            order.Appointments.Count.Should().Be(1);
        }

        [Test, Isolated]
        public async Task InsertAppointment_NoOrder_ShouldReturnNotFound()
        {
            // arrange
            var controller = SetUpAppointmentController();
            controller.MockCurrentUser(dsc.Id, dsc.UserName);
            // Act            
            AppointmentDto dto = new AppointmentDto() { OrderId = "-1", Id = null, ScheduledDate = DateTime.Now, Description = "Adding a new appointment" };
            var result = controller.UpsertItineraryAppointment(dto);
            // Assert
            result.Should().BeOfType<System.Web.Http.Results.NotFoundResult>();
        }

        [Test, Isolated]
        public async Task DeleteAppointment_ValidAppointment_ShouldDeleteAppointment()
        {
            // arrange
            var order = OrderBuilder.New().First();
            order.TrackingId = TokenHelper.NewToken();
            order.Transferee = transferee;
            order.Consultant = dsc;
            order.ProgramManager = pm;
            Context.Orders.Add(order);
            Context.SaveChanges();
            Context.Entry(order).Reload();

            // Act
            var controller = SetUpAppointmentController();
            controller.MockCurrentUser(dsc.Id, dsc.UserName);
            var dto = new AppointmentDto() { OrderId = order.Id, Id = null, ScheduledDate = DateTime.Now, Description = "Adding a new appointment" };
            var result = controller.UpsertItineraryAppointment(dto);
            result = controller.DeleteAppointment(order.Appointments.First().Id);

            // Assert
            Context.Entry(order).Reload();
            result.Should().BeOfType<System.Web.Http.Results.OkResult>();
            order.Appointments.Count.Should().Be(0);
        }

        [Test, Isolated]
        public async Task DeleteAppointment_NoAppointment_ShouldReturnNotFound()
        {
            // arrange
            var controller = SetUpAppointmentController();
            controller.MockCurrentUser(dsc.Id, dsc.UserName);
            // Act
            var result = controller.DeleteAppointment("-1");
            // Assert
            result.Should().BeOfType<System.Web.Http.Results.NotFoundResult>();
        }
    }
}
