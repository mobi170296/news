using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsApplication.Exception
{
    public class InputException : System.Exception
    {
        private int code;
        private SortedList<string,string> errors;
        public InputException(int code, SortedList<string,string> error, string message = "") : base(message)
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
        public SortedList<string,string> Errors
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