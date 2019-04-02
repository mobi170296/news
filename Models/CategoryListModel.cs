using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewsApplication.Library.Database;
using System.Data;

namespace NewsApplication.Models
{
    public class CategoryListModel
    {
        public List<Category> list;
        public IDatabaseUtility connection;
        public CategoryListModel()
        {
            this.list = new List<Category>();
        }
        public CategoryListModel(IDatabaseUtility connection) : this()
        {
            this.connection = connection;
        }
        public List<Category> GetAll()
        {
            this.list.Clear();
            List<int> ids = new List<int>();
            using(IDataReader result = this.connection.select("*").from("category").Execute())
            {
                while (result.Read())
                {
                    ids.Add((int)result["id"]);
                }
            }

            foreach(int id in ids)
            {
                Category cate = new Category(this.connection);
                cate.id = id;
                cate.Load();
                this.list.Add(cate);
            }
            return this.list;
        }
        public List<Category> GetLimit(int from, int total)
        {
            this.list.Clear();
            List<int> ids = new List<int>();
            using (IDataReader result = this.connection.select("*").from("post").limit(from, total).Execute())
            {
                while (result.Read())
                {
                    ids.Add((int)result["id"]);
                }
            }

            foreach (int id in ids)
            {
                Category cate = new Category(this.connection);
                cate.id = id;
                cate.Load();
                this.list.Add(cate);
            }

            return this.list;
        }
    }
}