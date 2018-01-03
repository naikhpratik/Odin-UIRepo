using AutoMapper;
using Microsoft.AspNet.Identity;
using Odin.Data.Core;
using Odin.Data.Core.Dtos;
using Odin.Data.Core.Models;
using Odin.Domain;
using Odin.Interfaces;
using System;
using System.Web.Http;
using Odin.Extensions;

namespace Odin.Controllers.Api
{
    public class AppointmentController : ApiController
    {
        private readonly IOrderImporter _orderImporter;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AppointmentController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _orderImporter = new OrderImporter(unitOfWork, mapper);
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
               
        [HttpDelete]
        [Authorize]
        [Route("api/orders/transferee/itinerary/appointment/{id}")]
        public IHttpActionResult DeleteAppointment(string Id)
        {
            var userId = User.Identity.GetUserId();
            var appt = _unitOfWork.Appointments.GetAppointmentById(Id);

            if (appt == null)
            {
                return NotFound();
            }
            _unitOfWork.Appointments.Remove(appt);
            _unitOfWork.Complete();
            return Ok();
        }
        [HttpPost]
        [Authorize]
        [Route("api/orders/transferee/itinerary/appointment")]
        public IHttpActionResult UpsertItineraryAppointment(AppointmentDto dto)
        {

            var userId = User.Identity.GetUserId();
            var orderId = dto.OrderId;

            Order order = null;
            if (User.IsInRole(UserRoles.ProgramManager) || User.IsInRole(UserRoles.Consultant))
            {
                order = _unitOfWork.Orders.GetOrderFor(userId, orderId,User.GetUserRole());
            }
           
            if (order == null)
            {
                return NotFound();
            }
            if (dto.Id == null)
            {
                var app = new Appointment { Id = Guid.NewGuid().ToString(), OrderId = dto.OrderId, ScheduledDate = dto.ScheduledDate, Deleted = false, Description = dto.Description };
                order.Appointments.Add(app);
                _unitOfWork.Complete();
                return Ok();
            }
            
            var apptment = _unitOfWork.Appointments.GetAppointmentById(dto.Id);
            if (apptment == null)
            {
                return NotFound();
            }
            else
            {
                _mapper.Map<AppointmentDto, Appointment>(dto, apptment);
            }
            _unitOfWork.Complete();
            return Ok();
        }
    }
}
