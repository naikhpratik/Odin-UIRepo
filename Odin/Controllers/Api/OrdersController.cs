using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Odin.Data.Core;
using Odin.Data.Core.Dtos;
using Odin.Data.Core.Models;
using Odin.Filters;

namespace Odin.Controllers.Api
{
    [Authorize]
    public class OrdersController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrdersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // POST /api/orders
        [HttpPost]
        [ApiAuthorize]
        [ValidationActionFilter]
        public IHttpActionResult UpsertOrder(OrderDto orderDto)
        {
            var order = _unitOfWork.Orders.GetOrderByTrackingId(orderDto.TrackingId);

            _mapper.Map<OrderDto, Order>(orderDto, order);

            // Convert OrderDto to order
            // Upsert
            // Determine if consultant is new
            //      If new create user and start process of emailing DSC
            // Lookup Program Manager by secontactuid, assign PM, or throw back error pm does not exist
            // Lookup transferee by email, if new transferee add transferee record.

            return Ok();
        }
    }
}
