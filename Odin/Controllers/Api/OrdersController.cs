using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Odin.Data.Core.Dtos;
using Odin.Data.Core.Models;
using Odin.Filters;

namespace Odin.Controllers.Api
{
    [Authorize]
    public class OrdersController : ApiController
    {
        // POST /api/orders
        [HttpPost]
        [ApiAuthorize]
        public IHttpActionResult UpsertOrder(OrderDto order)
        {
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
