using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsApplication.Library.Database;
using NewsApplication.Models;
using NewsApplication.Exception;

namespace NewsApplication.Controllers
{
    public class UserController : Controller
    {
        public UserController()
        {

        }
        [HttpGet]
        public ActionResult Index()
        {
            MySQLUtility connection = new MySQLUtility();
            try
            {
                connection.Connect();
                Authenticate auth = new Authenticate(connection);

                User user = auth.GetUser();

                if (user.IsLogin())
                {
                    return View(user);
                }
                else
                {
                    ViewBag.ErrorMessage = "Bạn chưa đăng nhập";
                    return View("_Error");
                }
            }
            catch (DBException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("_Error");
            }
        }
        [HttpPost]
        public ActionResult Index(User input)
        {
            MySQLUtility connection = new MySQLUtility();
            User user = new User();
            try
            {
                connection.Connect();
                Authenticate authenticate = new Authenticate(connection);

                user = authenticate.GetUser();

                if (user.IsLogin())
                {
                    input.SetConnection(connection);

                    //Thao tác đặt giá trị mặc định
                    //Giá trị cho phép rỗng lastname
                    input.lastname = input.lastname == null ? "" : input.lastname;


                    input.CheckValidForFirstname().CheckValidForLastname().CheckValidForFirstname().CheckValidForPhone().CheckValidForEmail().CheckValidForGender();


                    if(input.GetErrorsMap().Count() != 0)
                    {
                        throw new InputException(1, input.GetErrorsMap());
                    }

                    input.password = Request.Cookies["password"].Value;
                    input.role = user.role;
                    user.Update(input);

                    ViewBag.SuccessMessage = "Cập nhật thông tin thành công";
                    return View(user);
                }
                else
                {
                    ViewBag.ErrorMessage = "Bạn chưa đăng nhập";
                    return View("_Error");
                }
            }catch(DBException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View(user);
            }catch(InputException e)
            {
                ViewBag.ErrorsMap = e.Errors;
                return View(user);
            }
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            MySQLUtility connection = new MySQLUtility();
            try
            {
                connection.Connect();

                Authenticate authenticate = new Authenticate(connection);

                User user = authenticate.GetUser();

                if (user.IsLogin())
                {
                    return View(user);
                }
                else
                {
                    ViewBag.ErrorMessage = "Bạn chưa đăng nhập không thể thay đổi mật khẩu";
                    return View("_Error");
                }
            }catch(DBException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("_Error");
            }
        }
        [HttpPost]
        public ActionResult ChangePassword(string[] password)
        {
            MySQLUtility connection = new MySQLUtility();
            User user = null;

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

                user = authenticate.GetUser();

                if (user.IsLogin())
                {
                    user.SetConnection(connection);

                    if(password == null || password.Length != 2 || password[0] != password[1])
                    {
                        user.AddErrorMessage("password", "Mật khẩu mới không hợp lệ!");
                    }

                    if (user.GetErrorsMap().Count()!=0)
                    {
                        throw new InputException(1, user.GetErrorsMap());
                    }

                    user.password = password[0];

                    user.CheckValidForPassword();

                    if(user.GetErrorsMap().Count() != 0)
                    {
                        throw new InputException(1, user.GetErrorsMap());
                    }

                    user.ChangePassword(password[0]);

                    ViewBag.SuccessMessage = "Đã cập nhật mật khẩu thành công!";

                    HttpCookie cpassword = Request.Cookies["password"];
                    cpassword.Value = password[0];
                    Response.Cookies.Add(cpassword);

                    return View(user);
                }
                else
                {
                    ViewBag.ErrorMessage = "Bạn chưa đăng nhập";
                    return View("_Error");
                }
            }catch(DBException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View(user);
            }catch(InputException e)
            {
                ViewBag.ErrorsMap = e.Errors;
                return View(user);
            }
        }
        [HttpGet]
        public ActionResult Register()
        {
            MySQLUtility connection = new MySQLUtility();
            try
            {
                connection.Connect();
                Authenticate auth = new Authenticate(connection);

                User user = auth.GetUser();

                if (user.IsLogin())
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View();
                }
            }
            catch (DBException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("_Error");
            }
        }
        [HttpPost]
        public ActionResult Register(User input, string[] password)
        {
            MySQLUtility connection = new MySQLUtility();
            try
            {
                connection.Connect();

                Authenticate auth = new Authenticate(connection);
                User user = auth.GetUser();

                if (user.IsLogin())
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    input.SetConnection(connection);
                    if(password == null || password.Length != 2 || password[0] != password[1])
                    {
                        input.AddErrorMessage("retypepassword", "Nhập lại mật khẩu không trùng khớp!");
                        input.password = password[0];
                    }

                    if (!input.CheckValid())
                    {
                        throw new InputException(1, input.GetErrorsMap());
                    }

                    if (input.Register())
                    {
                        HttpCookie cusername = new HttpCookie("username", input.username);
                        cusername.Expires = DateTime.Now.AddMonths(1);
                        cusername.HttpOnly = true;
                        HttpCookie cpassword = new HttpCookie("password", input.password);
                        cpassword.Expires = DateTime.Now.AddMonths(1);
                        cpassword.HttpOnly = true;

                        Response.Cookies.Add(cusername);
                        Response.Cookies.Add(cpassword);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        throw new InputException(1, null, "Đăng ký tài khoản thất bại. Vui lòng thử lại!");
                    }
                }
            }catch(DBException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("_Error");
            }catch(InputException e)
            {
                ViewBag.ErrorMessage = e.Message;
                ViewBag.ErrorsMap = e.Errors;
                return View(input);
            }
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
                ViewBag.ErrorMessage = e.Message;
                return View("_Error");
            }

            User user = new Authenticate(connection).GetUser();

            if (user.IsLogin())
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if(input.username != null && input.password != null)
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
                            cusername.Expires = DateTime.Now.AddMonths(1);
                            Response.Cookies.Add(cusername);
                            HttpCookie cpassword = new HttpCookie("password", password);
                            cpassword.HttpOnly = true;
                            cpassword.Expires = DateTime.Now.AddMonths(1);
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
        public ActionResult Logout()
        {
            HttpCookie cusername = new HttpCookie("username");
            cusername.Expires = DateTime.Now.AddMonths(-1);
            cusername.HttpOnly = true;

            HttpCookie cpassword = new HttpCookie("password");
            cpassword.Expires = DateTime.Now.AddMonths(-1);
            cpassword.HttpOnly = true;

            Response.Cookies.Add(cusername);
            Response.Cookies.Add(cpassword);

            return RedirectToAction("Index", "Home");
        }
    }
}