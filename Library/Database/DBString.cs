﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsApplication.Library.Database
{
    public class DBString : IDBDataType
    {
        private string data;
        public DBString(string data)
        {
            this.data = data;
        }
        public string sqlValue()
        {
            return "'" + this.data + "'";
        }
        public object value()
        {
            return data;
        }
    }
}