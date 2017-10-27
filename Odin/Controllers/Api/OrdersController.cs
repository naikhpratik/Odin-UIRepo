using AutoMapper;
using Microsoft.ApplicationInsights;
using Microsoft.AspNet.Identity;
using Odin.Data.Core;
using Odin.Data.Core.Dtos;
using Odin.Data.Core.Models;
using Odin.Domain;
using Odin.Filters;
using Odin.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Odin.Controllers.Api
{
    public class OrdersController : ApiController
    {
        private readonly IOrderImporter _orderImporter;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrdersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _orderImporter = new OrderImporter(unitOfWork, mapper);
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // POST /api/orders
        [HttpPost]
        [ApiAuthorize]
        [ValidationActionFilter]
        public IHttpActionResult UpsertOrder(OrderDto orderDto)
        {
            try
            {
                _orderImporter.ImportOrder(orderDto);
            }
            catch (Exception e)
            {
                var ai = new TelemetryClient();
                ai.TrackException(e);
                return InternalServerError(e);
            }
            

            return Ok();
        }

        // GET /api/orders
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetOrders()
        {
            var userId = User.Identity.GetUserId();

            var orders = _unitOfWork.Orders.GetOrdersFor(userId);

            var transfereeIndexDtos = _mapper.Map<IEnumerable<Order>, IEnumerable<TransfereeIndexDto>>(orders);

            return Ok(new OrderIndexDto {Transferees = transfereeIndexDtos});
        }

        [HttpPost]
        [Authorize]
        [Route("api/orders/transferee/intake/destination")]
        public IHttpActionResult UpsertIntakeDestination(OrdersTransfereeIntakeDestinationDto dto)
        {

            var userId = User.Identity.GetUserId();

            var order = _unitOfWork.Orders.GetOrderById(dto.Id);

            if (order == null)
            {
                return NotFound();
            }

            _mapper.Map<OrdersTransfereeIntakeDestinationDto, Order>(dto, order);
            _unitOfWork.Complete();
            
            return Ok();
        }

        [HttpPost]
        [Authorize]
        [Route("api/orders/transferee/intake/origin")]
        public IHttpActionResult UpsertIntakeOrigin(OrdersTransfereeIntakeOriginDto dto)
        {

            var userId = User.Identity.GetUserId();

            var order = _unitOfWork.Orders.GetOrderById(dto.Id);

            if (order == null)
            {
                return NotFound();
            }

            _mapper.Map<OrdersTransfereeIntakeOriginDto, Order>(dto, order);
            _unitOfWork.Complete();

            return Ok();
        }

        [HttpPost]
        [Authorize]
        [Route("api/orders/transferee/intake/family")]
        public IHttpActionResult UpsertIntakeFamily(OrdersTransfereeIntakeFamilyDto dto)
        {
            var userId = User.Identity.GetUserId();
            var order = _unitOfWork.Orders.GetOrderById(dto.Id);

            if (order == null)
            {
                return NotFound();
            }

            foreach (var childDto in dto.Children)
            {
                var child = order.Children.FirstOrDefault(c => !String.IsNullOrEmpty(childDto.Id) && c.Id == childDto.Id);
                if (child == null)
                {
                    child = _mapper.Map<ChildDto, Child>(childDto);
                    child.Id = Guid.NewGuid().ToString();
                    order.Children.Add(child);
                }
                else
                {
                    _mapper.Map<ChildDto, Child>(childDto, child);
                }
            }

            foreach (var petDto in dto.Pets)
            {
                var pet = order.Pets.FirstOrDefault(p => !String.IsNullOrEmpty(petDto.Id) && p.Id == petDto.Id);
                if (pet == null)
                {
                    pet = _mapper.Map<PetDto, Pet>(petDto);
                    pet.Id = Guid.NewGuid().ToString();
                    order.Pets.Add(pet);
                }
                else
                {
                    _mapper.Map<PetDto, Pet>(petDto, pet);
                }
            }

            _mapper.Map<OrdersTransfereeIntakeFamilyDto, Order>(dto, order);
            _unitOfWork.Complete();

            //On success, return the newly created id's with the associated temp id's so that the client can be updated.
            return Ok();
        }

        [HttpPost]
        [Authorize]
        [Route("api/orders/transferee/children/{orderId}")]
        public IHttpActionResult InsertChild(string orderId)
        {
            var userId = User.Identity.GetUserId();
            var order = _unitOfWork.Orders.GetOrderById(orderId);

            if (order == null)
            {
                return NotFound();
            }

            var child = new Child { Id = Guid.NewGuid().ToString() };
            order.Children.Add(child);
            _unitOfWork.Complete();

            //On success, return the newly created id.
            return Ok(child.Id);
        }

        [HttpDelete]
        [Authorize]
        [Route("api/orders/transferee/children/{id}")]
        public IHttpActionResult DeleteChild(string id)
        {

            var userId = User.Identity.GetUserId();
            var child = _unitOfWork.Children.GetChildById(id);

            if (child == null)
            {
                return NotFound();
            }
            child.Deleted = true;
            _unitOfWork.Complete();
            return Ok();
        }

        [HttpPost]
        [Authorize]
        [Route("api/orders/transferee/services/{orderId}/{id}")]
        public IHttpActionResult InsertService(string orderId, int id)
        {

            var userId = User.Identity.GetUserId();
            var order = _unitOfWork.Orders.GetOrderById(orderId);

            if (order == null)
            {
                return NotFound();
            }

            var newService = new Service(){ServiceTypeId = id, Selected = true};

            order.Services.Add(newService);

            _unitOfWork.Complete();
            return Ok(newService.Id);
        }

        [HttpPost]
        [Authorize]
        [Route("api/orders/transferee/intake/services")]
        public IHttpActionResult UpsertIntakeServices(OrdersTransfereeIntakeServicesDto dto)
        {

            var userId = User.Identity.GetUserId();
            var order = _unitOfWork.Orders.GetOrderById(dto.Id);

            if (order == null)
            {
                return NotFound();
            }

            foreach (var serviceDto in dto.Services)
            {
                var service = order.Services.FirstOrDefault(s => !String.IsNullOrEmpty(serviceDto.Id) && s.Id == serviceDto.Id);
                if (service == null)
                {
                    service = _mapper.Map<OrdersTransfereeIntakeServiceDto, Service>(serviceDto);
                    service.Id = Guid.NewGuid().ToString();
                    order.Services.Add(service);
                }
                else
                {
                    _mapper.Map<OrdersTransfereeIntakeServiceDto, Service>(serviceDto, service);
                }
            }

            _unitOfWork.Complete();
            return Ok();
        }

        [HttpPost]
        [Authorize]
        [Route("api/orders/transferee/intake/rmc")]
        public IHttpActionResult UpsertIntakeRmc(OrdersTransfereeIntakeRmcDto dto)
        {

            var userId = User.Identity.GetUserId();

            var order = _unitOfWork.Orders.GetOrderById(dto.Id);

            if (order == null)
            {
                return NotFound();
            }

            _mapper.Map<OrdersTransfereeIntakeRmcDto, Order>(dto, order);
            _unitOfWork.Complete();

            return Ok();
        }

        [HttpPost]
        [Authorize]
        [Route("api/orders/transferee/pets/{orderId}")]
        public IHttpActionResult InsertPet(string orderId)
        {
            var userId = User.Identity.GetUserId();
            var order = _unitOfWork.Orders.GetOrderById(orderId);

            if (order == null)
            {
                return NotFound();
            }

            var pet = new Pet { Id = Guid.NewGuid().ToString() };
            order.Pets.Add(pet);
            _unitOfWork.Complete();

            //On success, return the newly created id.
            return Ok(pet.Id);
        }

        [HttpDelete]
        [Authorize]
        [Route("api/orders/transferee/pets/{id}")]
        public IHttpActionResult DeletePet(string id)
        {

            var userId = User.Identity.GetUserId();
            var pet = _unitOfWork.Pets.GetChildById(id);

            if (pet == null)
            {
                return NotFound();
            }
            pet.Deleted = true;
            _unitOfWork.Complete();
            return Ok();
        }

        [HttpPost]
        [Authorize]
        [Route("api/orders/transferee/intake/temphousing")]
        public IHttpActionResult UpsertIntakeTempHousing(OrdersTransfereeIntakeTempHousingDto dto)
        {

            var userId = User.Identity.GetUserId();

            var order = _unitOfWork.Orders.GetOrderById(dto.Id);

            if (order == null)
            {
                return NotFound();
            }

            _mapper.Map<OrdersTransfereeIntakeTempHousingDto, Order>(dto, order);
            _unitOfWork.Complete();

            return Ok();
        }

        [HttpPost]
        [Authorize]
        [Route("api/orders/transferee/intake/rent")]
        public IHttpActionResult UpsertIntakeRent(OrdersTransfereeIntakeRentDto dto)
        {
            var userId = User.Identity.GetUserId();

            var order = _unitOfWork.Orders.GetOrderById(dto.Id);

            if (order == null)
            {
                return NotFound();
            }

            if (order.Rent == null)
            {
                order.Rent = _mapper.Map<OrdersTransfereeIntakeRentDto, Rent>(dto);
            }
            else
            {
                _mapper.Map<OrdersTransfereeIntakeRentDto, Rent>(dto, order.Rent);
            }

            _unitOfWork.Complete();

            return Ok();
        }
    }
}
