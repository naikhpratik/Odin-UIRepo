using AutoMapper;
using Microsoft.AspNet.Identity;
using Odin.Data.Core;
using Odin.Data.Core.Dtos;
using Odin.Data.Core.Models;
using Odin.Domain;
using Odin.Interfaces;
using System;
using System.Web.Http;
using System.Security.Claims;

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
            var property = _unitOfWork.HomeFindingProperties.GetHomeFindingPropertyById(dto.HomeFindingPropertyId);
            if (property == null)
            {
                return NotFound();
            }            
            var fullNameClaim = ((ClaimsIdentity)User.Identity).FindFirst("FullName");
            string author = "";
            if (fullNameClaim != null)
                author = fullNameClaim.Value;

            dto.Author = author;
            dto.AuthorId = User.Identity.GetUserId();
            
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
                Notification notification = new Notification()
                    {
                        NotificationType = NotificationType.MessageCreated,
                        Message = "A New message was composed",
                        Title = "New Message",
                        OrderId = dto.OrderId
                    };
                if (User.IsInRole(UserRoles.ProgramManager))
                {                    
                    var order = _unitOfWork.Orders.GetOrderById(dto.OrderId);
                    int seId = (int)(order.Consultant.SeContactUid == null ? 0 : order.Consultant.SeContactUid);
                    Consultant consultant = _unitOfWork.Consultants.GetConsultantBySeContactUid(seId);
                    consultant.Notify(notification);
                    var ee = order.Transferee;
                    ee.Notify(notification);
                }
                else if (User.IsInRole(UserRoles.Consultant))
                {
                    var order = _unitOfWork.Orders.GetOrderById(dto.OrderId);
                    var ee = order.Transferee;
                    ee.Notify(notification);
                }
                else if (User.IsInRole(UserRoles.Transferee))
                {                   
                    var order = _unitOfWork.Orders.GetOrderById(dto.OrderId);
                    int seId = (int)(order.Consultant.SeContactUid == null ? 0 : order.Consultant.SeContactUid);
                    Consultant consultant = _unitOfWork.Consultants.GetConsultantBySeContactUid(seId);
                    consultant.Notify(notification);
                }
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
