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

namespace Odin.IntegrationTests.Controllers.Api
{
    [TestFixture]
    public class OrdersControllerTests : WebApiBaseTest
    {
        [Test, Isolated]
        public async Task UpsertOrder_ValidUpdateRequest_ShouldNotUpdateNonDtoFields()
        {
            // Arrange
            var order = OrderBuilder.New().First();
            order.TrackingId = TokenHelper.NewToken();
            order.Transferee = transferee;
            order.Consultant = dsc;
            order.ProgramManager = pm;
            Context.Orders.Add(order);
            var orderDto = OrderDtoBuilder.New();
            orderDto.TrackingId = order.TrackingId;
            orderDto.ProgramManager = ProgramManagerDtoBuilder.New();
            orderDto.ProgramManager.SeContactUid = pm.SeContactUid.Value;
            orderDto.Consultant = ConsultantDtoBuilder.New();
            orderDto.Consultant.SeContactUid = dsc.SeContactUid.Value;
            orderDto.Transferee = TransfereeDtoBuilder.New();
            orderDto.Transferee.Email = transferee.Email;
            var request = CreateRequest("api/orders", "application/json", HttpMethod.Post, orderDto);
            request.Headers.Add("Token", ApiKey);

            // Act
            var response = await Server.HttpClient.SendAsync(request);
            var errorResponse = await response.ReadContentAsyncSafe<ErrorResponse>();

            // Assert
            errorResponse?.Errors.Should().BeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Context.Entry(order).Reload();
            order.FamilyDetails.Should().NotBeNullOrEmpty();
        }

        [Test, Isolated]
        public async Task UpsertOrder_ValidInsertRequest_CreatesOrderRecord()
        {
            // Arrange
            var orderDto = OrderDtoBuilder.New();
            orderDto.ProgramManager = ProgramManagerDtoBuilder.New();
            orderDto.ProgramManager.SeContactUid = pm.SeContactUid.Value;
            orderDto.Transferee = TransfereeDtoBuilder.New();
            orderDto.Transferee.Email = "test@test.com";
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
    }
}
