using AutoMapper;
using Microsoft.AspNet.Identity;
using Odin.Data.Core;
using Odin.Data.Core.Models;
using Odin.Helpers;
using Odin.Interfaces;
using Odin.ViewModels.Orders.Index;
using Odin.ViewModels.Orders.Transferee;
using Odin.ViewModels.Shared;
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
        public static string CurrentManager;
        public static string userId;

        public OrdersController(IUnitOfWork unitOfWork, IMapper mapper, IAccountHelper accountHelper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: Orders/id
        public ViewResult Index(string id)
        {
            //id = selected manager's id 
            //var userId = "";
            IEnumerable<Order> orders;

            if (id == null)
            {
                userId = User.Identity.GetUserId();
                orders = _unitOfWork.Orders.GetOrdersFor(userId, getUserRole());
                CurrentManager = null;
            }
            else
            {
                //TempData["curr_mngr"] = id;
                CurrentManager = id;
                userId = id;
                orders = _unitOfWork.Orders.GetOrdersFor(userId, UserRoles.ProgramManager);
            }
            ViewBag.userRole = _unitOfWork.Users.GetRoleByUserId(userId);
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

        /// <summary>
        /// We Can add this function to IUserRepository 
        /// </summary>
        /// <returns></returns>
        public string getUserRole()
        {
            if (User.IsInRole(UserRoles.Admin))
            {
                return UserRoles.Admin;
            }
            else if (User.IsInRole(UserRoles.Consultant))
            {
                return UserRoles.Consultant;
            }
            if (User.IsInRole(UserRoles.GlobalSupplyChain))
            {
                return UserRoles.GlobalSupplyChain;
            }
            else if (User.IsInRole(UserRoles.ProgramManager))
            {
                return UserRoles.ProgramManager;
            }
            else
            {
                return UserRoles.Transferee;
            }
        }

        // GET Partials
        public ActionResult DashboardPartial(string id)
        {
            var userId = User.Identity.GetUserId();
            var order = _unitOfWork.Orders.GetOrderFor(userId, id, UserRoles.Transferee);

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
        public ActionResult HelpPartial(string id)
        {
            var userId = User.Identity.GetUserId();
            var order = _unitOfWork.Orders.GetOrderFor(userId, id, UserRoles.Transferee);
            ViewBag.PropBotScript = "javascript:" + System.IO.File.ReadAllText(Server.MapPath(@"~/Scripts/bookmarklet/dist.min.js"));
            
            return PartialView("~/views/orders/partials/_Help.cshtml");
        }

        public ActionResult HousingPartial(string id)
        {
            var userId = User.Identity.GetUserId();

            Order order = null;
            if (User.IsInRole(UserRoles.Transferee))
            {
                order = _unitOfWork.Orders.GetOrderFor(userId, id, UserRoles.Transferee);
            }
            else if (User.IsInRole(UserRoles.ProgramManager))
            {
                order = _unitOfWork.Orders.GetOrderFor(userId, id, UserRoles.ProgramManager);
            }
            else
            {
                order = _unitOfWork.Orders.GetOrderFor(userId, id);
            }

            ViewBag.CurrentUser = userId;

            HousingViewModel viewModel = new HousingViewModel(order, _mapper, User);
            return PartialView("~/views/orders/partials/_Housing.cshtml", viewModel);

        }
        public ActionResult PropertiesPartial(string id)
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
            HousingViewModel viewModel = new HousingViewModel(order, _mapper);
            return PartialView("~/views/orders/partials/_HousingProperties.cshtml", viewModel.Properties);
        }
        public ActionResult PropertiesPartialPDF(string id, string listChoice)
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
        public ActionResult DetailsPartial(string id)
        {
            var userId = User.Identity.GetUserId();
            var order = _unitOfWork.Orders.GetOrderById(id);
            if (order == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Not found");
            }
            if (order.ConsultantId != userId && order.ProgramManagerId != userId && !User.IsInRole(UserRoles.ProgramManager))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized, "Unauthorized Order");
            }
            OrdersTransfereeViewModel viewModel = GetViewModelForOrderDetails(id);
            return PartialView("~/views/orders/partials/_Details.cshtml", viewModel);
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

        //id is the Order id. 
        public ActionResult HistoryPartial(string id)
        {
            var userId = User.Identity.GetUserId();
            var order = _unitOfWork.Orders.GetOrderFor(userId, id);

            if (order == null)
            {

                //TempData.Add("notfound", 1);
                return PartialView("~/views/orders/partials/_History.cshtml", null);
            }
            else
            {
                //TempData.Add("found", 2);
                IEnumerable<UserNotification> userNotifications = _unitOfWork.UserNotifications.GetUserNotificationHistory(userId, order.Id);
                IEnumerable<HistoryViewModel> vms = _mapper.Map<IEnumerable<UserNotification>, IEnumerable<HistoryViewModel>>(userNotifications);
                return PartialView("~/views/orders/partials/_History.cshtml", vms);
            }


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
        public ActionResult Transferee(string id)
        {
            //id is selected order id
            var userId = User.Identity.GetUserId();
            //setting current managers id to navigate through his orders
            if (CurrentManager != null && userId != CurrentManager)
            {
                userId = CurrentManager;
            }

            var userRole = getUserRole();
            Order order = null;
            if (User.IsInRole(UserRoles.Transferee))
            {
                order = _unitOfWork.Orders.GetOrderFor(userId, id, UserRoles.Transferee);
            }
            else if (User.IsInRole(UserRoles.ProgramManager))
            {
                order = _unitOfWork.Orders.GetOrderFor(userId, id, UserRoles.ProgramManager);
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
