using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Odin.Data.Core;
using Odin.Data.Core.Dtos;
using Odin.Data.Core.Models;
using Odin.Data.Persistence;
using Odin.Domain;
using Odin.Interfaces;
using System;
using System.Web;
using System.Web.Http;

namespace Odin.Controllers.Api
{
    public class MessageController : ApiController
    {
        private readonly IOrderImporter _orderImporter;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MessageController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _orderImporter = new OrderImporter(unitOfWork, mapper);
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        
        [HttpPost]
        [Authorize]
        [Route("api/orders/transferee/housing/messages")]
        public IHttpActionResult UpsertPropertyMessage(MessageDto dto)
        {
            var email = User.Identity.Name;
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            ApplicationUser user = userManager.FindByNameAsync(User.Identity.Name).Result;
            //var mgr = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            //var user = mgr.FindByEmail(email);
            var author = user.FullName == " " ? email : user.FullName;
            
            var HomeFindingPropertyId = dto.HomeFindingPropertyId;
            dto.Author = author;
            var property = _unitOfWork.HomeFindingProperties.GetHomeFindingPropertyById(HomeFindingPropertyId);

            if (property == null)
            {
                return NotFound();
            }
            if (dto.Id == null)
            {
                var msg = new Message {
                    Id = Guid.NewGuid().ToString(),
                    HomeFindingPropertyId = dto.HomeFindingPropertyId,
                    MessageDate = dto.MessageDate,
                    Deleted = false,
                    MessageText = dto.MessageText,
                    Author = dto.Author
                };
                property.Messages.Add(msg);
                _unitOfWork.Complete();
                return Ok();
            }
            return NotFound();            
        }
    }
}
