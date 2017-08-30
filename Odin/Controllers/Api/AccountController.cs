using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using Odin.Data.Core;
using Odin.Data.Core.Dtos;
using Odin.Data.Core.Models;

namespace Odin.Controllers.Api
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
            var users = _unitOfWork.Users.GetUsersWithRole(UserRoles.Consultant);

            var userDtos = _mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<UserDto>>(users);

            return Ok(userDtos);
        }
    }
}
