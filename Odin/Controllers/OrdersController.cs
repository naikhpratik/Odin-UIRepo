﻿using AutoMapper;
using Microsoft.AspNet.Identity;
using Odin.Data.Core;
using Odin.Data.Core.Models;
using Odin.Interfaces;
using Odin.ViewModels.Orders.Transferee;
using Odin.ViewModels.Shared;
using Odin.Data.Extensions;
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

        public ActionResult HousingPartial(string id)
        {
            var order = _unitOfWork.Orders.GetOrderById(id);

            HousingViewModel viewModel = _mapper.Map<HomeFinding, HousingViewModel>(order.HomeFinding);
            viewModel.NumberOfPets = order.Pets.Count();
            int numKids = order.Children == null ? 0 : order.Children.Count();
            if (numKids == 0 && order.SpouseName == "")
                viewModel.SpouceAndKids = null;
            else
                viewModel.SpouceAndKids = (order.SpouseName == "" ? "No" : "Yes") + " / " + numKids.ToString();
            return PartialView("~/views/orders/partials/_Housing.cshtml", viewModel);
        }

        // GET Partials
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
            OrdersTransfereeItineraryViewModel viewModel = GetItineraryByOrderId(id);
            viewModel.Id = id;
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

            //vm.PossibleServices =                _mapper.Map<IEnumerable<ServiceType>, IEnumerable<ServiceTypeViewModel>>(filtPossible);

            vm.NumberOfBathrooms = _unitOfWork.NumberOfBathrooms.GetNumberOfBathroomsList();
            vm.HousingTypes = _unitOfWork.HousingTypes.GetHousingTypesList();
            vm.AreaTypes = _unitOfWork.AreaTypes.GetAreaTypesList();
            vm.TransportationTypes = _unitOfWork.TransportationTypes.GetTransportationTypes();
            vm.DepositTypes = _unitOfWork.DepositTypes.GetDepositTypesList();
            vm.BrokerFeeTypes = _unitOfWork.BrokerFeeTypes.GetBorkerBrokerFeeTypes();

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

    }
    
}