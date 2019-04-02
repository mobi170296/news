using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewsApplication.Library.Database;
using System.Data;

namespace NewsApplication.Models
{
    public class PostListModel
    {
        public List<Post> list;
        IDatabaseUtility connection;
        public PostListModel()
        {
            this.list = new List<Post>();
        }
        public PostListModel(IDatabaseUtility connection) : this()
        {
            this.connection = connection;
        }
        public List<Post> GetAll()
        {
            list.Clear();
            List<int> ids = new List<int>();
            using (IDataReader result = this.connection.select("*").from("post").Execute())
            {
                ids.Add((int)result["id"]);
            }

            foreach (int id in ids)
            {
                Post post = new Post(this.connection);
                post.id = id;
                post.Load();
                this.list.Add(post);
            }
            return this.list;
        }

        public List<Post> GetLimit(int s, int t)
        {
            list.Clear();
            List<int> ids = new List<int>();
            using (IDataReader result = this.connection.select("*").from("post").limit(s, t).Execute())
            {
                while (result.Read())
                {
                    ids.Add((int)result["id"]);
                }
            }


            foreach (int id in ids)
            {
                Post post = new Post(this.connection);
                post.id = id;
                post.Load();
                this.list.Add(post);
            }
            return this.list;
        }
        public int GetTotal()
        {
            using (IDataReader result = this.connection.select("count(*)").from("post").Execute())
            {
                result.Read();
                return result.GetInt32(0);
            }
        }
    }
}