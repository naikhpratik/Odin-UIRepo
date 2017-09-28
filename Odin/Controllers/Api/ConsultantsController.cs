using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Odin.Data.Core;
using Odin.Data.Core.Dtos;
using Odin.Filters;
using Odin.Interfaces;

namespace Odin.Controllers.Api
{
    public class ConsultantsController : ApiController
    {
        private readonly IConsultantImporter _consultantImporter;

        public ConsultantsController(IConsultantImporter consultantImporter)
        {
            _consultantImporter = consultantImporter;
        }

        [HttpPost]
        [ApiAuthorize]
        [ValidationActionFilter]
        public IHttpActionResult UpsertConsultants(ConsultantsDto consultantsDto)
        {


            return Ok();
        }
    }
}
