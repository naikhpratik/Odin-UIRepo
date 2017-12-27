using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Moq;

namespace Odin.IntegrationTests.Extensions
{
    public static class ControllerExtensions
    {
        public static void MockCurrentUser(this Controller controller, string userId, string username)
        {
            var identity = new GenericIdentity(username);
            identity.AddClaim(
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name",
                    username));

            identity.AddClaim(
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                    userId));

            var principal = new GenericPrincipal(identity, null);

            controller.ControllerContext = Mock.Of<ControllerContext>(ctx =>
                ctx.HttpContext == Mock.Of<HttpContextBase>(http =>
                    http.User == principal));
        }

        public static void MockCurrentUser(this ApiController controller, string userId, string username)
        {
            var identity = new GenericIdentity(username);
            identity.AddClaim(
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name",
                    username));

            identity.AddClaim(
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                    userId));

            var principal = new GenericPrincipal(identity, null);

            controller.ControllerContext.RequestContext.Principal = principal;
        }

        public static void MockCurrentUserAndRole(this ApiController controller, string userId, string username, string roleName)
        {
            var identity = new GenericIdentity(username);
            identity.AddClaim(
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name",
                    username));

            identity.AddClaim(
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                    userId));

            identity.AddClaim(new Claim(ClaimTypes.Role, roleName));           

            var principal = new GenericPrincipal(identity, new string[] { "Transferee" });

            controller.ControllerContext.RequestContext.Principal = principal;
        }
    }
}
