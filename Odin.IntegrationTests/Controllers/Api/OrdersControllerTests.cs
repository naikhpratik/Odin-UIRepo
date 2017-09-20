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
            var transferee = Context.Transferees.First(u => u.UserName.Equals("odinee@dwellworks.com"));
            var dsc = Context.Consultants.First(u => u.UserName.Equals("odinconsultant@dwellworks.com"));
            var pm = Context.Managers.First(u => u.UserName.Equals("odinpm@dwellworks.com"));
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
            var request = CreateRequest("api/orders", "application/json", HttpMethod.Post, orderDto);
            request.Headers.Add("Token", ApiKey);

            // Act
            var response = await Client.SendAsync(request);
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
            orderDto.Transferee = TransfereeDtoBuilder.New();
            orderDto.Consultant = ConsultantDtoBuilder.New();

            var request = CreateRequest("api/orders", "application/json", HttpMethod.Post, orderDto);
            request.Headers.Add("Token", ApiKey);

            // Act
            var response = await Client.SendAsync(request);
            var errorResponse = await response.ReadContentAsyncSafe<ErrorResponse>();

            // Assert
            errorResponse?.Errors.Should().BeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var order = Context.Orders.FirstOrDefault(o => o.TrackingId.Equals(orderDto.TrackingId));
            order.Should().NotBeNull();
        }
    }
}
