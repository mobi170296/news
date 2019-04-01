using NewsApplication.Library.Database;
using NewsApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsApplication.Controllers
{
    public class LayoutController : Controller
    {
        // GET: Layout
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Header()
        {
            try
            {
                MySQLUtility connection = new MySQLUtility();
                connection.Connect();

                Authenticate auth = new Authenticate(connection);

                User user = auth.GetUser();

                ViewBag.user = user;

                //ViewBag.categories = ;;

                return PartialView();
            }
            catch (DBException e)
            {
                return Content("Không thể load heading. Vui lòng tải lại trang web!");
            }
        }
        public PartialViewResult Footer()
        {
            return PartialView();
        }
    }
}