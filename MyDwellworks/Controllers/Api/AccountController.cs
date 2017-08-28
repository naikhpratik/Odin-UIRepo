using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using MyDwellworks.Data.Core;
using MyDwellworks.Data.Core.Dtos;
using MyDwellworks.Data.Core.Models;
using MyDwellworks.Data.Persistence;

namespace MyDwellworks.Controllers.Api
{
    [Authorize]
    public class AccountController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AccountController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [Authorize(Roles = "Global Supply Chain")]
        [Route("api/users")]
        public IHttpActionResult GetUsers()
        {
            var users = _unitOfWork.Users.GetUsersWithRole(UserRoles.GlobalSupplyChain);

            var userDtos = _mapper.Map<UserDto>(users);

            return Ok(userDtos);
        }
    }
}
