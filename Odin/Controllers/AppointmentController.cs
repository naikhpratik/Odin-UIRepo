using AutoMapper;
using System.Web.Http;
using Odin.Data.Core;
using Odin.Data.Core.Models;
using System;
using System.Web.Mvc;

namespace Odin.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AppointmentController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public ActionResult AppointmentPartial(string id)
        {
            Appointment viewModel;
            if (string.IsNullOrEmpty(id) == false)
            {
                viewModel = GetAppointmentById(id);
                if (viewModel == null)
                    return HttpNotFound();
                viewModel.Id = id;
            }
            else
            {
                viewModel = new Appointment() { Id = null, ScheduledDate = DateTime.Now };
            }
            return PartialView("~/views/orders/partials/_Appointment.cshtml", viewModel);
        }
        private Appointment GetAppointmentById(string Id)
        {
            var itinAppointment = _unitOfWork.Appointments.GetAppointmentById(Id);
            return itinAppointment;
        }
      
    }
}