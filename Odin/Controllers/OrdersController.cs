using AutoMapper;
using Microsoft.AspNet.Identity;
using Odin.Data.Core;
using Odin.Data.Core.Models;
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

            return View();
        }

        // GET Partials
        public ActionResult HousingPartial(string id)
        {
            var userId = User.Identity.GetUserId();
            var order = _unitOfWork.Orders.GetOrderFor(userId, id);

            HousingViewModel viewModel = _mapper.Map<HomeFinding, HousingViewModel>(order.HomeFinding);
            viewModel = _mapper.Map<Order, HousingViewModel>(order, viewModel);

            ICollection<HomeFindingProperty> homeFindingProperties = order.HomeFinding.HomeFindingProperties;
            ICollection<HousingPropertyViewModel> propertyViewModels;
            propertyViewModels = _mapper.Map<ICollection<HomeFindingProperty>, ICollection<HousingPropertyViewModel>>(homeFindingProperties);

            viewModel.Properties = propertyViewModels;
            return PartialView("~/views/orders/partials/_Housing.cshtml", viewModel);
        }

        public ActionResult DetailsPartial(string id)
        {
            var order = _unitOfWork.Orders.GetOrderById(id);
            OrdersTransfereeViewModel viewModel = viewModelForOrder(order);
            return PartialView("~/views/orders/partials/_Details.cshtml",viewModel);
        }

        public ActionResult IntakePartial(string id)
        {
            var order = _unitOfWork.Orders.GetOrderById(id);
            OrdersTransfereeViewModel viewModel = viewModelForOrder(order);
            return PartialView("~/views/orders/partials/_Intake.cshtml", viewModel);
        }

        public ActionResult ItineraryPartial(string id)
        {
            //var order = _unitOfWork.Orders.GetOrderById(id);
            //OrdersTransfereeViewModel viewModel = viewModelForOrder(order);
            return PartialView("~/views/orders/partials/_Itinerary.cshtml", null);
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
        public ViewResult Transferee(string id)
        {
            var order = _unitOfWork.Orders.GetOrderById(id);

            OrdersTransfereeViewModel vm = _mapper.Map<Order, OrdersTransfereeViewModel>(order);

            var cats = order.Services.Select(s => s.ServiceType.Category).ToList();
            var ids = order.Services.Select(s => s.ServiceType.Id).ToList();
            
            //Remove service types that already have services.
            var filtPossible = _unitOfWork.ServiceTypes.GetPossibleServiceTypes(cats, ids);

            vm.PossibleServices =
                _mapper.Map<IEnumerable<ServiceType>, IEnumerable<ServiceTypeViewModel>>(filtPossible);

            vm.NumberOfBathrooms = _unitOfWork.NumberOfBathrooms.GetNumberOfBathroomsList();
            vm.HousingTypes = _unitOfWork.HousingTypes.GetHousingTypesList();
            vm.AreaTypes = _unitOfWork.AreaTypes.GetAreaTypesList();
            vm.TransportationTypes = _unitOfWork.TransportationTypes.GetTransportationTypes();
            vm.DepositTypes = _unitOfWork.DepositTypes.GetDepositTypesList();
            vm.BrokerFeeTypes = _unitOfWork.BrokerFeeTypes.GetBorkerBrokerFeeTypes();
            
            return View(vm);
        }

        private OrdersTransfereeViewModel viewModelForOrder(Order order)
        {
            OrdersTransfereeViewModel vm = _mapper.Map<Order, OrdersTransfereeViewModel>(order);

            var cats = order.Services.Select(s => s.ServiceType.Category).ToList();
            var ids = order.Services.Select(s => s.ServiceType.Id).ToList();

            //Remove service types that already have services.
            var filtPossible = _unitOfWork.ServiceTypes.GetPossibleServiceTypes(cats, ids);

            vm.PossibleServices =
                _mapper.Map<IEnumerable<ServiceType>, IEnumerable<ServiceTypeViewModel>>(filtPossible);

            vm.NumberOfBathrooms = _unitOfWork.NumberOfBathrooms.GetNumberOfBathroomsList();
            vm.HousingTypes = _unitOfWork.HousingTypes.GetHousingTypesList();
            vm.AreaTypes = _unitOfWork.AreaTypes.GetAreaTypesList();
            vm.TransportationTypes = _unitOfWork.TransportationTypes.GetTransportationTypes();
            vm.DepositTypes = _unitOfWork.DepositTypes.GetDepositTypesList();
            vm.BrokerFeeTypes = _unitOfWork.BrokerFeeTypes.GetBorkerBrokerFeeTypes();

            return vm;
        }
    }
    
}