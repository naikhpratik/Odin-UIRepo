using AutoMapper;
using Microsoft.AspNet.Identity;
using Odin.Data.Core;
using Odin.Data.Core.Models;
using Odin.Helpers;
using Odin.Interfaces;
using Odin.ViewModels.Orders.Transferee;
using Odin.ViewModels.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

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
        public ActionResult PropertiesPartialPDF(string id, string listChoice)
        {
            var userId = User.Identity.GetUserId();
            var order = _unitOfWork.Orders.GetOrderFor(userId, id);
            HousingViewModel viewModel = new HousingViewModel(order, _mapper, listChoice);
            if (viewModel.Properties.Count() == 0)
            {
                return new HttpNotFoundResult();
            }           
            ViewBag.isPDF = true;
            return new Rotativa.ViewAsPdf("Partials/_HousingProperties", viewModel.Properties)
            {
                FileName = "Housing.pdf",
                PageMargins = new Rotativa.Options.Margins(0, 0, 0, 0)
            };           
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
            OrdersTransfereeViewModel viewModel = GetViewModelForOrderDetails(id);
            return PartialView("~/views/orders/partials/_Details.cshtml",viewModel); 
        }

        public ActionResult IntakePartial(string id)
        {
            OrdersTransfereeViewModel viewModel = GetViewModelForOrderDetails(id);
            return PartialView("~/views/orders/partials/_Intake.cshtml", viewModel);
        }

        public ActionResult ItineraryPartial(string id)
        {            
            OrdersTransfereeItineraryViewModel viewModel = GetItineraryByOrderId(id);
            viewModel.Id = id;
            viewModel.IsPdf = false;
            Transferee ee = GetTransfereeByOrderId(id);
            if (ee == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Not found");
            viewModel.TransfereeName = ee.FullName;
            return PartialView("~/views/orders/partials/_Itinerary.cshtml", viewModel);
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

            Order order = null;
            if (User.IsInRole(UserRoles.Transferee))
            {
                order = _unitOfWork.Orders.GetOrderFor(userId, id, UserRoles.Transferee);
            }
            else
            {
                order = _unitOfWork.Orders.GetOrderFor(userId, id);
            }

            if (order == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Not found");
            }
            ViewBag.Id = id;
            OrdersTransfereeViewModel viewModel = GetViewModelForOrderDetails(id);
            return View(viewModel);
        }       
        private OrdersTransfereeViewModel GetViewModelForOrderDetails(string id)
        {
            var userId = User.Identity.GetUserId();
            var order = _unitOfWork.Orders.GetOrderById(id);

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
        public Transferee GetTransfereeByOrderId(string id)
        {
            return _unitOfWork.Transferees.GetTransfereeByOrderId(id);
        }
        private OrdersTransfereeItineraryViewModel GetItineraryByOrderId(string id)
        {
            ItineraryHelper itinHelper = new ItineraryHelper(_unitOfWork, _mapper);
            return itinHelper.Build(id);
        }
        public ActionResult GeneratePDF(string id)
        {
            OrdersTransfereeItineraryViewModel viewModel = GetItineraryByOrderId(id);
            viewModel.Id = id;
            viewModel.IsPdf = true;
            Transferee ee = GetTransfereeByOrderId(id);
            viewModel.TransfereeName = ee.FullName;
            return new Rotativa.ViewAsPdf("Partials/_Itinerary", viewModel)
            {
                FileName = "Itinerary.pdf",
                PageMargins = new Rotativa.Options.Margins(0, 0, 0, 0)
            };
        }
    }
}
