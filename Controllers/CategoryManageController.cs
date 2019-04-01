using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsApplication.Models;
using NewsApplication.Library.Database;
using MySql.Data.MySqlClient;
using NewsApplication.Exception;

namespace NewsApplication.Controllers
{
    public class CategoryManageController : Controller
    {

        public ActionResult Index()
        {
            //Hien thi danh muc tin
            MySQLUtility connection = new MySQLUtility();
            try
            {
                connection.Connect();
            }catch(DBException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("_Error");
            }

            try
            {
                Authenticate authenticate = new Authenticate(connection);

                User user = authenticate.GetUser();


                if (user.IsLogin() && user.HaveRole(NewsApplication.Models.User.ADMIN))
                {
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
                        Category cate = new Category();
                        cate.SetConnection(connection);
                        cate.id = id;
                        cate.Load();
                        categories.Add(cate);
                    }

                    ViewBag.categories = categories;

                    return View();
                }
                else
                {
                    ViewBag.ErrorMessage = "Bạn không thể truy cập trang này";
                    return View("_Error");
                }
            }
            catch (DBException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View();
            }
        }
        [HttpGet]
        public ActionResult Add()
        {
            MySQLUtility connection = new MySQLUtility();
            try
            {
                connection.Connect();
            }
            catch(DBException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("_Error");
            }


            Authenticate authenticate = new Authenticate(connection);
            User user = authenticate.GetUser();

            if (user.IsLogin() && user.HaveRole(NewsApplication.Models.User.ADMIN))
            {
                return View();
            }
            else
            {
                ViewBag.Title = "Bạn không thể truy cập trang này";
                return View("_Error");
            }
        }
        [HttpPost]
        public ActionResult Add(Category category)
        {
            MySQLUtility connection = new MySQLUtility();
            try
            {
                connection.Connect();
            }catch(DBException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("_Error");
            }


            Authenticate authenticate = new Authenticate(connection);

            User user = authenticate.GetUser();

            try
            {
                if (user.IsLogin() && user.HaveRole(NewsApplication.Models.User.ADMIN))
                {
                    category.SetConnection(connection);
                    category.CheckValidForLink().CheckValidForName();
                    if (category.GetErrorsMap().Count == 0)
                    {
                        category.Add();
                        TempData["SuccessMessage"] = "Bạn đã tạo thành công danh mục " + category.name;
                        return RedirectToAction("Index", "CategoryManage");
                    }
                    else
                    {
                        throw new InputException(1, category.GetErrorsMap());
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Bạn không thể truy cập trang này";
                    return View("_Error");
                }
            }
            catch(DBException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View(category);
            }catch(InputException e)
            {
                ViewBag.ErrorsMap = e.Errors;
                return View(category);
            }
            
        }
        [HttpGet]
        public ActionResult Update(int? id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Update(int? id, Category model)
        {
            return View();
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Delete(Category model)
        {

            return View();
        }
    }
}