using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Odin.Data.Core;

namespace Odin.Controllers
{
    [Authorize]
    public class TransfereesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransfereesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Transferees
        public ViewResult Index()
        {
            return View();
        }
    }
}