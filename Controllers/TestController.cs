using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsApplication.Controllers
{
    public class Model
    {
        public int id;
    }
    public class TestController : Controller
    {
        // GET: Test
        [HttpPost]
        public ActionResult Index(Model model)
        {
            return Content("");
        }
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}