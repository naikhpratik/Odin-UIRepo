using System.Web.Mvc;

namespace Odin.Controllers
{
    [Authorize]
    public class HelpController : Controller
    {
        // GET: Help
        public ActionResult Index()
        {
            ViewBag.PropBotScript = "javascript:"+System.IO.File.ReadAllText(Server.MapPath(@"~/Scripts/bookmarklet/dist.min.js"));
            return View();
        }
    }
}