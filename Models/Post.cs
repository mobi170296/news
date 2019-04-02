using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewsApplication.Library.Database;
using MySql.Data.MySqlClient;
using System.Data;

namespace NewsApplication.Models
{
    public class Post
    {
        public int id { get; set; }
        public int journalist_id { get; set; }
        public int category_id { get; set; }
        public string content { get; set; }
        public string summary { get; set; }
        public string title { get; set; }
        public DateTime created_time { get; set; }
        public DateTime modified_time { get; set; }
        public int valid { get; set; }
        public int inspector_id { get; set; }
        public IDatabaseUtility connection;
        public SortedList<string, string> errorsmap;
        public Post()
        {
            this.id = -1;
            this.errorsmap = new SortedList<string, string>();
        }
        public Post(IDatabaseUtility connection) : this()
        {
            this.connection = connection;
        }
        public void SetConnection(IDatabaseUtility connection)
        {
            this.connection = connection;
        }
        public void AddErrorMessage(string name,string value)
        {
            this.errorsmap[name] = value;
        }
        public string GetErrorMessage(string name)
        {
            if (this.errorsmap.Keys.Contains(name))
            {
                return errorsmap[name];
            }
            else
            {
                return null;
            }
        }
        public SortedList<string,string> GetErrorsMap()
        {
            return this.errorsmap;
        }
        public SortedList<string,string> ErrorsMap
        {
            get
            {
                return this.errorsmap;
            }
        }
        public Post CheckValidForContent()
        {
            if(this.content == null || this.content.Length < 10)
            {
                this.errorsmap["content"] = "Nội dung của bài viết có chiều dài không hợp lý";
            }
            return this;
        }
        public Post CheckValidForTitle()
        {
            if(this.title == null || this.title.Length < 10)
            {
                this.errorsmap["title"] = "Tiêu đề bài viết có chiều dài không hợp lệ";
            }
            return this;

        }
        public Post CheckValidForSummary()
        {
            if(this.summary == null || this.summary.Length> 200)
            {
                this.errorsmap["summary"] = "Tóm tắt bài viết không hợp lệ!";
            }
            return this;
        }
        public Post CheckValidForCategoryId()
        {
            using(IDataReader result = this.connection.select("*").from("category").where("id=" + this.category_id).Execute())
            {
                if (!result.Read())
                {
                    errorsmap["category_id"] = "Danh mục này không tồn tại! ";
                }
            }
            return this;
        }
        public void Standardization()
        {
            if (this.content != null)
            {
                this.content = this.content.Replace(@"\", @"\\").Replace(@"'", @"\'");
            }
            if (this.title != null)
            {
                this.title = this.title.Replace(@"\", @"\\").Replace(@"'", @"\'");
            }
            if (this.summary != null)
            {
                this.summary = this.summary.Replace(@"\", @"\\").Replace(@"'", @"\'");
            }
        }
        public bool Load()
        {
            //load dua vao id
            using (MySqlDataReader result = (MySqlDataReader)this.connection.select("*").from("post").where("id=" + new DBNumber(this.id).SqlValue()).Execute())
            {
                if (result.Read())
                {
                    this.journalist_id = result.GetInt32("journalist_id");
                    this.category_id = result.GetInt32("category_id");
                    this.content = result.GetString("content");
                    this.title = result.GetString("title");
                    this.created_time = result.GetDateTime("created_time");
                    this.valid = result.GetInt32("valid");
                    try
                    {
                        this.inspector_id = result.GetInt32("inspector_id");
                    }catch
                    {
                        this.inspector_id = -1;
                    }
                    this.modified_time = result.GetDateTime("modified_time");
                    this.summary = result.GetString("summary");
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
            return this.connection.Insert("post", new SortedList<string, IDBDataType>()
            {
                {"journalist_id", new DBNumber(this.journalist_id) },
                {"category_id", new DBNumber(this.category_id) },
                {"content", new DBString(this.content) },
                {"title", new DBString(this.title) },
                {"summary", new DBString(this.summary) },
                {"valid", new DBNumber(this.valid) },
            }) != 0;
        }
        public bool Update(Post newdata)
        {
            this.Standardization();
            return this.connection.Update("post", new SortedList<string, IDBDataType>()
            {
                {"content", new DBString(newdata.content) },
                {"title", new DBString(newdata.title) },
                {"modified_time", new DBDateTime(DateTime.Now) },
                {"summary", new DBString(newdata.summary) }
            },"id=" + new DBNumber(this.id).SqlValue()) != 0;
        }
        public bool Delete()
        {
            return this.connection.Delete("post", "id=" + new DBNumber(this.id).SqlValue()) != 0;
        }
        public bool Show()
        {
            this.connection.Update("post", new SortedList<string, IDBDataType>()
            {
                {"valid", new DBNumber(1) },
                {"inspector_id", new DBNumber(this.inspector_id) }
            }, "id=" + new DBNumber(this.id).SqlValue());
            return true;
        }
        public bool Hide()
        {
            this.connection.Update("post", new SortedList<string, IDBDataType>()
            {
                {"valid", new DBNumber(0) }
            }, "id=" + new DBNumber(this.id).SqlValue());
            return true;
        }
        public bool IsShown()
        {
            return this.valid != 1;
        }
        public bool IsHidden()
        {
            return this.valid == 0;
        }
    }
}