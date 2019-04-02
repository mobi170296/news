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
                    //Đặt giá trị cho biến chấp nhận rỗng null => ""
                    category.link = category.link == null ? "" : category.link;
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
                    Category category = new Category(connection);
                    category.id = (int)id;
                    if(id == null || !category.Load())
                    {
                        TempData["ErrorMessage"] = "Danh mục tin tức không tồn tại";
                        return RedirectToAction("Index");
                    }
                    ViewBag.oldname = category.name;
                    return View(category);
                }
                else
                {
                    ViewBag.ErrorMessage = "Bạn không thể truy cập trang này";
                    return View("_Error");
                }

            }catch(DBException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View();
            }
        }
        [HttpPost]
        public ActionResult Update(int? id, Category model)
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



            Category category = new Category(connection);
            try
            {
                Authenticate authenticate = new Authenticate(connection);
                User user = authenticate.GetUser();

                if(user.IsLogin() && user.HaveRole(NewsApplication.Models.User.ADMIN))
                {
                    category.id = (int)id;
                    if(id != null && category.Load())
                    {
                        //Viết lại giá trị cho rỗng <=> null MVC FW
                        model.link = model.link == null ? "" : model.link;

                        model.CheckValidForLink().CheckValidForName();
                        if(model.GetErrorsMap().Count == 0)
                        {
                            category.Update(model);

                            ViewBag.SuccessMessage = "Cập nhật thành công danh mục";
                            ViewBag.oldname = category.name;
                            return View(model);
                        }
                        else
                        {
                            throw new InputException(1, model.GetErrorsMap());
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Danh mục không tồn tại";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Bạn không có quyền truy cập";
                    return View("_Error");
                }
            }catch(DBException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return RedirectToAction("Index");
            }catch(InputException e)
            {
                ViewBag.ErrorsMap = e.Errors;
                ViewBag.oldname = category.name;
                return View(model);
            }
        }
        [HttpGet]
        public ActionResult Delete(int? id)
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

            try
            {
                Authenticate authenticate = new Authenticate(connection);
                User user = authenticate.GetUser();

                if(user.IsLogin() && user.HaveRole(NewsApplication.Models.User.ADMIN))
                {
                    Category category = new Category(connection);
                    category.id = (int)id;
                    if(id == null || !category.Load())
                    {
                        TempData["ErrorMessage"] = "ID của danh mục không hợp lệ";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(category);
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Bạn không có quyền truy cập";
                    return View("_error");
                }

            }catch(DBException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("_Error");
            }
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            MySQLUtility connection = new MySQLUtility();

            try
            {
                connection.Connect();
            }catch(DBException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("_error");
            }

            User user = null;

            try
            {
                Authenticate authenticate = new Authenticate(connection);
                user = authenticate.GetUser();

                if(user.IsLogin() && user.HaveRole(NewsApplication.Models.User.ADMIN))
                {
                    Category category = new Category(connection);
                    category.id = id;
                    if(!category.Load())
                    {
                        TempData["ErrorMessage"] = "Danh mục không tồn tại";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        category.Delete();
                        TempData["SuccessMessage"] = "Xóa thành công";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Bạn không thể truy cập trang này";
                    return View("_Error");
                }
            }catch(DBException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("_Error");
            }
        }
    }
}