using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Microsoft.ApplicationInsights;
using Microsoft.AspNet.Identity;
using Odin.Data.Core;
using Odin.Data.Core.Dtos;
using Odin.Data.Core.Models;
using Odin.Domain;
using Odin.Filters;
using Odin.Interfaces;

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
            
    }
}
