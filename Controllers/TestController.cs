using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsApplication.Controllers
{
    public class Model
    {
        public string password;
    }
    public class TestController : Controller
    {
        // GET: Test
        [HttpPost]
        public ActionResult Index(Model model, string[] password, string id)
        {
            return Content("" + password[0] + "; " + password[1]);
        }
        [HttpGet]
        public ActionResult Index()
        {
            HttpCookie cookie = new HttpCookie("name", "Trịnh Văn Linh");
            cookie.Expires = DateTime.Now.AddMonths(3);
            cookie.HttpOnly = true;
            Response.Cookies.Add(cookie);
            return Content("OK");
        }
    }
}