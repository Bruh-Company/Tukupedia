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
        public string table;

        public DB()
        {
            statement = "";
        }
        public DB(string table)
        {
            table = sanitize(table);

            statement = "";
            this.table = table;
        }

        /*
         * Cara Kerja Insert
         * new DB("user").insert("id",1,"name","bruh").execute();
         */
        public DB insert(params object[] param)
        {
            resetStatement();
            string columns = "(";
            string values = "(";
            for (int i = 0; i < param.Length; i+=2)
            {
                param[i] = sanitize(param[i].ToString());
                param[i + 1] = sanitize(param[i + 1].ToString());
                string comma = (i == param.Length - 2) ? ")" : ",";
                columns += $" {param[i]} {comma} ";
                string petik = param[i + 1].ToString().Contains("TO_") ? "" : "'";
                values += $" {petik}{param[i+1]}{petik} {comma} ";
            }
            statement += $"INSERT INTO {table} {columns} VALUES {values} ";
            return this;
        }
        /*
         * Cara Kerja Insert Raw
         * new DB().insertRAW($"INSERT INTO CUSTOMER(ID,NAME) VALUES ('1','BRUH')").execute();
         */
        public DB insertRAW(string statement)
        {
            resetStatement();
            this.statement = sanitize(statement);
            return this;
        }
        /*
         * Cara Kerja Delete
         * new DB("user").delete().where("id","1").execute();
         * new DB("user").delete().where("id","1",">").execute();
         */
        public DB delete()
        {
            resetStatement();
            statement += $"DELETE {table} ";
            return this;
        }
        /*
         * Cara Kerja Update
         * new DB("user").update("name","boodie","alamat","UKP").execute();
         */
        public DB update(params object[] param)
        {
            resetStatement();
            string str = "";
            for (int i = 0; i < param.Length; i+=2)
            {
                param[i] = sanitize(param[i].ToString());
                param[i + 1] = sanitize(param[i + 1].ToString());

                string comma = (i == param.Length - 2) ? "" : ",";
                string petik = param[i + 1].ToString().Contains("TO_") ? "" : "'";
                str +=$" {param[i]} = {petik}{param[i+1]}{petik} {comma}";
            }
            statement += $"UPDATE {table} SET {str} ";

            return this;
        }
        
        /*
         * Digunakan jika where harus "AND"
         */
        public DB where(string column, string value, string Operator="=")
        {
            string where = statement.Contains("WHERE") ? " AND " : " WHERE ";
            statement += $" {where} {sanitize(column)} {sanitize(Operator)} '{sanitize(value)}' ";
            return this;
        }
        /*
         * Digunakan jika where harus "OR"
         */
        public DB orWhere(string column, string value, string Operator = "=")
        {
            string where = statement.Contains("WHERE") ? " OR " : " WHERE ";
            statement += $" {where} {sanitize(column)} {sanitize(Operator)} '{sanitize(value)}' ";
            return this;
        }
        /*
         * Cara Kerja Select
         * Note : untuk select gunakan (nama table).(column)
         * Karena tidak memakai alias
         * -Cara 1-
         * Mendapatkan sebuah data table yang rownya memiliki price yang
         * lebih dari 1000
         * new DB("user").select().where("price","1000",">").get();
         * 
         * -Cara 2-
         * Mendapatkan sebuah data table yang rownya memiliki price yang
         * lebih dari 1000. Dengan kolom yang ditentukan
         * new DB("user").select("Nama as nama", "ID as identity", "price as harga").where("price","1000",">").get();
         */
        public DB select(params object[] param)
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
                    param[i] = sanitize(param[i].ToString());

                    string temp = param[i].ToString();
                    string comma = (i == param.Length - 1) ? "" : ",";
                    statement += $" {temp} {comma} ";
                }
            }
            return this;
        }
        /*
         * Cara Kerja Join
         * 
         * new DB("user").select().join("barang","barang_id","=","ID").where("price","1000",">").get();
         * 
         */
        public DB join(string table, string id ,string Operator, string foreignKey)
        {
            statement += $" join {sanitize(table)} on {this.table}.{sanitize(id)} {opSanitize(Operator)} {sanitize(table)}.{sanitize(foreignKey)} ";
            return this;
        }
        public DB leftJoin(string table, string id, string Operator, string foreignKey)
        {
            statement += $" left join {sanitize(table)} on {this.table}.{sanitize(id)} {opSanitize(Operator)} {sanitize(table)}.{sanitize(foreignKey)} ";
            return this;
        }
        public DB rightJoin(string table, string id, string Operator, string foreignKey)
        {
            statement += $" right join {sanitize(table)} on {this.table}.{sanitize(id)} {opSanitize(Operator)} {sanitize(table)}.{sanitize(foreignKey)} ";
            return this;
        }
        /**
         * Note Jika ada having dan memakai alias, maka berilah alias nya juga
         * */
        public DB having(string column ,string Operator, string value)
        {
            statement += $" having {sanitize(column)} {opSanitize(Operator)} {value} ";
            return this;
        }
        public DB groupBy(string group)
        {
            statement += $" group by {sanitize(group)} ";
            return this;
        }
        public DB orderBy(string ordered)
        {
            statement += $" order by {sanitize(ordered)} ";
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
            //MessageBox.Show(statement);
            
            App.openConnection(out _);
            cmd.ExecuteNonQuery();
            App.closeConnection(out _);
        }
        public DataTable get()
        {
            DataTable table = new DataTable();
            try
            {
                App.openConnection(out _);
                OracleDataAdapter adapter = new OracleDataAdapter(statement, App.connection);
                App.closeConnection(out _);
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
                //MessageBox.Show(get().Rows.Count.ToString());
                row = get().Rows[0];
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return row;

        }
        
        private static string sanitize(string str)
        {
            return str.Replace("'", "").Replace("\"", "").Replace("`", "");
        }

        private static string opSanitize(string op)
        {
            string[] known = { "<", ">", "<=", ">=", "=", "<>", "!=" };
            if (known.Contains(op)) return op;
            return "";
        }
        public string to_date(string date, string pattern)
        {
            return $"TO_DATE('{date}','{pattern}')";
        }
        public string to_char(string value, string pattern)
        {
            return $"TO_DATE('{value}','{pattern}')";
        }


    }
}
