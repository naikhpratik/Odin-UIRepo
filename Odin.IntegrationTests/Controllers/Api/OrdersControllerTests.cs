using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.Testing;
using NUnit.Framework;
using Odin.Data.Builders;
using Odin.Data.Core.Dtos;
using Odin.Data.Core.Models;
using Odin.Data.Helpers;
using Odin.Extensions;
using Odin.Helpers;
using Odin.IntegrationTests.TestAttributes;

namespace Odin.IntegrationTests.Controllers.Api
{
    [TestFixture]
    public class OrdersControllerTests : WebApiBaseTest
    {
        [Test, CleanData]
        public async Task UpsertOrder_ValidUpdateRequest_ShouldNotUpdateNonDtoFields()
        {
            // Arrange
            var order = OrderBuilder.New().First();
            order.TrackingId = TokenHelper.NewToken();
            order.Transferee = transferee;
            order.Consultant = dsc;
            order.ProgramManager = pm;
            Context.Orders.Add(order);
            Context.SaveChanges();
            var orderDto = OrderDtoBuilder.New();
            orderDto.TrackingId = order.TrackingId;
            orderDto.ProgramManager = ProgramManagerDtoBuilder.New();
            orderDto.ProgramManager.SeContactUid = pm.SeContactUid.Value;
            orderDto.Consultant = ConsultantDtoBuilder.New();
            orderDto.Consultant.SeContactUid = dsc.SeContactUid.Value;
            orderDto.Transferee = TransfereeDtoBuilder.New();
            orderDto.Transferee.Email = transferee.Email;

            // Act
            var request = CreateRequest("api/orders", "application/json", HttpMethod.Post, orderDto);
            request.Headers.Add("Token", ApiKey);
            var response = await Server.HttpClient.SendAsync(request);
            var errorResponse = await response.ReadContentAsyncSafe<ErrorResponse>();

            // Assert
            errorResponse?.Errors.Should().BeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Context.Entry(order).Reload();
            order.FamilyDetails.Should().NotBeNullOrEmpty();
        }

        [Test, CleanData]
        public async Task UpsertOrder_ValidInsertRequest_CreatesOrderRecord()
        {
            // Arrange
            var orderDto = OrderDtoBuilder.New();
            orderDto.ProgramManager = ProgramManagerDtoBuilder.New();
            orderDto.ProgramManager.SeContactUid = pm.SeContactUid.Value;
            orderDto.Transferee = TransfereeDtoBuilder.New();
            orderDto.Transferee.Email = "integration@test.com";
            orderDto.Consultant = ConsultantDtoBuilder.New();
            orderDto.Consultant.SeContactUid = dsc.SeContactUid.Value;

            // Act                
            var request = CreateRequest("api/orders", "application/json", HttpMethod.Post, orderDto);
            request.Headers.Add("Token", ApiKey);
            var response = await Server.HttpClient.SendAsync(request);
            var errorResponse = await response.ReadContentAsyncSafe<ErrorResponse>();
                
            // Assert
            errorResponse?.Errors.Should().BeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var order = Context.Orders.FirstOrDefault(o => o.TrackingId.Equals(orderDto.TrackingId));
            order.Should().NotBeNull();
          
        }

        [Test, CleanData]
        public async Task UpsertOrder_InsertWithNewTransferee_CreatesTransfereeAndOrder()
        {
            // Arrange
            var orderDto = OrderDtoBuilder.New();
            orderDto.ProgramManager = ProgramManagerDtoBuilder.New();
            orderDto.ProgramManager.SeContactUid = pm.SeContactUid.Value;
            orderDto.Transferee = TransfereeDtoBuilder.New();
            orderDto.Transferee.Email = "integration@test.com";
            orderDto.Consultant = ConsultantDtoBuilder.New();
            orderDto.Consultant.SeContactUid = dsc.SeContactUid.Value;
            Context.Transferees.SingleOrDefault(t => t.Email.Equals("integration@test.com")).Should().BeNull();
            Context.Orders.SingleOrDefault(o => o.TrackingId.Equals(orderDto.TrackingId)).Should().BeNull();

            // Act
            var request = CreateRequest("api/orders", "application/json", HttpMethod.Post, orderDto);
            request.Headers.Add("Token", ApiKey);
            var response = await Server.HttpClient.SendAsync(request);
            var errorResponse = await response.ReadContentAsyncSafe<ErrorResponse>();

            // Assert
            errorResponse?.Errors.Should().BeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var newTransferee = Context.Transferees.SingleOrDefault(t => t.Email.Equals("integration@test.com"));
            newTransferee.Should().NotBeNull();
            var newOrder = Context.Orders.SingleOrDefault(o => o.TrackingId.Equals(orderDto.TrackingId));
            newOrder.Should().NotBeNull();
        }

        [Test, CleanData]
        public async Task UpsertOrder_InsertWithExistingTransferee_CreatesOrder()
        {
            // Arrange
            var orderDto = OrderDtoBuilder.New();
            orderDto.ProgramManager = ProgramManagerDtoBuilder.New();
            orderDto.ProgramManager.SeContactUid = pm.SeContactUid.Value;
            orderDto.Transferee = TransfereeDtoBuilder.New();
            orderDto.Transferee.Email = transferee.Email;
            orderDto.Consultant = ConsultantDtoBuilder.New();
            orderDto.Consultant.SeContactUid = dsc.SeContactUid.Value;
            Context.Orders.SingleOrDefault(o => o.TrackingId.Equals(orderDto.TrackingId)).Should().BeNull();

            // Act
            var request = CreateRequest("api/orders", "application/json", HttpMethod.Post, orderDto);
            request.Headers.Add("Token", ApiKey);
            var response = await Server.HttpClient.SendAsync(request);
            var errorResponse = await response.ReadContentAsyncSafe<ErrorResponse>();

            // Assert
            errorResponse?.Errors.Should().BeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Context.Orders.SingleOrDefault(o => o.TrackingId.Equals(orderDto.TrackingId)).Should().NotBeNull();
        }

        [Test, CleanData]
        public async Task UpsertOrder_UpdateOrder_ShouldUpdateFields()
        {
            // Arrange
            var order = OrderBuilder.New().First();
            order.TrackingId = TokenHelper.NewToken();
            order.Transferee = transferee;
            order.Consultant = dsc;
            order.ProgramManager = pm;
            order.DestinationCity = "test-before-insert";
            Context.Orders.Add(order);
            Context.SaveChanges();
            var orderDto = OrderDtoBuilder.New();
            orderDto.TrackingId = order.TrackingId;
            orderDto.ProgramManager = ProgramManagerDtoBuilder.New();
            orderDto.ProgramManager.SeContactUid = pm.SeContactUid.Value;
            orderDto.Consultant = ConsultantDtoBuilder.New();
            orderDto.Consultant.SeContactUid = dsc.SeContactUid.Value;
            orderDto.Transferee = TransfereeDtoBuilder.New();
            orderDto.Transferee.Email = transferee.Email;
            orderDto.DestinationCity = "integration-test-city";

            // Act
            var request = CreateRequest("api/orders", "application/json", HttpMethod.Post, orderDto);
            request.Headers.Add("Token", ApiKey);
            var response = await Server.HttpClient.SendAsync(request);
            var errorResponse = await response.ReadContentAsyncSafe<ErrorResponse>();

            // Assert
            errorResponse?.Errors.Should().BeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Context.Entry(order).Reload();
            order.DestinationCity.Should().Be(orderDto.DestinationCity);
        }

        [Test, CleanData]
        public async Task UpsertOrder_UpdateOrderWithNewEeEmail_ShouldCreateNewTransferee()
        {
            // Arrange
            var order = OrderBuilder.New().First();
            order.TrackingId = TokenHelper.NewToken();
            order.Transferee = transferee;
            order.Consultant = dsc;
            order.ProgramManager = pm;
            Context.Orders.Add(order);
            Context.SaveChanges();
            var orderDto = OrderDtoBuilder.New();
            orderDto.TrackingId = order.TrackingId;
            orderDto.ProgramManager = ProgramManagerDtoBuilder.New();
            orderDto.ProgramManager.SeContactUid = pm.SeContactUid.Value;
            orderDto.Consultant = ConsultantDtoBuilder.New();
            orderDto.Consultant.SeContactUid = dsc.SeContactUid.Value;
            orderDto.Transferee = TransfereeDtoBuilder.New();
            orderDto.Transferee.Email = "integration@test.com";
            Context.Transferees.SingleOrDefault(t => t.Email.Equals(orderDto.Transferee.Email)).Should().BeNull();

            // Act
            var request = CreateRequest("api/orders", "application/json", HttpMethod.Post, orderDto);
            request.Headers.Add("Token", ApiKey);
            var response = await Server.HttpClient.SendAsync(request);
            var errorResponse = await response.ReadContentAsyncSafe<ErrorResponse>();

            // Assert
            errorResponse?.Errors.Should().BeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Context.Transferees.SingleOrDefault(t => t.Email.Equals(orderDto.Transferee.Email)).Should().NotBeNull();
        }

        [Test, CleanData]
        public async Task UpsertOrder_DifferentPm_ShouldChangePmAssigned()
        {
            // Arrange
            var secondPm = Context.Managers.Add(new Manager
            {
                UserName = "integrationee@dwellworks.com",
                FirstName = "user2",
                Email = "integrationee@dwellworks.com",
                PasswordHash = "-",
                SeContactUid = 99999
            });
            var order = OrderBuilder.New().First();
            order.TrackingId = TokenHelper.NewToken();
            order.Transferee = transferee;
            order.Consultant = dsc;
            order.ProgramManager = pm;
            Context.Orders.Add(order);
            Context.SaveChanges();
            var orderDto = OrderDtoBuilder.New();
            orderDto.TrackingId = order.TrackingId;
            orderDto.ProgramManager = ProgramManagerDtoBuilder.New();
            orderDto.ProgramManager.SeContactUid = secondPm.SeContactUid.Value;
            orderDto.Consultant = ConsultantDtoBuilder.New();
            orderDto.Consultant.SeContactUid = dsc.SeContactUid.Value;
            orderDto.Transferee = TransfereeDtoBuilder.New();
            orderDto.Transferee.Email = transferee.Email;
            Context.Entry(order).Reload();
            order.ProgramManager.SeContactUid.Value.Should().Be(pm.SeContactUid.Value);

            // Act
            var request = CreateRequest("api/orders", "application/json", HttpMethod.Post, orderDto);
            request.Headers.Add("Token", ApiKey);
            var response = await Server.HttpClient.SendAsync(request);
            var errorResponse = await response.ReadContentAsyncSafe<ErrorResponse>();

            // Assert
            errorResponse?.Errors.Should().BeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Context.Entry(order).Reload();
            order.ProgramManager.SeContactUid.Value.Should().Be(secondPm.SeContactUid.Value);
        }

        [Test, CleanData]
        public async Task UpsertOrder_DifferentDsc_ShouldChangeDscAssigned()
        {
            // Arrange
            var secondConsultant = Context.Consultants.Add(new Consultant()
            {
                UserName = "integrationc@dwellworks.com",
                FirstName = "user2",
                Email = "integrationc@dwellworks.com",
                PasswordHash = "-",
                SeContactUid = 99999
            });
            var order = OrderBuilder.New().First();
            order.TrackingId = TokenHelper.NewToken();
            order.Transferee = transferee;
            order.Consultant = dsc;
            order.ProgramManager = pm;
            Context.Orders.Add(order);
            Context.SaveChanges();
            var orderDto = OrderDtoBuilder.New();
            orderDto.TrackingId = order.TrackingId;
            orderDto.ProgramManager = ProgramManagerDtoBuilder.New();
            orderDto.ProgramManager.SeContactUid = pm.SeContactUid.Value;
            orderDto.Consultant = ConsultantDtoBuilder.New();
            orderDto.Consultant.SeContactUid = secondConsultant.SeContactUid.Value;
            orderDto.Transferee = TransfereeDtoBuilder.New();
            orderDto.Transferee.Email = transferee.Email;
            Context.Entry(order).Reload();
            order.Consultant.SeContactUid.Value.Should().Be(dsc.SeContactUid.Value);

            // Act
            var request = CreateRequest("api/orders", "application/json", HttpMethod.Post, orderDto);
            request.Headers.Add("Token", ApiKey);
            var response = await Server.HttpClient.SendAsync(request);
            var errorResponse = await response.ReadContentAsyncSafe<ErrorResponse>();

            // Assert
            errorResponse?.Errors.Should().BeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Context.Entry(order).Reload();
            order.Consultant.SeContactUid.Value.Should().Be(secondConsultant.SeContactUid.Value);
        }


        [Test, CleanData]
        public async Task UpsertOrder_IncorrectToken_Returns401()
        {
            // Arrange
            var orderDto = OrderDtoBuilder.New();
            orderDto.TrackingId = TokenHelper.NewToken();
            orderDto.ProgramManager = ProgramManagerDtoBuilder.New();
            orderDto.ProgramManager.SeContactUid = pm.SeContactUid.Value;
            orderDto.Consultant = ConsultantDtoBuilder.New();
            orderDto.Consultant.SeContactUid = dsc.SeContactUid.Value;
            orderDto.Transferee = TransfereeDtoBuilder.New();
            orderDto.Transferee.Email = transferee.Email;

            // Act
            var request = CreateRequest("api/orders", "application/json", HttpMethod.Post, orderDto);
            request.Headers.Add("Token", "--IncorrectToken--");
            var response = await Server.HttpClient.SendAsync(request);
            var errorResponse = await response.ReadContentAsyncSafe<ErrorResponse>();

            // Assert
            errorResponse.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            errorResponse?.Errors.First().Should().Contain("authorized");
        }

        //TODO: UpsertOrder_OnException_Returns501()
        //TODO: UpsertOrder_DtoMissingFields_ReturnsArrayOfErrors()
    }
}
