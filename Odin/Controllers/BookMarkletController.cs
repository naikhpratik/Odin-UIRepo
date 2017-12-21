using AutoMapper;
using Microsoft.AspNet.Identity;
using Odin.Data.Core;
using Odin.Data.Core.Dtos;
using Odin.Data.Core.Models;
using Odin.Filters;
using Odin.Interfaces;
using Odin.ViewModels.BookMarklet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Odin.Controllers
{

    [AuthorizeBookMarklet]
    public class BookMarkletController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IQueueStore _queueStore;
        private readonly IBookMarkletHelper _bookMarkletHelper;

        public BookMarkletController(IUnitOfWork unitOfWork, IMapper mapper, IQueueStore queueStore, IBookMarkletHelper bookMarkletHelper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _queueStore = queueStore;
            _bookMarkletHelper = bookMarkletHelper;
        }

        [HttpGet]
        public ActionResult Index(string url)
        {
            var userId = User.Identity.GetUserId();
            IEnumerable<Order> orders = null;
            if (User.IsInRole(UserRoles.Transferee))
            {
                orders = _unitOfWork.Orders.GetOrdersFor(userId,UserRoles.Transferee);
            }
            else
            {
                orders = _unitOfWork.Orders.GetOrdersFor(userId);
            }

            if (orders.Count() == 0)
            {
                BookMarkletErrorViewModel error = new BookMarkletErrorViewModel();
                error.Header = "Sorry!";
                error.Message = "It looks like you don't have any orders yet!";
                return View("Error", error);
            }

            if (_bookMarkletHelper.IsValidUrl(url))
            {
                BookMarkletViewModel bm = new BookMarkletViewModel();
                bm.Orders = _mapper.Map<IEnumerable<Order>, IEnumerable<BookMarkletOrderViewModel>>(orders);
                bm.PropertyUrl = url;
                return View(bm);
            }
            else
            {
                BookMarkletErrorViewModel error = new BookMarkletErrorViewModel();
                error.Header = "Sorry!";
                error.Message = "This page is not currently supported! Try another homefinding site for this property.";
                return View("Error",error);
            }
            
        }

        [HttpPost]
        public ActionResult Index(BookMarkletDto dto)
        {
            if (String.IsNullOrEmpty(dto.OrderId) || String.IsNullOrEmpty(dto.PropertyUrl))
            {
                BookMarkletErrorViewModel error = new BookMarkletErrorViewModel();
                error.Header = "Uh oh!";
                error.Message = "It looks like something went wrong.  Please try again.";
                return View("Error", error);
            }

            var queueEntry = _mapper.Map<BookMarkletDto, PropBotJobQueueEntry>(dto);
            _queueStore.Add(queueEntry);

            //var vm = _mapper.Map<BookMarkletDto, BookMarkletAddViewModel>(dto);
            return View();
        }
        
    }
}