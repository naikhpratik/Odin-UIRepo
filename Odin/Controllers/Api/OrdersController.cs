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
using System.Collections.ObjectModel;
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

        // Post /api/orders/transferee/intake/destination
        [HttpPost]
        [Authorize]
        [Route("api/orders/transferee/intake/destination")]
        public IHttpActionResult UpsertIntakeDestination(OrdersTransfereeIntakeDestinationDto dto)
        {

            var userId = User.Identity.GetUserId();
            //var order = _unitOfWork.Orders.GetOrderFor(userId,dto.Id);
            var order = _unitOfWork.Orders.GetOrderById(dto.Id);

            if (order == null)
            {
                return NotFound();
            }

            _mapper.Map<OrdersTransfereeIntakeDestinationDto, Order>(dto, order);
            _unitOfWork.Complete();
            
            return Ok();
        }

        // Post /api/orders/transferee/intake/origin
        [HttpPost]
        [Authorize]
        [Route("api/orders/transferee/intake/origin")]
        public IHttpActionResult UpsertIntakeOrigin(OrdersTransfereeIntakeOriginDto dto)
        {

            var userId = User.Identity.GetUserId();
            //var order = _unitOfWork.Orders.GetOrderFor(userId,dto.Id);
            var order = _unitOfWork.Orders.GetOrderById(dto.Id);

            if (order == null)
            {
                return NotFound();
            }

            _mapper.Map<OrdersTransfereeIntakeOriginDto, Order>(dto, order);
            _unitOfWork.Complete();

            return Ok();
        }

        // Post /api/orders/transferee/intake/family
        [HttpPost]
        [Authorize]
        [Route("api/orders/transferee/intake/family")]
        public IHttpActionResult UpsertIntakeFamily(OrdersTransfereeIntakeFamilyDto dto)
        {
            var userId = User.Identity.GetUserId();
            //var order = _unitOfWork.Orders.GetOrderFor(userId,dto.Id);
            var order = _unitOfWork.Orders.GetOrderById(dto.Id);


            Collection<Child> newChildren = new Collection<Child>();
            foreach (var childDto in dto.Children)
            {
                var child = order.Children.FirstOrDefault(c => c.Id > 0 && c.Id == childDto.Id);
                if (child == null) 
                {
                    child = _mapper.Map<ChildDto, Child>(childDto);
                    order.Children.Add(child);
                    newChildren.Add(child);
                }
                else
                {
                    _mapper.Map<ChildDto, Child>(childDto, child);
                }
            }

            if (order == null)
            {
                return NotFound();
            }

            _mapper.Map<OrdersTransfereeIntakeFamilyDto, Order>(dto, order);
            _unitOfWork.Complete();

            Dictionary<string,int> newEntities = new Dictionary<string, int>();
            foreach (var child in newChildren)
            {
                newEntities.Add(child.TempId,child.Id);
            }

            //On success, return the newly created id's with the associated temp id's so that the client can be updated.
            return Ok(newEntities);
        }

    }
}
