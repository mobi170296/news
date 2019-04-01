﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewsApplication.Library.Database;
using NewsApplication.Exception;
using System.Text.RegularExpressions;
using System.Data;
using MySql.Data.MySqlClient;

namespace NewsApplication.Models
{
    public class User
    {
        const int ADMIN = 1;
        const int NORMAL = 2;
        public int id { get; set; }
        public int role { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public SortedList<string, string> errorsmap;
        private IDatabaseUtility connection;
        public User()
        {
            this.id = -1;
            this.errorsmap = new SortedList<string, string>();
        }
        public User(IDatabaseUtility connection) : this()
        {
            this.connection = connection;
        }
        public void SetConnection(IDatabaseUtility connection)
        {
            this.connection = connection;
        }
        public bool IsLogin()
        {
            return this.id != -1;
        }
        public bool HaveRole(int role)
        {
            return this.role == role;
        }
        public bool CheckValid()
        {
            errorsmap.Clear();
            //Check username: Min: 6, Max: 50 Regex
            if(this.username == null || !Regex.IsMatch(this.username, "^[A-z0-9_]+$"))
            {
                errorsmap["username"] = "Username không hợp lệ, phải có từ 6 đến 50 ký tự và chỉ chứa a-Z, 0-9, _";
            }
            else
            {
                using(MySqlDataReader result = (MySqlDataReader)this.connection.select("*").from("user").where("username=" + new DBString(this.username).SqlValue()).Execute())
                {
                    if (result.HasRows)
                    {
                        errorsmap["username"] = "Tên đăng nhập này đã tồn tại!";
                    }
                }
            }

            if(this.firstname == null || !Regex.IsMatch(this.firstname, @"^(\p{L}| )+$"))
            {
                errorsmap["firstname"] = "Tên không hợp lệ";
            }

            //Check password: min: 6, max: unlimited
            if(this.password == null || this.password.Length < 6)
            {
                errorsmap["password"] = "Mật khẩu không hợp lệ, phải có từ 6 ký tự trở lên";
            }

            //Check email: Regex
            if(this.email == null || !Regex.IsMatch(this.email, @"^([A-z0-9]+\.)*[A-z0-9]+@[A-z0-9]{3,}\.[A-z0-9]{2,}$"))
            {
                errorsmap["email"] = "Địa chỉ email không hợp lệ";
            }

            //Check phone: Regex
            if(this.phone == null || !Regex.IsMatch(this.phone, @"^0\d{9,10}$"))
            {
                errorsmap["phone"] = "Số điện thoại không hợp lệ";
            }

            if(this.errorsmap.Count() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Standardization()
        {
            if(this.username != null)
            {
                this.username = this.username.Replace("\\", "\\\\").Replace("'", "\\'");
            }
            if(this.phone!=null)
            this.phone = this.phone.Replace("\\", "\\\\").Replace("'", "\\'");
            if(this.email != null)
            this.email = this.email.Replace("\\", "\\\\").Replace("'", "\\'");
            if(this.password != null)
            this.password = this.password.Replace("\\", "\\\\").Replace("'", "\\'");
        }
        //Add method
        public bool Register()
        {
            //username, password, role, email, phone
            this.Standardization();
            return this.connection.Insert("user", new SortedList<string, IDBDataType>
            {
                {"username", new DBString(this.username) },
                {"password", new DBRaw("md5(" + new DBString(this.password).SqlValue() + ")") },
                {"phone", new DBString(this.phone) },
                {"email", new DBString(this.email) },
                {"role", new DBNumber(User.NORMAL) },
                {"lastname", new DBString(this.lastname) },
                {"firstname", new DBString(this.firstname) }
            }) != 0;
        }
        public bool Login()
        {
            HttpCookie cusername = HttpContext.Current.Request.Cookies["username"];
            HttpCookie cpassword = HttpContext.Current.Request.Cookies["password"];
            if(cusername == null || cpassword == null)
            {
                return false;
            }
            else
            {
                return this.Login(cusername.Value, cpassword.Value);
            }
        }
        public bool Login(string username, string password)
        {
            this.username = username;
            this.password = password;
            this.Standardization();

            using (MySqlDataReader result = (MySqlDataReader)this.connection.select("*").from("user").where("username=" + new DBString(this.username).SqlValue() + " and password=" + new DBRaw("md5('" + this.password + "')").SqlValue()).Execute())
            {
                if (result.Read())
                {
                    this.id = result.GetInt32("id");
                    this.email = result.GetString("email");
                    this.password = result.GetString("password");
                    this.username = result.GetString("username");
                    this.role = result.GetInt32("role");
                    this.firstname = result.GetString("firstname");
                    this.lastname = result.GetString("lastname");
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool Update(User newdata)
        {
            newdata.Standardization();
            this.connection.Update("user", new SortedList<string, IDBDataType>
            {
                {"password", new DBRaw("password('" + this.password + "')") },
                {"role", new DBNumber(this.role) },
                {"email", new DBString(this.email) },
                {"phone", new DBString(this.phone) },
                {"firstname", new DBString(this.firstname) },
                {"lastname", new DBString(this.lastname) }
            }, "id=" + this.id);
            return true;
        }
        public bool Delete()
        {
            this.connection.Delete("user", "id=" + this.id);
            return true;
        }
        public SortedList<string,string> GetErrorsMap()
        {
            return this.errorsmap;
        }
        public string GetErrorMessage(string name)
        {
            if (this.errorsmap.ContainsKey(name))
            {
                return this.errorsmap[name];
            }
            else
            {
                return null;
            }
        }
    }
}