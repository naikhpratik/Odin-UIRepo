using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using MyDwellworks.Data.Core;

namespace MyDwellworks.Controllers.Api
{
    [Authorize]
    public class AccountController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AccountController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IHttpActionResult GetUsers()
        {


            return Ok();
        }
    }
}
