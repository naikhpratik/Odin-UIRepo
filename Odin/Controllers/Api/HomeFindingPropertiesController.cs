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
        public IHttpActionResult Index(HomeFindingPropertyDto propertyDto)
        {
            var userId = User.Identity.GetUserId();
            Order order = _unitOfWork.Orders.GetOrderFor(userId, propertyDto.OrderId);
            HomeFinding homeFinding = order.HomeFinding;

            Property property = _mapper.Map<HomeFindingPropertyDto, Property>(propertyDto);
            HomeFindingProperty hfp = new HomeFindingProperty();
            hfp.Property = property;
            homeFinding.HomeFindingProperties.Add(hfp);

            _unitOfWork.Complete();

            return Ok();
        }
    }
}