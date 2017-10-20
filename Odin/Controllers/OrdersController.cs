using AutoMapper;
using Microsoft.AspNet.Identity;
using Odin.Data.Core;
using Odin.Data.Core.Models;
using Odin.Interfaces;
using Odin.ViewModels.Orders.Index;
using Odin.ViewModels.Orders.Transferee;
using System.Collections.Generic;
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

            var orderVms = _mapper.Map<IEnumerable<Order>, IEnumerable<OrdersIndexViewModel>>(orders);

            return View(orderVms);
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

            return View(vm);
        }
    }
    
}