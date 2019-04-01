using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using System.Collections;
using NewsApplication.Library.Database;
using NewsApplication.Models;

namespace NewsApplication.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            MySQLUtility connection = new MySQLUtility();
            try
            {
                connection.Connect();
            }catch(DBException ex)
            {
                return View("_Error");
            }
            Authenticate auth = new Authenticate(connection);
            User user = auth.GetUser();
            if (user.IsLogin())
            {
                return Content("Bạn đã đăng nhập");
            }
            else
            {
                return Content("Bạn chưa đăng nhập");
            }
        }
    }
}