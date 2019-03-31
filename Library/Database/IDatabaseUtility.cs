using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace NewsApplication.Library.Database
{
    public interface IDatabaseUtility
    {
        void Connect();
        int Insert(string table, SortedList<string,IDBDataType> data);
        int Update(string table, SortedList<string,IDBDataType> nvp, string where);
        int Delete(string table, string where);
        MySqlDataReader Query(string sql);
    }
}