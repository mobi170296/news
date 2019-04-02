using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewsApplication.Models;
using NewsApplication.Library.Database;

namespace NewsApplication.Models
{
	public class PostImage
	{
        public const string POSTER_IMAGE_DIR = "~/upload/posters/";
        public int id { get; set; }
        public int post_id { get; set; }
        public string path { get; set; }
        public DateTime created { get; set; }
        public IDatabaseUtility connection;
        public PostImage()
        {

        }
        public PostImage(IDatabaseUtility connection) : this()
        {
            this.connection = connection;
        }

        public void Load()
        {

        }

        public bool Add()
        {
            this.connection.Insert("postimage", new SortedList<string, IDBDataType>()
            {
                {"post_id", new DBNumber(this.post_id)},
                {"path", new DBString(this.path)}
            });
            return true;
        }
	}
}