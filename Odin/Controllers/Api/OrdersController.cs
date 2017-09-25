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

            return Ok("//TODO: ret value");
        }

        [HttpGet]
        public IHttpActionResult GetValue()
        {
            return Ok("test");
        }
    }
}
