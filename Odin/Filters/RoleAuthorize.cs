using System.Web.Mvc;

namespace Odin.Filters
{
    public class RoleAuthorize : AuthorizeAttribute
    {
        public RoleAuthorize(params string[] roles) : base()
        {
            var roleStr = string.Empty;
            foreach (var role in roles)
            {
                roleStr += role + ',';
            }

           this.Roles = roleStr.TrimEnd(',');
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            // If they are authorized, handle accordingly
            if (this.AuthorizeCore(filterContext.HttpContext))
            {
                base.OnAuthorization(filterContext);
            }
            else
            {
                // Otherwise redirect to your specific authorized area
                //filterContext.Result = new RedirectResult("~/Errors/404.html");
                //filterContext.Result = new HttpNotFoundResult();
                filterContext.Result = new RedirectResult("~/Error/Error404");
            }
        }
    }
}