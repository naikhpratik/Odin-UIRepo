using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Microsoft.ApplicationInsights;
using Odin.Data.Core;
using Odin.Data.Core.Dtos;
using Odin.Data.Core.Models;
using Odin.Filters;
using Odin.Interfaces;

namespace Odin.Controllers.Api
{
    public class OrdersController : ApiController
    {
        private readonly IOrderImporter _orderImporter;

        public OrdersController(IOrderImporter orderImporter)
        {
            _orderImporter = orderImporter;
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
    }
}
