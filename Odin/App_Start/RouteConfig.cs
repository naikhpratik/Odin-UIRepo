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
                defaults: new { controller = "Orders", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "OrderHomes",
                url: "{controller}/{action}/{Orderid}/{HomeId}",
                defaults: new { controller = "Orders", action = "Index", Orderid = UrlParameter.Optional, Homeid = UrlParameter.Optional }
            );
        }
    }
}
