using AutoMapper;
using Microsoft.AspNet.Identity;
using Odin.Data.Core;
using Odin.Data.Core.Models;
using Odin.Helpers;
using Odin.Interfaces;
using Odin.ViewModels.Orders.Transferee;
using Odin.ViewModels.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

using WebGrease.Css.Extensions;

namespace Odin.Controllers
{
    [Authorize]
    public class UserNotificationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserNotificationController(IUnitOfWork unitOfWork, IMapper mapper, IQueueStore queueStore)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public ActionResult GetUserNotifications()
        {
            var userId = User.Identity.GetUserId();

            IEnumerable<UserNotification> userNotifications = _unitOfWork.UserNotifications.GetUserNotification(userId);
            

            IEnumerable<NotificationViewModel> vms =_mapper.Map<IEnumerable<UserNotification>, IEnumerable<NotificationViewModel>>(userNotifications);
            
            return PartialView("~/views/Shared/partials/_notifications.cshtml", vms);
        }


        //public ActionResult GetUserNotificationHistory()
        //{
        //    var userId = User.Identity.GetUserId();

        //    IEnumerable<UserNotification> userNotifications = _unitOfWork.UserNotifications.GetUserNotification(userId);


        //    IEnumerable<NotificationViewModel> vms = _mapper.Map<IEnumerable<UserNotification>, IEnumerable<NotificationViewModel>>(userNotifications);

        //    return PartialView("~/views/Shared/NotificationHistory.cshtml", vms);
        //}



    }

}
