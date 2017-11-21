using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Odin.Data.Core;
using AutoMapper;
using Odin.Filters;
using System.Web.Mvc;
using Odin.ViewModels.Orders.Transferee;
using Odin.Data.Core.Models;
using System.Net;
using Microsoft.AspNet.Identity;

namespace Odin.Controllers
{
    public class HomeFindingPropertiesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HomeFindingPropertiesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        // POST /homefindingproperties
        [HttpPost]
        [Authorize]
        public ActionResult Index(HousingPropertyViewModel propertyVM)
        {
            var userId = User.Identity.GetUserId();

            HomeFindingProperty homeFindingProperty = new HomeFindingProperty();
            homeFindingProperty = _mapper.Map<HousingPropertyViewModel, HomeFindingProperty>(propertyVM, homeFindingProperty);

            Order order = _unitOfWork.Orders.GetOrderFor(userId, propertyVM.OrderId);
            HomeFinding homeFinding = order.HomeFinding;
            homeFinding.HomeFindingProperties.Add(homeFindingProperty);

            _unitOfWork.Complete();

            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }
    }
}