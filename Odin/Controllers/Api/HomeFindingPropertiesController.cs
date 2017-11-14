using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Odin.Data.Core;
using AutoMapper;
using Odin.Filters;
using System.Web.Http;
using Odin.ViewModels.Orders.Transferee;
using Odin.Data.Core.Dtos;
using Microsoft.AspNet.Identity;
using Odin.Data.Core.Models;

namespace Odin.Controllers.Api
{
    public class HomeFindingPropertiesController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HomeFindingPropertiesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        // POST /api/homefindingproperties
        [HttpPost]
        [Authorize]
        public IHttpActionResult Index(HousingPropertyViewModel propertyVM)
        {
            var userId = User.Identity.GetUserId();

            HomeFindingProperty homeFindingProperty = _mapper.Map<HousingPropertyViewModel, HomeFindingProperty>(propertyVM);

            // !!!: This relies that the HomeFinding Id is the same of the Order. Refactor later
            Order order = _unitOfWork.Orders.GetOrderFor(userId, propertyVM.Id);
            HomeFinding homeFinding = order.HomeFinding;
            homeFinding.HomeFindingProperties.Add(homeFindingProperty);

            _unitOfWork.Complete();

            return Ok();
        }
    }
}