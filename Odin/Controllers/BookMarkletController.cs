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
        private readonly IQueueStore _queueStore;

        public BookMarkletController(IUnitOfWork unitOfWork, IMapper mapper, IAccountHelper accountHelper, IQueueStore queueStore)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _queueStore = queueStore;
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
            var queueEntry = new PropBotJobQueueEntry();
            queueEntry.PropertyUrl = "http://danielsfavoritesite.com";
            queueEntry.OrderId = "testOrderId";
            queueEntry.Notes = "Cool new spot downtown, austin's gonna rent it";

            _queueStore.Add(queueEntry);

            return View();
        }
        
    }
}