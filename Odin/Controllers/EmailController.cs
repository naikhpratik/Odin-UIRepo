using System.Web.Mvc;
using Odin.Interfaces;
using System.Net;
using AutoMapper;
using System.Collections.Generic;    
using System.Net.Mail;
using Odin.ViewModels.Mailers;
using Odin.Helpers;
using Odin.Data.Core;
using Odin.Data.Core.Models;
using Odin.ViewModels.Orders.Transferee;
using System;
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
            if (ee == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Not found");
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
                OrdersTransfereeItineraryViewModel viewModel = BuildItineraryByOrderId(vm.id);
                viewModel.Id = vm.id;
                viewModel.IsPdf = true;
                var to = ParseAddress(vm.Email);
                if (to == null)
                    return null;
                Transferee ee = GetTransfereeByOrderId(vm.id);
                if (ee == null)
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Not found");
                viewModel.TransfereeName = ee.FullName;
                string filename = "Itinerary" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
                var pdf = new Rotativa.ViewAsPdf("~/views/Orders/Partials/_Itinerary.cshtml", viewModel);
                byte[] pdfBytes = pdf.BuildFile(ControllerContext);
                MemoryStream stream = new MemoryStream(pdfBytes);                
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
        private OrdersTransfereeItineraryViewModel BuildItineraryByOrderId(string id)
        {
            IItineraryModelBuilder<OrdersTransfereeItineraryViewModel> builder = new ItineraryModelBuilder(_unitOfWork, _mapper);
            return builder.Build(id);            
        }
        private static IEnumerable<string> ParseAddress(string addresses)
        {
            if (string.IsNullOrEmpty(addresses) == true)
                return null;
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
