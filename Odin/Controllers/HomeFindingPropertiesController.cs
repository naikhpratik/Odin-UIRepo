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
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Odin.Interfaces;
using System.Collections.ObjectModel;

namespace Odin.Controllers
{
    [Authorize]
    public class HomeFindingPropertiesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageStore _imageStore;

        public HomeFindingPropertiesController(IUnitOfWork unitOfWork, IMapper mapper, IImageStore imageStore)
        {
            _mapper = mapper;
            _imageStore = imageStore;
            _unitOfWork = unitOfWork;
        }

        // POST /homefindingproperties
        [HttpPost]
        public ActionResult Create(HousingPropertyViewModel propertyVM)
        {
            var userId = User.Identity.GetUserId();

            HomeFindingProperty homeFindingProperty = new HomeFindingProperty();
            // mapping wipes out the Id - this is hack to resolve that
            propertyVM.Id = homeFindingProperty.Id;
            homeFindingProperty = _mapper.Map<HousingPropertyViewModel, HomeFindingProperty>(propertyVM, homeFindingProperty);

            Order order = _unitOfWork.Orders.GetOrderFor(userId, propertyVM.OrderId);
            HomeFinding homeFinding = order.HomeFinding;
            homeFinding.HomeFindingProperties.Add(homeFindingProperty);

            foreach (var postedFile in propertyVM.UploadedPhotos)
            {
                if (postedFile != null) {
                    try
                    {
                        var storageId = _imageStore.SaveImage(postedFile.InputStream);
                        var urlStr = _imageStore.UriFor(storageId).AbsoluteUri;
                        var photo = new Photo(storageId, urlStr);
                        homeFindingProperty.Property.Photos.Add(photo);
                    }
                    catch (Exception e)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.ServiceUnavailable);
                    }
                }
            }

            try
            {
                _unitOfWork.Complete();
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.ServiceUnavailable);
            }

            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }

        // GET /homefindingproperties/propertypartial/[propertyId]
        [HttpGet]
        public ActionResult PropertyPartial(string id)
        {
            HomeFindingProperty homeFindingProperty;
            homeFindingProperty = _unitOfWork.HomeFindingProperties.GetHomeFindingPropertyById(id);

            HousingPropertyViewModel viewModel = _mapper.Map<HomeFindingProperty, HousingPropertyViewModel>(homeFindingProperty);

            return PartialView("~/views/orders/partials/_PropertyDetails.cshtml", viewModel);
        }
    }
}