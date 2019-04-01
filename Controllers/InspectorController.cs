using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsApplication.Controllers
{
    public class InspectorController : Controller
    {
        // GET: Inspector
        public ActionResult Index()
        {
            return View();
        }
    }
}