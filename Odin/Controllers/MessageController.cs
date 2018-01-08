using AutoMapper;
using Microsoft.AspNet.Identity;
using Odin.Data.Core;
using Odin.Data.Core.Models;
using Odin.ViewModels.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Odin.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MessageController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public ActionResult MessagePartial(string id)
        {            
            PropertyMessagesViewModel viewModel = new PropertyMessagesViewModel();
            HomeFindingProperty property = getPropertyById(id);
            if (property == null)
                return HttpNotFound();
            viewModel.messages = GetMessagesByPropertyId(property.Id);
            if (viewModel.messages == null)
                return HttpNotFound();
            if (viewModel.messages.Count() > 0)
                viewModel.latest = viewModel.messages.First().MessageDate;
            viewModel.Id = property.Id;
            var userId = User.Identity.GetUserId(); 
            ViewBag.CurrentUser = userId;
            return PartialView("~/views/Orders/Partials/_Message.cshtml", viewModel);
        }

        private IEnumerable<Message> GetMessagesByPropertyId(string Id)
        {
            var msg = _unitOfWork.Messages.GetMessagesByPropertyId(Id);
            return msg;
        }
        private HomeFindingProperty getPropertyById(string id)
        {
            var prop = _unitOfWork.HomeFindingProperties.GetHomeFindingPropertyById(id);
            return prop;
        }
    }
}