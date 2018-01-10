using Moq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Http;
using System.Web.Mvc;

namespace Odin.Tests.Extensions
{
    public static class ControllerExtensions
    {
        public static string FirstName { get; set; }
        public static string LastName { get; set; }

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

        public static void MockControllerContextForUserAndRole(this Controller controller, string userId, string role)
        {
            var controllerContext = new Mock<ControllerContext>();
            var claim = new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", userId);

            var claimsIdentity = new ClaimsIdentity(new[] { claim });
            var principal = new Moq.Mock<IPrincipal>();

            principal.SetupGet(x => x.Identity).Returns(claimsIdentity);
            principal.Setup(x => x.IsInRole(role)).Returns(true);
            controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);

            controller.ControllerContext = controllerContext.Object;
        }

        public static string FullName => $"{FirstName} {LastName}";

        public static void MockCurrentUser(this ApiController controller, string userId, string username)
        {
            var identity = new GenericIdentity(username);
            identity.AddClaim(
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name",
                    username));

            identity.AddClaim(
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                    userId));

            identity.AddClaim(new Claim("FullName", FullName));

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

            //identity.AddClaim(new Claim(ClaimTypes.Role, roleName));

            identity.AddClaim(new Claim("FullName", FullName));

            var principal = new GenericPrincipal(identity, new string[]{ roleName });

            controller.ControllerContext.RequestContext.Principal = principal;
        }
    }
}
