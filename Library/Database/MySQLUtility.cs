using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace NewsApplication.Library.Database
{
    public class MySQLUtility : IDatabaseUtility
    {
        private MySqlConnection connection;
        public MySQLUtility()
        {
            try
            {
                this.connection = new MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["default"].ConnectionString);
            }catch(ConfigurationException e)
            {

            }
        }
        public MySQLUtility(string cs)
        {
            this.connection = new MySqlConnection(cs);
        }
        public void Connection()
        {
            try
            {
                this.connection.Open();
            }catch(MySqlException e)
            {
                throw new DBException(e.Code, e.Message);
            }
        }

        public int Insert(string table, SortedList<string,IDBDataType> data)
        {
            string keystring = "";
            string datastring = "";
            for (int i = 0; i < data.Count() - 1; i++)
            {
                keystring += data.Keys[i] + ",";
                datastring += data.Values[i].sqlValue() + ",";
            }
            keystring += data.Keys[data.Count() - 1];
            datastring += data.Values[data.Count() - 1];

            string query = "INSERT INTO " + table +"(" + keystring + ") VALUES(" + keystring + ")";

            try
            {
                MySqlCommand command = this.connection.CreateCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = query;
                return command.ExecuteNonQuery();
            }catch(MySqlException e)
            {
                throw new DBException(e.Code, e.Message);
            }
        }

        public int Update(string table, SortedList<string,IDBDataType> data, string where)
        {
            int count = data.Count();
            string nvp = "";
            for(int i=0;i<count - 1; i++)
            {
                nvp += data.Keys[i] + "=" + data.Values[i].sqlValue() + ",";
            }
            nvp += data.Keys[count - 1] + "=" + data.Values[count - 1];

            string query = "UPDATE " + table + " SET " + nvp + " WHERE " + where;

            try
            {
                MySqlCommand command = this.connection.CreateCommand();
                command.CommandText = query;
                return command.ExecuteNonQuery();
            }catch(MySqlException e)
            {
                throw new DBException(e.Code, e.Message);
            }
        }

        public int Delete(string table, string where)
        {
            string query = "DELETE FROM " + table + " WHERE " + where;
            try
            {
                MySqlCommand command = this.connection.CreateCommand();
                command.CommandText = query;
                return command.ExecuteNonQuery();
            }catch(MySqlException e)
            {
                throw new DBException(e.Code, e.Message);
            }
        }

    }
}