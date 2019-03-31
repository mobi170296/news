using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsApplication.Library.Database
{
    public class DBException : System.Exception
    {
        private uint code;
        private string message;
        public DBException(uint code, string message)
        {
            this.code = code;
            this.message = message;
        }
        public uint Code
        {
            get
            {
                return this.code;
            }
            set
            {
                this.code = value;
            }
        }
        public string Message
        {
            get
            {
                return this.message;
            }
            set
            {
                this.message = value;
            }
        }
    }
}