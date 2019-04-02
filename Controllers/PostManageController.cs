using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsApplication.Models;
using NewsApplication.Library.Database;
using MySql.Data.MySqlClient;
using NewsApplication.Exception;
using System.Security.Cryptography;
using System.Text;

namespace NewsApplication.Controllers
{
    public class PostManageController : Controller
    {
        // GET: PostManage
        public ActionResult Index(int page = 1)
        {
            IDatabaseUtility connection = new MySQLUtility();

            try
            {
                connection.Connect();
            }catch(DBException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("_errors");
            }

            try
            {
                Authenticate authenticate = new Authenticate(connection);
                User user = authenticate.GetUser();

                if (user.IsLogin() && user.HaveRole(NewsApplication.Models.User.JOURNALIST))
                {
                    PostListModel list = new PostListModel(connection);
                    ViewBag.posts = list.GetLimit((page - 1) * 10, 10);
                    int total = list.GetTotal();
                    ViewBag.pagepartition = new PagePartitionModel("Index", "PostManage", page, (int)Math.Ceiling(total /10.0));
                    return View();
                }
                else
                {
                    ViewBag.ErrorMessage = "Bạn không có quyền truy cập";
                    return View("_errors");
                }
            }catch(DBException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("_errors");
            }
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
                return View("_errors");
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
                    return View("_errors");
                }
            }catch(DBException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View();
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(Post post, HttpPostedFileBase poster)
        {
            IDatabaseUtility connection = new MySQLUtility();

            try
            {
                connection.Connect();
            }catch(DBException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("_errors");
            }


            try
            {
                Authenticate authenticate = new Authenticate(connection);
                User user = authenticate.GetUser();

                if (user.IsLogin() && user.HaveRole(NewsApplication.Models.User.JOURNALIST))
                {
                    if(poster == null)
                    {
                        post.AddErrorMessage("poster", "Bạn chưa chọn ảnh bìa cho tin tức");
                    }
                    post.SetConnection(connection);
                    ViewBag.categories = new CategoryListModel(connection).GetAll();

                    post.CheckValidForSummary().CheckValidForContent().CheckValidForTitle().CheckValidForCategoryId();

                    if(post.GetErrorsMap().Count() == 0)
                    {
                        PostImage image = new PostImage(connection);
                        post.valid = 0;
                        post.journalist_id = user.id;
                        post.Add();
                        image.post_id = (int)connection.GetLastInsertedId();
                        image.path = "" + image.post_id + "_" + new Random().Next() + System.IO.Path.GetExtension(poster.FileName);
                        image.Add();

                        poster.SaveAs(Server.MapPath(PostImage.POSTER_IMAGE_DIR + image.path));
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
                    return View("_errors");
                }
            }catch(DBException e)
            {
                ViewBag.ErrorMessage = "" + e.Message;
                return View("_errors");
            }catch(InputException e)
            {
                ViewBag.ErrorsMap = e.Errors;
                return View();
            }
        }
        [HttpGet]
        public ActionResult Update(int? id)
        {
            return View();
        }
    }
}