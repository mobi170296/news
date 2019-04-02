using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsApplication.Library.Database
{
    public class DBException : System.Exception
    {
        private uint code;
        public DBException(uint code, string message) : base(message)
        {
            this.code = code;
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
    }
}