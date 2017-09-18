using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Odin.Data.Builders;
using Odin.Data.Core.Models;
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
            var transferee = TransfereeBuilder.New().First();
            order.Transferee = transferee;
            Context.Orders.Add(order);
            Context.SaveChanges();
            var orderDto = OrderDtoBuilder.New();
            orderDto.TrackingId = order.TrackingId;
            var request = CreateRequest("api/orders", "application/json", HttpMethod.Post, orderDto);
            request.Headers.Add("Token", ApiKey);

            // Act
            var response = await Client.SendAsync(request);
            var errorResponse = await response.Content.ReadAsAsync<ErrorResponse>();

            // Assert
            errorResponse.Errors.Should().BeNull();
            response.Should().Be(HttpStatusCode.OK);
            Context.Entry(order).Reload();
            order.FamilyDetails.Should().NotBeNullOrEmpty();
        }
    }
}
