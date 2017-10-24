using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using Odin.Controllers.Api;
using Odin.Data.Builders;
using Odin.Data.Core.Dtos;
using Odin.Data.Core.Models;
using Odin.Data.Helpers;
using Odin.Data.Persistence;
using Odin.Extensions;
using Odin.IntegrationTests.Extensions;
using Odin.IntegrationTests.TestAttributes;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace Odin.IntegrationTests.Controllers.Api
{
    [TestFixture]
    public class OrdersControllerTests : WebApiBaseTest
    {
        private OrdersController SetUpOrdersController()
        {
            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));
            var mapper = config.CreateMapper();
            var unitOfWork = new UnitOfWork(Context);
            return new OrdersController(unitOfWork, mapper);
        }

        [Test, Isolated]
        public async Task GetOrders_ValidRequests_ShouldReturnOrders()
        {
            // Arrange
            var orders = OrderBuilder.New(2);
            orders.ForEach(o => o.ConsultantId = dsc.Id);
            orders.ForEach(o => o.TransfereeId = transferee.Id);
            orders.ForEach(o => o.ProgramManagerId = pm.Id);
            orders.ForEach(o => o.TrackingId = TokenHelper.NewToken());
            var testDateTime = new DateTime(1999, 6, 14);
            orders.ForEach(o => o.PreTripDate = testDateTime);
            try
            {
                Context.Orders.AddRange(orders);
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            var controller = SetUpOrdersController();
            controller.MockCurrentUser(dsc.Id, dsc.UserName);

            // Act
            var result = controller.GetOrders();
            var resultContent = result.GetContent<OrderIndexDto>();

            // Assert
            result.Should().BeOfType<OkNegotiatedContentResult<OrderIndexDto>>();
            resultContent.Transferees.Should().NotBeNull();
            var newTransferees = resultContent.Transferees.Where(t => t.PreTrip == testDateTime);
            newTransferees.Should().HaveCount(2);
        }

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
            errorResponse?.errors.Should().BeNull();
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
            errorResponse?.errors.Should().BeNull();
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
            errorResponse?.errors.Should().BeNull();
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
            errorResponse?.errors.Should().BeNull();
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
            errorResponse?.errors.Should().BeNull();
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
            errorResponse?.errors.Should().BeNull();
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
            errorResponse?.errors.Should().BeNull();
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
            errorResponse?.errors.Should().BeNull();
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
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            //errorResponse.errors.Should().NotBeNull();
            //errorResponse?.errors?.First().Should().Contain("authorized");
        }

        //TODO: UpsertOrder_OnException_Returns501()
        //TODO: UpsertOrder_DtoMissingFields_ReturnsArrayOfErrors()

        [Test, CleanData]
        public async Task UpsertOrder_ExistingOrderNewService_ShouldAddServiceToOrder()
        {
            var order = OrderBuilder.New().First();
            order.TrackingId = TokenHelper.NewToken();
            order.Transferee = transferee;
            order.Consultant = dsc;
            order.ProgramManager = pm;
            order.DestinationCity = "integration city";
            Context.Orders.Add(order);
            Context.SaveChanges();

            ServiceType serviceType = Context.ServiceTypes.First();
            ServiceDto serviceDto = new ServiceDto { ServiceTypeId = serviceType.Id };

            var orderDto = OrderDtoBuilder.New();
            orderDto.TrackingId = order.TrackingId;
            orderDto.ProgramManager = ProgramManagerDtoBuilder.New();
            orderDto.ProgramManager.SeContactUid = pm.SeContactUid.Value;
            orderDto.Consultant = ConsultantDtoBuilder.New();
            orderDto.Consultant.SeContactUid = dsc.SeContactUid.Value;
            orderDto.Transferee = TransfereeDtoBuilder.New();
            orderDto.Transferee.Email = transferee.Email;
            orderDto.Services.Add(serviceDto);

            // Act
            var request = CreateRequest("api/orders", "application/json", HttpMethod.Post, orderDto);
            request.Headers.Add("Token", ApiKey);
            var response = await Server.HttpClient.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Context.Entry(order).Reload();
            Context.Entry(order).Collection(o => o.Services).Load();
            order.Services.Count.Should().Be(1);
            order.Services.First().ServiceTypeId.Should().Be(serviceType.Id);
        }

        [Test, CleanData]
        public async Task UpsertOrder_ExistingOrderDuplicateService_ShouldNotAddServiceToOrder()
        {
            ServiceType serviceType = Context.ServiceTypes.First();            

            Service service = new Service()
            {
                ServiceType = serviceType,
                ServiceTypeId = serviceType.Id,
                ScheduledDate = DateTime.Now,
                CompletedDate = DateTime.Now,
            };

            ServiceDto serviceDto = new ServiceDto { ServiceTypeId = serviceType.Id };


            var order = OrderBuilder.New().First();
            order.TrackingId = TokenHelper.NewToken();
            order.Transferee = transferee;
            order.Consultant = dsc;
            order.ProgramManager = pm;
            order.Services.Add(service);
            order.DestinationCity = "integration city";
            Context.Orders.Add(order);
            Context.SaveChanges();

            Context.Entry(order).Reload();
            Context.Entry(order).Collection(o => o.Services).Load();
            order.Services.Count.Should().Be(1);

            var orderDto = OrderDtoBuilder.New();
            orderDto.TrackingId = order.TrackingId;
            orderDto.ProgramManager = ProgramManagerDtoBuilder.New();
            orderDto.ProgramManager.SeContactUid = pm.SeContactUid.Value;
            orderDto.Consultant = ConsultantDtoBuilder.New();
            orderDto.Consultant.SeContactUid = dsc.SeContactUid.Value;
            orderDto.Transferee = TransfereeDtoBuilder.New();
            orderDto.Transferee.Email = transferee.Email;
            orderDto.Services.Add(serviceDto);

            // Act
            var request = CreateRequest("api/orders", "application/json", HttpMethod.Post, orderDto);
            request.Headers.Add("Token", ApiKey);
            var response = await Server.HttpClient.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Context.Entry(order).Reload();
            Context.Entry(order).Collection(o => o.Services).Load();
            order.Services.Count.Should().Be(1);
            order.Services.First().CompletedDate.HasValue.Should().Be(true);
            order.Services.First().ServiceTypeId.Should().Be(serviceType.Id);
        }

        [Test, CleanData]
        public async Task UpsertOrder_ExistingOrderInvalidService_ShouldReturnErrorsAndNotAddToOrder()
        {
            ServiceType serviceType = Context.ServiceTypes.OrderByDescending(st => st.Id).First();

            ServiceDto serviceDto = new ServiceDto { ServiceTypeId = serviceType.Id + 500 }; //Make invalid id

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
            orderDto.Services.Add(serviceDto);

            // Act
            var request = CreateRequest("api/orders", "application/json", HttpMethod.Post, orderDto);
            request.Headers.Add("Token", ApiKey);
            var response = await Server.HttpClient.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            Context.Entry(order).Reload();
            Context.Entry(order).Collection(o => o.Services).Load();
            order.Services.Count.Should().Be(0);
        }


        [Test, CleanData]
        public async Task UpsertOrder_NewOrderNewService_ShouldAddServiceToOrder()
        {
            
            ServiceType serviceType = Context.ServiceTypes.First();
            ServiceDto serviceDto = new ServiceDto { ServiceTypeId = serviceType.Id };

            var orderDto = OrderDtoBuilder.New();
            orderDto.TrackingId = TokenHelper.NewToken();
            orderDto.ProgramManager = ProgramManagerDtoBuilder.New();
            orderDto.ProgramManager.SeContactUid = pm.SeContactUid.Value;
            orderDto.Consultant = ConsultantDtoBuilder.New();
            orderDto.Consultant.SeContactUid = dsc.SeContactUid.Value;
            orderDto.Transferee = TransfereeDtoBuilder.New();
            orderDto.Transferee.Email = transferee.Email;
            orderDto.Services.Add(serviceDto);

            // Act
            var request = CreateRequest("api/orders", "application/json", HttpMethod.Post, orderDto);
            request.Headers.Add("Token", ApiKey);
            var response = await Server.HttpClient.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var order = Context.Orders.Where(o => o.TrackingId == orderDto.TrackingId).Include(o => o.Services).Single();
            order.Services.Count.Should().Be(1);
            order.Services.First().ServiceTypeId.Should().Be(serviceType.Id);
        }

        [Test, CleanData]
        public async Task UpsertOrder_NewOrderInvalidService_ShouldReturnErrorsAndNotCreateOrder()
        {
            ServiceType serviceType = Context.ServiceTypes.OrderByDescending(st => st.Id).First();
            ServiceDto serviceDto = new ServiceDto { ServiceTypeId = serviceType.Id + 500 }; //Make invalid id

            var orderDto = OrderDtoBuilder.New();
            orderDto.TrackingId = TokenHelper.NewToken();
            orderDto.ProgramManager = ProgramManagerDtoBuilder.New();
            orderDto.ProgramManager.SeContactUid = pm.SeContactUid.Value;
            orderDto.Consultant = ConsultantDtoBuilder.New();
            orderDto.Consultant.SeContactUid = dsc.SeContactUid.Value;
            orderDto.Transferee = TransfereeDtoBuilder.New();
            orderDto.Transferee.Email = transferee.Email;
            orderDto.Services.Add(serviceDto);

            // Act
            var request = CreateRequest("api/orders", "application/json", HttpMethod.Post, orderDto);
            request.Headers.Add("Token", ApiKey);
            var response = await Server.HttpClient.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            var order = Context.Orders.Where(o => o.TrackingId == orderDto.TrackingId).Include(o => o.Services).SingleOrDefault();
            order.Should().BeNull();
        }

    }
}
