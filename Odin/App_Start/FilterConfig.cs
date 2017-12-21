using System.Web.Mvc;
using Odin.Filters;

namespace Odin
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new AiHandleErrorAttribute());
            filters.Add(new RequireHttpsAttribute());
        }
    }
}
