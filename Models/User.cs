using System;
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
        public const int ADMIN = 1;
        public const int NORMAL = 2;
        public const int JOURNALIST = 3;
        public const int INSPECTOR = 4;
        public int id { get; set; }
        public int role { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public int gender { get; set; }
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
            //Check username: Min: 6, Max: 50 Regex
            if (this.username == null || !Regex.IsMatch(this.username, "^[A-z0-9_]+$"))
            {
                errorsmap["username"] = "Username không hợp lệ, phải có từ 6 đến 50 ký tự và chỉ chứa a-Z, 0-9, _";
            }
            else
            {
                using (MySqlDataReader result = (MySqlDataReader)this.connection.select("*").from("user").where("username=" + new DBString(this.username).SqlValue()).Execute())
                {
                    if (result.HasRows)
                    {
                        errorsmap["username"] = "Tên đăng nhập này đã tồn tại!";
                    }
                }
            }

            //check lastname
            if (this.lastname == null || !Regex.IsMatch(this.lastname, @"^(\p{L}| ){0,50}$"))
            {
                errorsmap["lastname"] = "Họ không hợp lệ";
            }

            //check firstname
            if (this.firstname == null || !Regex.IsMatch(this.firstname, @"^(\p{L}| ){1,50}$"))
            {
                errorsmap["firstname"] = "Tên không hợp lệ";
            }

            //Check password: min: 6, max: unlimited
            if (this.password == null || this.password.Length < 6)
            {
                errorsmap["password"] = "Mật khẩu không hợp lệ, phải có từ 6 ký tự trở lên";
            }

            //Check email: Regex
            if (this.email == null || !Regex.IsMatch(this.email, @"^([A-z0-9]+\.)*[A-z0-9]+@[A-z0-9]{3,}\.[A-z0-9]{2,}$"))
            {
                errorsmap["email"] = "Địa chỉ email không hợp lệ";
            }

            //Check phone: Regex
            if (this.phone == null || !Regex.IsMatch(this.phone, @"^0\d{9,10}$"))
            {
                errorsmap["phone"] = "Số điện thoại không hợp lệ";
            }

            //check gender
            if (this.gender != 0 && this.gender != 1)
            {
                errorsmap["gender"] = "Giới tính không hợp lệ!";
            }

            if (this.errorsmap.Count() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public User CheckValidForFirstname()
        {
            //check firstname
            if (this.firstname == null || !Regex.IsMatch(this.firstname, @"^(\p{L}| ){1,50}$"))
            {
                errorsmap["firstname"] = "Tên không hợp lệ";
            }
            return this;
        }
        public User CheckValidForLastname()
        {
            if (this.lastname == null || !Regex.IsMatch(this.lastname, @"^(\p{L}| ){0,50}$"))
            {
                errorsmap["lastname"] = "Họ không hợp lệ";
            }
            return this;
        }
        public User CheckValidForUsername()
        {
            if (this.username == null || !Regex.IsMatch(this.username, "^[A-z0-9_]+$"))
            {
                errorsmap["username"] = "Username không hợp lệ, phải có từ 6 đến 50 ký tự và chỉ chứa a-Z, 0-9, _";
            }
            else
            {
                using (MySqlDataReader result = (MySqlDataReader)this.connection.select("*").from("user").where("username=" + new DBString(this.username).SqlValue()).Execute())
                {
                    if (result.HasRows)
                    {
                        errorsmap["username"] = "Tên đăng nhập này đã tồn tại!";
                    }
                }
            }
            return this;
        }
        public User CheckValidForPassword()
        {
            //Check password: min: 6, max: unlimited
            if (this.password == null || this.password.Length < 6)
            {
                errorsmap["password"] = "Mật khẩu không hợp lệ, phải có từ 6 ký tự trở lên";
            }
            return this;
        }
        public User CheckValidForEmail()
        {
            //Check email: Regex
            if (this.email == null || !Regex.IsMatch(this.email, @"^([A-z0-9]+\.)*[A-z0-9]+@[A-z0-9]{3,}\.[A-z0-9]{2,}$"))
            {
                errorsmap["email"] = "Địa chỉ email không hợp lệ";
            }
            return this;
        }
        public User CheckValidForPhone()
        {

            //Check phone: Regex
            if (this.phone == null || !Regex.IsMatch(this.phone, @"^0\d{9,10}$"))
            {
                errorsmap["phone"] = "Số điện thoại không hợp lệ";
            }
            return this;
        }
        public User CheckValidForGender()
        {
            //check gender
            if (this.gender != 0 && this.gender != 1)
            {
                errorsmap["gender"] = "Giới tính không hợp lệ!";
            }
            return this;
        }
        public void Standardization()
        {
            if (this.username != null)
            {
                this.username = this.username.Replace("\\", "\\\\").Replace("'", "\\'");
            }
            if (this.phone != null)
            {
                this.phone = this.phone.Replace("\\", "\\\\").Replace("'", "\\'");
            }
            if (this.email != null)
            {
                this.email = this.email.Replace("\\", "\\\\").Replace("'", "\\'");
            }
            if (this.password != null)
            {
                this.password = this.password.Replace("\\", "\\\\").Replace("'", "\\'");
            }
            if (this.firstname != null)
            {
                this.firstname = this.firstname.Replace("\\", "\\\\").Replace("'", "\\'");
            }
            if (this.lastname != null)
            {
                this.lastname = this.lastname.Replace("\\", "\\\\").Replace("'", "\\'");
            }
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
                {"firstname", new DBString(this.firstname) },
                {"gender", new DBNumber(this.gender) }
            }) != 0;
        }
        public bool Login()
        {
            HttpCookie cusername = HttpContext.Current.Request.Cookies["username"];
            HttpCookie cpassword = HttpContext.Current.Request.Cookies["password"];
            if (cusername == null || cpassword == null)
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
                    this.gender = result.GetInt32("gender");
                    this.phone = result.GetString("phone");
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool ChangePassword(string password)
        {
            password = password.Replace("\\", "\\\\").Replace("'", "\\'");
            this.connection.Update("user", new SortedList<string, IDBDataType>
            {
                {"password", new DBRaw("md5(" + new DBString(password).SqlValue() + ")")}
            }, "id=" + new DBNumber(this.id).SqlValue());
            return true;
        }
        public bool Update(User newdata)
        {
            newdata.Standardization();
            this.connection.Update("user", new SortedList<string, IDBDataType>
            {
                {"password", new DBRaw("md5(" + new DBString(newdata.password).SqlValue() + ")") },
                {"role", new DBNumber(newdata.role) },
                {"email", new DBString(newdata.email) },
                {"phone", new DBString(newdata.phone) },
                {"firstname", new DBString(newdata.firstname) },
                {"lastname", new DBString(newdata.lastname) },
                {"gender", new DBNumber(newdata.gender) }
            }, "id=" + this.id);

            this.role = newdata.role;
            this.email = newdata.email;
            this.phone = newdata.phone;
            this.firstname = newdata.firstname;
            this.lastname = newdata.lastname;
            this.gender = newdata.gender;

            return true;
        }
        public bool Delete()
        {
            this.connection.Delete("user", "id=" + new DBNumber(this.id).SqlValue());
            return true;
        }
        public SortedList<string, string> GetErrorsMap()
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
        public void AddErrorMessage(string name, string value)
        {
            this.errorsmap[name] = value;
        }
        public bool Load()
        {
            using (IDataReader result = this.connection.select("*").from("user").where("id=" + this.id).Execute())
            {
                if (!result.Read())
                {
                    return false;
                }
                this.username = (string)result["username"];
                this.phone = (string)result["phone"];
                this.firstname = (string)result["firstname"];
                this.lastname = (string)result["lastname"];
                this.role = (int)result["role"];
                this.gender = (int)result["gender"];
                this.email = (string)result["email"];
            }

            return true;
        }
    }
}