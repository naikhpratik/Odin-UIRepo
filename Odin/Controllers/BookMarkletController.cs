using AutoMapper;
using Microsoft.AspNet.Identity;
using Odin.Data.Core;
using Odin.Data.Core.Models;
using Odin.Filters;
using Odin.Helpers;
using Odin.Interfaces;
using Odin.ViewModels.BookMarklet;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Odin.Controllers
{

    [AuthorizeBookMarklet]
    public class BookMarkletController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookMarkletController(IUnitOfWork unitOfWork, IMapper mapper, IAccountHelper accountHelper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult Index(string url)
        {
            var userId = User.Identity.GetUserId();
            IEnumerable<Order> orders = _unitOfWork.Orders.GetOrdersFor(userId);

            ViewBag.Valid = BookMarkletHelper.IsValidUrl(url);
            
            IEnumerable<BookMarkletViewModel> vms = _mapper.Map<IEnumerable<Order>, IEnumerable<BookMarkletViewModel>>(orders);
            return View(vms);
        }

        [HttpPost]
        public ActionResult Add(FormCollection collection)
        {
            return View();
        }
        
    }
}