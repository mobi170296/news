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
    public class PostManageController : Controller
    {
        // GET: PostManage
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Add()
        {
            IDatabaseUtility connection = new MySQLUtility();
            try
            {
                connection.Connect();
            }
            catch (DBException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("_error");
            }

            try
            {
                Authenticate authenticate = new Authenticate(connection);
                User user = authenticate.GetUser();

                if(user.IsLogin() && user.HaveRole(NewsApplication.Models.User.JOURNALIST))
                {
                    List<int> ids = new List<int>();
                    using(MySqlDataReader result = (MySqlDataReader)connection.select("*").from("category").Execute())
                    {
                        while(result.Read())
                            ids.Add(result.GetInt32("id"));
                    }

                    ViewBag.categories = new List<Category>();

                    foreach(int id in ids)
                    {
                        Category cate = new Category(connection);
                        cate.id = id;
                        cate.Load();
                        ViewBag.categories.Add(cate);
                    }
                    return View();
                }
                else
                {
                    ViewBag.ErrorMessage = "Bạn không có quyền truy cập vào đây";
                    return View("_error");
                }
            }catch(DBException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View();
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(Post post)
        {
            IDatabaseUtility connection = new MySQLUtility();

            try
            {
                connection.Connect();
            }catch(DBException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("_error");
            }


            try
            {
                Authenticate authenticate = new Authenticate(connection);
                User user = authenticate.GetUser();

                if (user.IsLogin() && user.HaveRole(NewsApplication.Models.User.JOURNALIST))
                {
                    post.SetConnection(connection);
                    ViewBag.categories = new CategoryListModel(connection).GetAll();

                    post.CheckValidForSummary().CheckValidForContent().CheckValidForTitle().CheckValidForCategoryId();

                    if(post.GetErrorsMap().Count() == 0)
                    {
                        post.valid = 0;
                        post.journalist_id = user.id;
                        post.Add();
                        TempData["SuccessMessage"] = "Bạn đã đăng bài thành công hãy tìm nhà kiểm duyệt để duyệt bài của bạn và hiển thị nó";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.postback = post;
                        throw new InputException(1, post.GetErrorsMap());
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Bạn không thể truy cập trang web này";
                    return View("_error");
                }
            }catch(DBException e)
            {
                ViewBag.ErrorMessage = "" + e.Message;
                return View("_error");
            }catch(InputException e)
            {
                ViewBag.ErrorsMap = e.Errors;
                return View();
            }
        }
    }
}