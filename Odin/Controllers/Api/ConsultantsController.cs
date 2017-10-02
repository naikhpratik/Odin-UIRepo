using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Microsoft.ApplicationInsights;
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
        public async Task<IHttpActionResult> UpsertConsultants(ConsultantsDto consultantsDto)
        {
            try
            {
                await _consultantImporter.ImportConsultants(consultantsDto);
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
