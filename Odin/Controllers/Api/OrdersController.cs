using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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
        public IHttpActionResult UpsertOrder(Order order)
        {


            return Ok();
        }
    }
}
