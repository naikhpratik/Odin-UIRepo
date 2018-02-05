using Microsoft.AspNet.Identity;
using Odin.Data.Core;
using Odin.Data.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Odin.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private IUnitOfWork _unitOfWork;

        // GET: Role
        public RoleController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index(string returnUrl = null)
        {
            if (User.IsInRole(UserRoles.Transferee))
            {
                IEnumerable<Order> orders = _unitOfWork.Orders.GetOrdersFor(User.Identity.GetUserId(), UserRoles.Transferee);

                if (Url.IsLocalUrl(returnUrl) && returnUrl != "/" && returnUrl != "/Orders")
                {
                    return Redirect(returnUrl);
                }

                if (orders.Count() == 1)
                {
                    return RedirectToAction("Transferee", "Orders", new {id = orders.First().Id});
                }
            }
            else if (User.IsInRole(UserRoles.Consultant) || User.IsInRole(UserRoles.ProgramManager))
            {
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Orders");
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }
    }
}