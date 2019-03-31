using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsApplication.Exception
{
    public class InputException : System.Exception
    {
        private int code;
        private string[] errors;
        public InputException(int code, string[] error)
        {
            this.code = code;
            this.errors = error;
        }
        public int Code {
            get {
                return this.code;
            }
            set
            {
                this.code = value;
            }
        }
        public string[] Errors
        {
            get
            {
                return this.errors;
            }
            set
            {
                this.errors = value;
            }
        }
    }
}