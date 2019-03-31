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
        public object Value()
        {
            return this.data;
        }
        public string SqlValue()
        {
            return this.data.ToString();
        }
    }
}