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
using Odin.Interfaces;

namespace Odin.Controllers.Api
{
    
    public class OrdersController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOrderImporter _orderImporter;

        public OrdersController(IUnitOfWork unitOfWork, IMapper mapper, IOrderImporter orderImporter)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _orderImporter = orderImporter;
        }

        // POST /api/orders
        [HttpPost]
        [ApiAuthorize]
        [ValidationActionFilter]
        public IHttpActionResult UpsertOrder(OrderDto orderDto)
        {
            _orderImporter.ImportOrder(orderDto);
            

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
