﻿using AutoMapper;
using Microsoft.AspNet.Identity;
using Odin.Data.Core;
using Odin.Data.Core.Models;
using Odin.Extensions;
using Odin.Filters;
using Odin.Helpers;
using Odin.Interfaces;
using Odin.ViewModels.Orders.Index;
using Odin.ViewModels.Orders.Transferee;
using Odin.ViewModels.Shared;
using System;
using System.Collections;
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
        //public static string CurrentManager;
        //public static string userId;

        public OrdersController(IUnitOfWork unitOfWork, IMapper mapper, IAccountHelper accountHelper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: Orders/id
        [RoleAuthorize(UserRoles.Consultant,UserRoles.ProgramManager)]
        public ViewResult Index(string id)
        {
            //id = selected manager's id 
            IEnumerable<Order> orders = null;
            //PM's and SC's are the only people who can view other individuals' files.
            if (!String.IsNullOrEmpty(id) && (User.IsInRole(UserRoles.ProgramManager) || User.IsInRole(UserRoles.GlobalSupplyChain)))
            {
                orders = _unitOfWork.Orders.GetOrdersFor(id, UserRoles.ProgramManager);
            }
            else
            {
                orders = _unitOfWork.Orders.GetOrdersFor(User.Identity.GetUserId(),User.GetUserRole());
            }
            
            var managers = _unitOfWork.Managers.GetManagers();
            var orderVms = _mapper.Map<IEnumerable<Order>, IEnumerable<OrdersIndexViewModel>>(orders);

            if (managers != null)
            {
                var result = ((IEnumerable)managers).Cast<Manager>().ToList();
                var managerVms = _mapper.Map<IEnumerable<Manager>, IEnumerable<ManagerViewModel>>(result);
                OrderIndexManagerViewModel ordermanagervms = new OrderIndexManagerViewModel(orderVms, managerVms);
                return View(ordermanagervms);
            }
            else
            {
                return View(orderVms);
            }
        }

        // GET Partials
        [RoleAuthorize(UserRoles.Transferee)]
        public ActionResult DashboardPartial(string id)
        {
            var userId = User.Identity.GetUserId();
            var order = _unitOfWork.Orders.GetOrderFor(userId, id, User.GetUserRole());

            if (order == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var dashVM = _mapper.Map<Order, DashboardViewModel>(order);
            ItineraryHelper helper = new ItineraryHelper(_unitOfWork, _mapper);
            dashVM.Itinerary = helper.GetItinerary(id);

            return PartialView("~/views/orders/partials/_Dashboard.cshtml", dashVM);
        }

        // GET Partials
        [RoleAuthorize(UserRoles.Transferee)]
        public ActionResult HelpPartial(string id)
        {
            var userId = User.Identity.GetUserId();
            var order = _unitOfWork.Orders.GetOrderFor(userId, id, User.GetUserRole());

            if (order == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            ViewBag.PropBotScript = "javascript:" + System.IO.File.ReadAllText(Server.MapPath(@"~/Scripts/bookmarklet/dist.min.js"));
            
            return PartialView("~/views/orders/partials/_Help.cshtml");
        }

        [RoleAuthorize(UserRoles.ProgramManager,UserRoles.Consultant,UserRoles.Transferee)]
        public ActionResult HousingPartial(string id)
        {
            var userId = User.Identity.GetUserId();

            Order order = _unitOfWork.Orders.GetOrderFor(userId, id, User.GetUserRole());

            ViewBag.CurrentUser = userId;

            HousingViewModel viewModel = new HousingViewModel(order, _mapper, User);
            return PartialView("~/views/orders/partials/_Housing.cshtml", viewModel);

        }

        [RoleAuthorize(UserRoles.ProgramManager, UserRoles.Consultant, UserRoles.Transferee)]
        public ActionResult PropertiesPartial(string id)
        {
            var userId = User.Identity.GetUserId();
            Order order = _unitOfWork.Orders.GetOrderFor(userId, id, User.GetUserRole());

            HousingViewModel viewModel = new HousingViewModel(order, _mapper);
            return PartialView("~/views/orders/partials/_HousingProperties.cshtml", viewModel.Properties);
        }

        [RoleAuthorize(UserRoles.ProgramManager, UserRoles.Consultant, UserRoles.Transferee)]
        public ActionResult PropertiesPartialPDF(string id, string listChoice)
        {
            var userId = User.Identity.GetUserId();
            Order order = _unitOfWork.Orders.GetOrderFor(userId, id, User.GetUserRole());
            HousingViewModel viewModel = new HousingViewModel(order, _mapper, listChoice, User);

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

        [RoleAuthorize(UserRoles.Consultant,UserRoles.ProgramManager)]
        public ActionResult DetailsPartial(string id)
        {
            var userId = User.Identity.GetUserId();

            Order order = _unitOfWork.Orders.GetOrderFor(userId, id, User.GetUserRole());

            if (order == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Not found");
            }

            OrdersTransfereeViewModel viewModel = GetOrdersTransfereeViewModel(order);
            return PartialView("~/views/orders/partials/_Details.cshtml", viewModel);
        }

        [RoleAuthorize(UserRoles.Consultant, UserRoles.ProgramManager)]
        public ActionResult IntakePartial(string id)
        {
            var userId = User.Identity.GetUserId();
            Order order = _unitOfWork.Orders.GetOrderFor(userId, id, User.GetUserRole());

            if (order == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Not found");
            }

            OrdersTransfereeViewModel viewModel = GetOrdersTransfereeViewModel(order);
            return PartialView("~/views/orders/partials/_Intake.cshtml", viewModel);
        }

        [RoleAuthorize(UserRoles.ProgramManager, UserRoles.Consultant, UserRoles.Transferee)]
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

        [RoleAuthorize(UserRoles.Consultant, UserRoles.ProgramManager)]
        public ActionResult HistoryPartial(string id)
        {
            var userId = User.Identity.GetUserId();
            Order order = _unitOfWork.Orders.GetOrderFor(userId, id, User.GetUserRole());

            if (order == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Not found");
            }
            else
            {
                IEnumerable<Notification> notifications = _unitOfWork.Notifications.GetOrderNotifications(order.Id);
                IEnumerable<HistoryViewModel> vms = _mapper.Map<IEnumerable<Notification>, IEnumerable<HistoryViewModel>>(notifications);
                return PartialView("~/views/orders/partials/_History.cshtml", vms);
            }
        }

        [RoleAuthorize(UserRoles.ProgramManager, UserRoles.Consultant, UserRoles.Transferee)]
        public ActionResult Transferee(string id)
        {
            //id is selected order id
            ViewBag.Id = id;
            var userId = User.Identity.GetUserId();

            Order order = _unitOfWork.Orders.GetOrderFor(userId,id,User.GetUserRole());

            if (order == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Not found");
            }

            OrdersTransfereeViewModel viewModel = GetOrdersTransfereeViewModel(order);
            return View(viewModel); 
        }

        private OrdersTransfereeViewModel GetOrdersTransfereeViewModel(Order order)
        {
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
