using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Odin.Data.Core;
using Odin.Data.Core.Dtos;
using Odin.Filters;

namespace Odin.Controllers.Api
{
    public class ManagersController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ManagersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("api/programmanaers")]
        [ApiAuthorize]
        public IHttpActionResult UpsertManagers(ManagersDto managersDto)
        {

            return Ok();
        }
    }
}
