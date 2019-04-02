using NewsApplication.Library.Database;
using NewsApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;

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

                List<int> ids = new List<int>();

                using (MySqlDataReader result = (MySqlDataReader)connection.select("*").from("category").Execute())
                {
                    while (result.Read())
                    {
                        ids.Add(result.GetInt32("id"));
                    }
                }

                List<Category> categories = new List<Category>();

                foreach(int id in ids)
                {
                    Category cate = new Category(connection);
                    cate.id = id;
                    cate.Load();
                    categories.Add(cate);
                }

                ViewBag.categories = categories;

                return PartialView();
            }
            catch (DBException e)
            {
                return Content("Không thể load heading. Vui lòng tải lại trang web! " + e.Message);
            }
        }
        public PartialViewResult Footer()
        {
            return PartialView();
        }
    }
}