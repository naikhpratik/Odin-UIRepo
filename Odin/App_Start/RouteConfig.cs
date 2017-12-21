using System.Web.Mvc;
using System.Web.Routing;

namespace Odin
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Role", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "Email",
               url: "{controller}/{action}/{id}/{email}",
               defaults: new { controller = "Orders", action = "EmailGeneratedPDF", id = UrlParameter.Optional, email = UrlParameter.Optional}
           );
        }
    }
}
