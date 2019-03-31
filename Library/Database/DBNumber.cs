using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsApplication.Library.Database
{
    public class DBNumber : IDBDataType
    {
        private object data;
        public DBNumber(object data)
        {
            this.data = data;
        }
        public object value()
        {
            return this.data;
        }
        public string sqlValue()
        {
            return this.data.ToString();
        }
    }
}