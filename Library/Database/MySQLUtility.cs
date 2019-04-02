using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace NewsApplication.Library.Database
{
    public class MySQLUtility : IDatabaseUtility
    {
        public MySqlConnection connection;
        public string cquery = "";
        private string _select, _from, _join, _on, _where, _groupby, _having, _orderby, _order= "ASC", _limit;
        public long insert_id;
        public IDatabaseUtility select(string s)
        {
            this._select = s;
            //select again
            _from = _join = _on = _where = _groupby = _having = _orderby = _order = _limit = null;
            return this;
        }
        public IDatabaseUtility from(string s)
        {
            this._from = s;
            return this;
        }
        public IDatabaseUtility join(string s)
        {
            this._join = s;
            return this;
        }
        public IDatabaseUtility on(string s)
        {
            this._on = s;
            return this;
        }
        public IDatabaseUtility where(string s)
        {
            this._where = s;
            return this;
        }
        public IDatabaseUtility groupby(string s)
        {
            this._groupby = s;
            return this;
        }
        public IDatabaseUtility having(string s)
        {
            this._having = s;
            return this;
        }
        public IDatabaseUtility orderby(string s)
        {
            this._orderby = s;
            return this;
        }
        public IDatabaseUtility limit(int s, int t)
        {
            this._limit = s + "," + t;
            return this;
        }
        public IDatabaseUtility asc()
        {
            this._order = "ASC";
            return this;
        }
        public IDatabaseUtility desc()
        {
            this._order = "DESC";
            return this;
        }
        public long GetLastInsertedId()
        {
            return this.insert_id;
        }
        public IDataReader Execute()
        {
            try
            {
                string query = "SELECT " + this._select + " FROM " + this._from +
                (this._join != null ? " JOIN " + this._join : "") +
                (this._join != null && this._on != null ? " ON " + this._on : "") +
                (this._where != null ? " WHERE " + this._where : "") +
                (this._groupby != null ? " GROUP BY " + this._groupby : "") +
                (this._having != null ? " HAVING " + this._having : "") +
                (this._orderby != null ? " ORDER BY " + this._orderby + " " + this._order +  " " : "") + 
                (this._limit != null ? " LIMIT " + this._limit : "");
                MySqlCommand command = this.connection.CreateCommand();
                command.CommandText = query;
                this.cquery = query;
                return command.ExecuteReader();
            } catch (MySqlException e) {
                throw new DBException(e.Code, e.Message);
            }
        }
        public MySQLUtility()
        {
            try
            {
                this.connection = new MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["default"].ConnectionString);
            }catch(ConfigurationException e)
            {
                throw new DBException(1, e.Message);
            }
        }
        public MySQLUtility(string cs)
        {
            this.connection = new MySqlConnection(cs);
        }
        public void Connect()
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
                datastring += data.Values[i].SqlValue() + ",";
            }
            keystring += data.Keys[data.Count() - 1];
            datastring += data.Values[data.Count() - 1].SqlValue();

            string query = "INSERT INTO " + table +"(" + keystring + ") VALUES(" + datastring + ")";

            try
            {
                MySqlCommand command = this.connection.CreateCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = query;
                this.cquery = query;
                int c = command.ExecuteNonQuery();
                this.insert_id = command.LastInsertedId;
                return c;
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
                nvp += data.Keys[i] + "=" + data.Values[i].SqlValue() + ",";
            }
            nvp += data.Keys[count - 1] + "=" + data.Values[count - 1].SqlValue();

            string query = "UPDATE " + table + " SET " + nvp + " WHERE " + where;

            try
            {
                MySqlCommand command = this.connection.CreateCommand();
                command.CommandText = query;
                this.cquery = query;
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
                this.cquery = query;
                return command.ExecuteNonQuery();
            }catch(MySqlException e)
            {
                throw new DBException(e.Code, e.Message);
            }
        }

        public IDataReader Query(string query)
        {
            try
            {
                MySqlCommand command = this.connection.CreateCommand();
                command.CommandText = query;
                this.cquery = query;
                return command.ExecuteReader();
            }catch(MySqlException e)
            {
                throw new DBException(e.Code, e.Message);
            }
        }

    }
}