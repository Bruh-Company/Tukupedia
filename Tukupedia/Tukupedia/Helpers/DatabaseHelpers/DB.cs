using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Tukupedia.Helpers.DatabaseHelpers
{
    public class DB
    {
        public string statement { get; set; }

        public void resetStatement() { statement = ""; }
        /*
         * Cara Kerja Insert
         * new DB().insert("user",[],[])
         */
        public DB insert(string table, params object[] param)
        {
            resetStatement();
            string columns = "(";
            string values = "(";
            for (int i = 0; i < param.Length; i++)
            {
                var item = param[i] as string[];
                string comma = (i == param.Length - 1) ? ")" : ",";
                columns += $" '{item[0]}' {comma}";
                values += $" '{item[1]}' {comma}";
            }
            statement += $"INSERT INTO {table} ({columns} VALUES {values})";
            return this;
        }
        public DB insertRAW(string statement)
        {
            resetStatement();
            this.statement = statement;
            return this;
        }
        public DB delete(string table)
        {
            resetStatement();
            statement += $"DELETE {table} ";
            return this;
        }
        public DB update(string table, params object[] param)
        {
            resetStatement();
            string str = "";
            for (int i = 0; i < param.Length; i++)
            {
                var item = param[i] as string[];
                string comma = (i == param.Length - 1) ? "" : ",";
                str +=$" {item[0]} = '{item[1]}' {comma}";
            }
            statement += $"UPDATE {table} SET {str} ";

            return this;
        }
        public DB where(string column, string value)
        {
            string where = statement.Contains("WHERE") ? " AND " : " WHERE ";
            statement += $" {where} {column} = '{value}' ";
            return this;
        }
        public DB orWhere(string column, string value)
        {
            string where = statement.Contains("WHERE") ? " OR " : " WHERE ";
            statement += $" {where} {column} = '{value}' ";
            return this;
        }
        public DB select(string table, params object[] param)
        {
            resetStatement();
            if (param.Length == 0)
            {
                statement += $"SELECT * FROM {table} ";
            }
            else
            {
                statement += $"SELECT * FROM {table} ";
                for (int i = 0; i < param.Length; i++)
                {
                    string temp = param[i].ToString();
                    string comma = (i == param.Length - 1) ? "" : ",";
                    statement += $" {temp} {comma} ";
                }
            }

            return this;
        }
        public DB join(string table, string id ,string Operator, string foreignKey)
        {
            statement += $" join {table} on {id} {Operator} {foreignKey} ";
            return this;
        }
        public DB leftJoin(string table, string id, string Operator, string foreignKey)
        {
            statement += $" left join {table} on {id} {Operator} {foreignKey} ";
            return this;
        }
        public DB rightJoin(string table, string id, string Operator, string foreignKey)
        {
            statement += $" right join {table} on {id} {Operator} {foreignKey} ";
            return this;
        }
        public DB having(string column ,string Operator, string value)
        {
            statement += $" having {column} {Operator} {value} ";
            return this;
        }
        public DB groupBy(string group)
        {
            statement += $" group by {group} ";
            return this;
        }
        public DB orderBy(string ordered)
        {
            statement += $" order by {ordered} ";
            return this;
        }
        public int count()
        {
            return get().Rows.Count;
        }
        public void execute()
        {
            OracleCommand cmd = new OracleCommand()
            {
                CommandText = statement,
                Connection = App.connection
            };
            App.openConnection();
            cmd.ExecuteNonQuery();
            App.closeConnection();
        }
        public DataTable get()
        {
            DataTable table = new DataTable();
            try
            {
                OracleDataAdapter adapter = new OracleDataAdapter(statement, App.connection);
                adapter.Fill(table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return table;
        }
        public DataRow getFirst()
        {
            DataRow row = null;
            try
            {
                row = get().Rows[0];
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return row;

        }
        
    }
}
