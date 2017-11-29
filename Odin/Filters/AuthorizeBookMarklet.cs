using System.Web.Mvc;

namespace Odin.Filters
{
    public class AuthorizeBookMarklet : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //filterContext.Result = new ViewResult
            //{
            //    ViewName = "Unauthorized"
            //};

            filterContext.Result = new ViewResult()
            {
                ViewName = "~/views/bookmarklet/Error.cshtml"
            };

        }
    }
}