using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsApplication.Models;
using NewsApplication.Library.Database;

namespace News.Controllers
{
    public class PostsController : Controller
    {
        //controller cho việc trình diễn nội dung tin tức
        public ActionResult Index(int? id)
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
                if (id == null)
                {
                    ViewBag.ErrorMessage = "Trang tin này không tồn tại!";
                    return View("_errors");
                }
                else
                {
                    Post post = new Post(connection);
                    post.id = (int)id;
                    if (post.Load())
                    {
                        if (post.IsShown())
                        {
                            return View(post);
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Trang này không tồn tại!";
                            return View("_errors");
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Trang nay không tồn tại!";
                        return View("_errors");
                    }
                }
            }
            catch (DBException e)
            {
                ViewBag.ERrorMessage = e.Message;
                return View("_errors");
            }


        }
	}
}