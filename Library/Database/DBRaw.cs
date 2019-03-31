using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsApplication.Library.Database
{
    public class DBRaw : IDBDataType
    {
        public string data;
        public DBRaw(string data)
        {
            this.data = data;
        }
        public object value()
        {
            return data;
        }
        public string sqlValue()
        {
            return this.data;
        }
    }
}