using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Odin.Data.Core;
using Odin.Data.Core.Models;
using Odin.ViewModels;
using Odin.Interfaces;
using Microsoft.AspNet.Identity.Owin;
using RazorEngine;

using Microsoft.AspNet.Identity.EntityFramework;

namespace Odin.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        //Uncomment to test or use the SendEmailConfirmationTokenAsync method
        //private IAccountHelper _accountHelper;
        public OrdersController(IUnitOfWork unitOfWork, IMapper mapper) //,IAccountHelper accountHelper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            //Uncomment to test or use the SendEmailConfirmationTokenAsync method
           // _accountHelper = accountHelper;
        }

    // GET: Orders
    public ViewResult Index()
        {
            var userId = User.Identity.GetUserId();
            //Uncomment to test the SendEmailConfirmationTokenAsync method
           // _accountHelper.SendEmailConfirmationTokenAsync("06bb5638-796e-4fb7-8e4e-dd95d898b123");

            //SendEmailConfirmationTokenAsync("06bb5638-796e-4fb7-8e4e-dd95d898b123").Wait();
            var orders = _unitOfWork.Orders.GetOrdersFor(userId);

            var orderVms = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderIndexViewModel>>(orders);

            return View(orderVms);
        }       
    }
}