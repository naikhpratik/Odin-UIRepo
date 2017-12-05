using System.Web.Mvc;
 using System;
using AutoMapper;
using System.Collections.Generic;    
using System.Net.Mail;
using Odin.ViewModels.Mailers;
using Odin.Helpers;
using Odin.Data.Core;
using Odin.Data.Core.Models;
using Odin.ViewModels.Orders.Transferee;
using Odin.ViewModels.Shared;
using System.Linq;
using System.IO;

namespace Odin.Controllers
{  
    public class EmailController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
               

        public EmailController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        public ActionResult Index(string id)
        {
            Transferee ee = GetTransfereeByOrderId(id);
            var viewModel = new EmailViewModel();
            viewModel.Email = ee.Email;
            viewModel.Name = ee.FullName;
            viewModel.Subject = "Your DwellWorks Itinerary";
            viewModel.Message = "Please find attached your itinerary for the upcoming move";
            return PartialView("~/views/Mailers/Partials/Email.cshtml", viewModel);
        }
        [HttpPost]
        public ActionResult Index(EmailViewModel vm)
        {
            try
            {
                OrdersTransfereeItineraryViewModel viewModel = GetItineraryByOrderId(vm.id);
                viewModel.Id = vm.id;
                viewModel.IsPdf = true;
                Transferee ee = GetTransfereeByOrderId(vm.id);
                viewModel.TransfereeName = ee.FullName;
                string filename = "Itinerary" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
                var pdf = new Rotativa.ViewAsPdf("~/views/Orders/Partials/_Itinerary.cshtml", viewModel) { FileName = filename, PageMargins = new Rotativa.Options.Margins(0, 0, 0, 0) };
                byte[] pdfBytes = pdf.BuildFile(ControllerContext);
                MemoryStream stream = new MemoryStream(pdfBytes);
                var to = ParseAddress(vm.Email);
                if (to == null)
                    return null;
                EmailHelper EH = new EmailHelper();                    
                EH.SendEmail_FS(to, vm.Subject, vm.Message, SendGrid.MimeType.Html, filename, pdfBytes);
                viewModel.IsPdf = false;
                return PartialView("~/views/orders/partials/_Itinerary.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                return null;
            }
        }
        public Transferee GetTransfereeByOrderId(string id)
        {
            return _unitOfWork.Transferees.GetTransfereeByOrderId(id);
        }
        private OrdersTransfereeItineraryViewModel GetItineraryByOrderId(string id)
        {
            var itinService = _unitOfWork.Services.GetServicesByOrderId(id);
            var itinAppointments = _unitOfWork.Appointments.GetAppointmentsByOrderId(id);
            //var itinViewings = _unitOfWork.HousingTypes.GetViewingsByOrderId(id);
            var itinerary1 = _mapper.Map<IEnumerable<Service>, IEnumerable<ItineraryEntryViewModel>>(itinService);
            var itinerary2 = _mapper.Map<IEnumerable<Appointment>, IEnumerable<ItineraryEntryViewModel>>(itinAppointments);
            //var itinerary3 = _mapper.Map<IEnumerable<HousingPropertyViewModel>, IEnumerable<ItineraryEntryViewModel>>(itinViewings);
            //var itinerary = itinerary1.Concat(itinerary2).Concat(itinerary3).OrderBy(s => s.ScheduledDate);
            var itinerary = itinerary1.Concat(itinerary2).OrderBy(s => s.ScheduledDate);
            OrdersTransfereeItineraryViewModel vm = new OrdersTransfereeItineraryViewModel();
            vm.Itinerary = itinerary;
            return vm;
        }
        private static IEnumerable<string> ParseAddress(string addresses)
        {
            addresses = addresses.Replace(",", ";").Replace(" ", ";");
            Char delim = ';';
            string[] newAdds = addresses.Split(delim);
            foreach (string add in newAdds)
            {
                if (add.Contains("@") == false)
                    return null;
            }
            return newAdds;
        }
        public ActionResult Error()
        {
            return View();
        }
    }
}
