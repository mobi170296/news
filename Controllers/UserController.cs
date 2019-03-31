using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsApplication.Library.Database;
using NewsApplication.Models;
namespace NewsApplication.Controllers
{
    public class UserController : Controller
    {
        public UserController()
        {

        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            IDatabaseUtility connection = new MySQLUtility();
            try
            {
                connection.Connect();
            }catch(DBException e)
            {
                ViewBag.error = e.Message;
                return View("_Error");
            }

            User user = new Authenticate(connection).GetUser();

            if (user.IsLogin())
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        [HttpPost]
        public ActionResult Login(User input)
        {
            IDatabaseUtility connection = new MySQLUtility();
            try
            {
                connection.Connect();
            }
            catch(DBException e)
            {
                return View("~/Views/Shared/_Error.cshtml");
            }

            User user = new Authenticate(connection).GetUser();

            if (user.IsLogin())
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if(input.username!=null && input.password != null)
                {
                    string username = input.username;
                    string password = input.password;
                    try
                    {
                        input.SetConnection(connection);
                        if(input.Login(input.username, input.password))
                        {
                            HttpCookie cusername = new HttpCookie("username", username);
                            cusername.HttpOnly = true;
                            cusername.Expires.AddMonths(1);
                            Response.Cookies.Add(cusername);
                            HttpCookie cpassword = new HttpCookie("password", password);
                            cpassword.HttpOnly = true;
                            cpassword.Expires.AddMonths(1);
                            Response.Cookies.Add(cpassword);
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ViewBag.error = "Tên đăng nhập hoặc tài khoản không đúng";
                            return View();
                        }
                    }catch(DBException e)
                    {
                        ViewBag.error = e.Message;
                        return View("~/Views/Shared/_Error.cshtml");
                    }
                }
                else
                {
                    ViewBag.error = "Tên đăng nhập hoặc tài khoản không hợp lệ";
                    return View();
                }
            }
        }
    }
}