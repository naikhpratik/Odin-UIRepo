using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MyDwellworks.Data.Core;

namespace MyDwellworks.Controllers.Api
{
    [Authorize]
    public class AccountController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IHttpActionResult GetUsers()
        {


            return Ok();
        }
    }
}
