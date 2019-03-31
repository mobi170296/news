using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsApplication.Library.Database
{
    public interface IDatabaseUtility
    {
        bool Insert(string table, SortedList<string,string> data);
        bool Update(string table, SortedList<string, string> nvp, string where);
        bool Delete(string table, string where);
        bool Query(string sql);
    }
}