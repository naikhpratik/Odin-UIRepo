using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Microsoft.ApplicationInsights;
using Odin.Data.Core;
using Odin.Data.Core.Dtos;
using Odin.Filters;
using Odin.Interfaces;

namespace Odin.Controllers.Api
{
    public class ManagersController : ApiController
    {
        private readonly IManagerImporter _managerImporter;

        public ManagersController(IManagerImporter managerImporter)
        {
            _managerImporter = managerImporter;
        }

        [HttpPost]
        [Route("api/programmanaers")]
        [ApiAuthorize]
        public IHttpActionResult UpsertManagers(ManagersDto managersDto)
        {
            try
            {
                _managerImporter.ImportManagers(managersDto);
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
