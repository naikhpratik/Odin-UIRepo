using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Odin.Controllers
{
    [Authorize]
    public class TransfereesController : Controller
    {
        // GET: Transferees
        public ActionResult Index()
        {
            return View();
        }
    }
}