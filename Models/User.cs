using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsApplication.Models
{
    public class User
    {
        public int id { get; set; }
        public int role { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public int phone { get; set; }
    }
}