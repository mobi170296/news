using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewsApplication.Library.Database;
using MySql.Data.MySqlClient;

namespace NewsApplication.Models
{
    public class Category
    {
        public int id { get; set; }
        public string name { get; set; }
        public string link { get; set; }
        public IDatabaseUtility connection;
        public SortedList<string, string> errorsmap;
        public Category()
        {
            this.id = -1;
            this.errorsmap = new SortedList<string, string>();
        }
        public Category(IDatabaseUtility connection) : this()
        {
            this.connection = connection;
        }
        public Category CheckValidForName()
        {
            if(this.name == null || this.name.Length == 0 || this.name.Length > 200)
            {
                this.errorsmap["name"] = "Tên danh mục không hợp lệ";
            }
            return this;
        }
        public Category CheckValidForLink()
        {
            if(this.link == null || this.link.Length > 512)
            {
                this.errorsmap["link"] = "Địa chỉ liên kết không hợp lệ";
            }
            return this;
        }
        public void SetConnection(IDatabaseUtility connection)
        {
            this.connection = connection;
        }
        public void Standardization()
        {
            if (this.name != null)
            {
                this.name = this.name.Replace(@"\", @"\\").Replace(@"'", @"\'");
            }
            if (this.link != null)
            {
                this.link = this.link.Replace(@"\", @"\\").Replace(@"'", @"\'");
            }
        }
        //Function like Login of User class
        //Load input id attr
        public bool Load()
        {
            using (MySqlDataReader result = (MySqlDataReader)this.connection.select("*").from("category").where("id=" + new DBNumber(this.id).SqlValue()).Execute())
            {
                if (result.Read())
                {
                    this.name = result.GetString("name");
                    this.link = result.GetString("link");
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool Add()
        {
            this.Standardization();
            this.connection.Insert("category", new SortedList<string, IDBDataType>
            {
                {"name", new DBString(this.name) },
                {"link", new DBString(this.link) }
            });

            return true;
        }
        public bool Update(Category category)
        {
            category.Standardization();
            this.connection.Update("category", new SortedList<string, IDBDataType>
            {
                {"name", new DBString(category.name) },
                {"link", new DBString(category.link) }
            }, "id=" + new DBNumber(this.id).SqlValue());
            this.name = category.name;
            this.link = category.link;
            return true;
        }
        public bool Delete()
        {
            this.connection.Delete("category", "id=" + new DBNumber(this.id).SqlValue());
            return true;
        }
        public SortedList<string,string> GetErrorsMap()
        {
            return this.errorsmap;
        }
        public string GetErrorMessage(string name)
        {
            if (this.errorsmap.Keys.Contains(name))
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
    }
}