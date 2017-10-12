using AutoMapper;
using System.Web.Mvc;
using Odin.Data.Core;
using Odin.Data.Core.Models;
using Odin.ViewModels;

namespace Odin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HomeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult OfOrder(int orderId, int HomeId)
        {
            //todo: 1 - add model or viewmodel to represent the home associated with an order
            // 2- create a view to display the home, could be a patialview - depending on size and picture?
            //var orderHome = _unitOfWork.GetHome4Order(orderId,HomeId);                        
            //return PartialView(orderHome);
            return View();
        }
    }
}