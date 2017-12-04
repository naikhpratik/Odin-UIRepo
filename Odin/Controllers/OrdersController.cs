using AutoMapper;
using Microsoft.AspNet.Identity;
using Odin.Data.Core;
using Odin.Data.Core.Models;
using Odin.Helpers;
using Odin.Interfaces;
using Odin.ViewModels.Orders.Transferee;
using Odin.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Odin.Helpers;
using System.IO;

namespace Odin.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrdersController(IUnitOfWork unitOfWork, IMapper mapper,IAccountHelper accountHelper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: Orders
        public ViewResult Index()
        {
            var userId = User.Identity.GetUserId();

            var orders = _unitOfWork.Orders.GetOrdersFor(userId);

            //var orderVms = _mapper.Map<IEnumerable<Order>, IEnumerable<OrdersIndexViewModel>>(orders);

            return View(orders);
        }

        // GET Partials
        public ActionResult HousingPartial(string id)
        {
            var userId = User.Identity.GetUserId();
            var order = _unitOfWork.Orders.GetOrderFor(userId, id);

            HousingViewModel viewModel = new HousingViewModel(order, _mapper);

            return PartialView("~/views/orders/partials/_Housing.cshtml", viewModel);
        }

        public ActionResult PropertiesPartial(string id)
        {
            var userId = User.Identity.GetUserId();
            var order = _unitOfWork.Orders.GetOrderFor(userId, id);

            HousingViewModel viewModel = new HousingViewModel(order, _mapper);

            return PartialView("~/views/orders/partials/_HousingProperties.cshtml", viewModel.Properties);
        }

        public ActionResult DetailsPartial(string id)
        {
            var userId = User.Identity.GetUserId();
            var order = _unitOfWork.Orders.GetOrderById(id);
            if (order == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Not found");
            }
            if (order.ConsultantId != userId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized, "Unauthorized Order");
            }
            OrdersTransfereeViewModel viewModel = GetViewModelForOrder(id);
            return PartialView("~/views/orders/partials/_Details.cshtml",viewModel); 
        }

        public ActionResult IntakePartial(string id)
        {
            OrdersTransfereeViewModel viewModel = GetViewModelForOrder(id);
            return PartialView("~/views/orders/partials/_Intake.cshtml", viewModel);
        }

        public ActionResult ItineraryPartial(string id)
        {            
            OrdersTransfereeItineraryViewModel viewModel = GetItineraryByOrderId(id);
            viewModel.Id = id;
            viewModel.mustPrint = false;
            return PartialView("~/views/orders/partials/_Itinerary.cshtml", viewModel);
        }
        public ActionResult AppointmentPartial(string id)
        {
            Appointment viewModel;
            if (string.IsNullOrEmpty(id) == false)
            {
                viewModel = GetAppointmentById(id);
                viewModel.Id = id;
            }
            else
            {
                viewModel = new Appointment() { Id = null, ScheduledDate = DateTime.Now }; ;
            }
            return PartialView("~/views/orders/partials/_Appointment.cshtml", viewModel);
        }

        public ActionResult Details(string orderId)
        {
            var userId = User.Identity.GetUserId();

            var order = _unitOfWork.Orders.GetOrderById(orderId);

            if (order == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Not found");
            }

            if (order.ConsultantId != userId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized, "Unauthorized Order");
            }

            return View();
        }

        // GET: Transferee
        public ActionResult Transferee(string id)
        {
            var userId = User.Identity.GetUserId();
            var order = _unitOfWork.Orders.GetOrderFor(userId, id);

            if (order == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Not found");
            }
            ViewBag.Id = id;
            OrdersTransfereeViewModel viewModel = GetViewModelForOrder(id);
            return View(viewModel);
        }

        private OrdersTransfereeViewModel GetViewModelForOrder(string id)
        {
            var userId = User.Identity.GetUserId();
            var order = _unitOfWork.Orders.GetOrderFor(userId,id);

            OrdersTransfereeViewModel vm = _mapper.Map<Order, OrdersTransfereeViewModel>(order);
            vm.Services = vm.Services.OrderBy(s => s.ServiceTypeSortOrder);
            
            //Populate list of service categories available for this order.
            var cats = ServiceHelper.GetCategoriesForServiceFlag(order.ServiceFlag);

            //Get all service types that the order already has.
            var ids = order.Services.Select(s => s.ServiceType.Id).ToList();

            //Remove service types that already have services.
            var filtPossible = _unitOfWork.ServiceTypes.GetPossibleServiceTypes(cats, ids);

            vm.PossibleServices =
                _mapper.Map<IEnumerable<ServiceType>, IEnumerable<ServiceTypeViewModel>>(filtPossible).OrderBy(s => s.SortOrder);

            vm.NumberOfBathrooms = _unitOfWork.NumberOfBathrooms.GetNumberOfBathroomsList();
            vm.HousingTypes = _unitOfWork.HousingTypes.GetHousingTypesList();
            vm.AreaTypes = _unitOfWork.AreaTypes.GetAreaTypesList();
            vm.TransportationTypes = _unitOfWork.TransportationTypes.GetTransportationTypes();
            vm.DepositTypes = _unitOfWork.DepositTypes.GetDepositTypesList();
            vm.BrokerFeeTypes = _unitOfWork.BrokerFeeTypes.GetBrokerFeeTypes();

            return vm;
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
        private Appointment GetAppointmentById(string Id)
        {
            var itinAppointment = _unitOfWork.Appointments.GetAppointmentById(Id);
            return itinAppointment;
        }
        
        public ActionResult GeneratePDF(string id)
        {
            OrdersTransfereeItineraryViewModel viewModel = GetItineraryByOrderId(id);
            viewModel.Id = id;
            viewModel.mustPrint = true;
            return new Rotativa.ViewAsPdf("Partials/_Itinerary", viewModel) { FileName = "Itinerary.pdf", PageMargins = new Rotativa.Options.Margins(0, 0, 0, 0) }; 
        }
        public ActionResult EmailGeneratedPDF(string id, string email)
        {
            OrdersTransfereeItineraryViewModel viewModel = GetItineraryByOrderId(id);
            viewModel.Id = id;
            viewModel.mustPrint = true;
            string filename = "Itinerary" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
            var pdf = new Rotativa.ViewAsPdf("Partials/_Itinerary", viewModel) {FileName = filename, PageMargins = new Rotativa.Options.Margins(0, 0, 0, 0) };
            byte[] pdfBytes = pdf.BuildFile(ControllerContext);
            MemoryStream stream = new MemoryStream(pdfBytes);
            EmailHelper EH = new EmailHelper();
            EH.SendEmail_FS(email, "Your DwellWorks Itinerary", "Please find attached your itinerary for the upcoming move", SendGrid.MimeType.Html, filename, pdfBytes);
            return PartialView("~/views/orders/partials/_Itinerary.cshtml", viewModel);
        }
    }

}
