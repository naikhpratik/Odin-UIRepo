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
using Odin.ViewModels.Shared;
using System.Web.Http;
using System.Collections.Generic;

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
            var author = user.FullName == " " ? email : user.FullName;
            
            var HomeFindingPropertyId = dto.HomeFindingPropertyId;
            dto.Author = author;
            dto.AuthorId = user.Id;
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
                    Author = dto.Author,
                    AuthorId = dto.AuthorId
                };
                property.Messages.Add(msg);
                _unitOfWork.Complete();
                return Ok();
            }
            return NotFound();            
        }
        [HttpPost]
        [Route("api/orders/transferee/housing/markRead/{propertyId}")]
        public IHttpActionResult markMessageRead(string propertyId)
        {
            if (string.IsNullOrEmpty(propertyId) == true)
                return NotFound();
            var property = _unitOfWork.HomeFindingProperties.GetHomeFindingPropertyById(propertyId);           
            string userId = User.Identity.GetUserId();
            bool markedOne = false;
            foreach (Message mess in property.Messages)            
            {
                if (mess.AuthorId != userId && mess.IsRead == false)
                {
                    mess.IsRead = true;
                    markedOne = true;
                }
            };            
            _unitOfWork.Complete();
            if (markedOne == false)
                return  NotFound();
            return Ok();            
        }
    }
}
