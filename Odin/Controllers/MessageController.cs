using AutoMapper;
using Odin.ViewModels.Shared;
using Odin.Data.Core;
using Odin.Data.Core.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using Odin.Data.Persistence;
using Microsoft.AspNet.Identity;

namespace Odin.Controllers
{
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