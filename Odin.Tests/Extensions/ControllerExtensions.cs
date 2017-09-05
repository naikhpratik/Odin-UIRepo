using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moq;

namespace Odin.Tests.Extensions
{
    public static class ControllerExtensions
    {
        public static void MockControllerContextForUser(this Controller controller, string userId)
        {
            var controllerContext = new Mock<ControllerContext>();
            var claim = new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", userId);

            var claimsIdentity = new ClaimsIdentity(new[] { claim });
            var principal = new Moq.Mock<IPrincipal>();
            principal.SetupGet(x => x.Identity).Returns(claimsIdentity);
            controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);

            controller.ControllerContext = controllerContext.Object;
        }
    }
}
