using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Data;

namespace NewsApplication.Library.Database
{
    public interface IDatabaseUtility
    {
        void Connect();
        IDatabaseUtility select(string s);
        IDatabaseUtility from(string s);
        IDatabaseUtility join(string s);
        IDatabaseUtility on(string s);
        IDatabaseUtility where(string s);
        IDatabaseUtility groupby(string s);
        IDatabaseUtility having(string s);
        IDatabaseUtility orderby(string s);
        IDatabaseUtility asc();
        IDatabaseUtility desc();
        IDatabaseUtility limit(int f, int t);
        IDataReader Execute();
        int Insert(string table, SortedList<string,IDBDataType> data);
        int Update(string table, SortedList<string,IDBDataType> nvp, string where);
        int Delete(string table, string where);
        IDataReader Query(string sql);
        long GetLastInsertedId();
    }
}