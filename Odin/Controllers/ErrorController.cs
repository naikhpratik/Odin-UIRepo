using System.Web.Mvc;

namespace Odin.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Index()
        {
            return View();
        }

        // GET: Error
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Error404()
        {
            return View();
        }
    }
}