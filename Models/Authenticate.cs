using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewsApplication.Library.Database;

namespace NewsApplication.Models
{
    public class Authenticate
    {
        IDatabaseUtility connection;
        User user;
        public Authenticate(IDatabaseUtility connection)
        {
            this.connection = connection;
            this.user = new User(this.connection);

            try
            {
                HttpCookie cusername = HttpContext.Current.Request.Cookies["username"];
                HttpCookie cpassword = HttpContext.Current.Request.Cookies["password"];

                if(cusername == null || cpassword == null)
                {
                    HttpCookie cun = new HttpCookie("username");
                    cun.Expires = DateTime.Now.AddMonths(-1);
                    cun.HttpOnly = true;
                    HttpContext.Current.Response.Cookies.Add(cun);
                    HttpCookie cpw = new HttpCookie("password");
                    cpw.Expires = DateTime.Now.AddMonths(-1);
                    cpw.HttpOnly = true;
                    HttpContext.Current.Response.Cookies.Add(cpw);
                }
                else
                {
                    string username = cusername.Value;
                    string password = cpassword.Value;

                    if(!this.user.Login(username, password))
                    {
                        HttpCookie cun = new HttpCookie("username");
                        cun.Expires = DateTime.Now.AddMonths(-1);
                        cun.HttpOnly = true;
                        HttpContext.Current.Response.Cookies.Add(cun);
                        HttpCookie cpw = new HttpCookie("password");
                        cpw.Expires = DateTime.Now.AddMonths(-1);
                        cpw.HttpOnly = true;
                        HttpContext.Current.Response.Cookies.Add(cpw);
                    }
                }
            }catch(DBException e)
            {
                string s = e.Message;
            }
        }
        public User GetUser()
        {
            return this.user;
        }
    }
}