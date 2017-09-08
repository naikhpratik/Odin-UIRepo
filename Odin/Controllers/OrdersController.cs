using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Odin.Data.Core;
using Odin.Data.Core.Models;
using Odin.ViewModels;

namespace Odin.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrdersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: Orders
        public ViewResult Index()
        {
            var userId = User.Identity.GetUserId();

            var orders = _unitOfWork.Orders.GetOrdersFor(userId);

            var orderVms = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderIndexViewModel>>(orders);

            return View(orderVms);
        }
    }
}