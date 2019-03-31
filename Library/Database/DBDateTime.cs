using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsApplication.Library.Database
{
    public class DBDateTime : IDBDataType
    {
        private DateTime data;
        public DBDateTime(DateTime data)
        {
            this.data = data;
        }
        public DBDateTime(int year, int month, int day, int hour, int minute, int second)
        {
            this.data = new DateTime(year, month, day, hour, minute, second);
        }
        public object Value()
        {
            return this.data;
        }
        public string SqlValue()
        {
            return "'" + this.data.Year + "-" + this.data.Month + "-" + this.data.Day + " " + this.data.Hour + ":" + this.data.Minute + ":" + this.data.Second + "'";
        }
    }
}