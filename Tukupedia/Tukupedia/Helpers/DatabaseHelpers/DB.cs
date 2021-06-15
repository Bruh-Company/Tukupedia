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
                if (param[i + 1] is DateTime)
                {
                    param[i] = sanitize(param[i].ToString());
                    param[i + 1] = $" TO_DATE('{(DateTime)param[i + 1]:ddMMyyyy}', 'ddmmyyyy') ";

                    string comma = (i == param.Length - 2) ? ")" : ",";
                    columns += $" {param[i]} {comma} ";
                    string petik = param[i + 1].ToString().Contains("TO_") ? "" : "'";
                    values += $" {petik}{param[i + 1]}{petik} {comma} ";
                }
                else
                {
                    param[i] = sanitize(param[i].ToString());
                    param[i + 1] = sanitize(param[i + 1].ToString());
                    string comma = (i == param.Length - 2) ? ")" : ",";
                    columns += $" {param[i]} {comma} ";
                    string petik = param[i + 1].ToString().Contains("TO_") ? "" : "'";
                    values += $" {petik}{param[i + 1]}{petik} {comma} ";
                }

            }
            statement += $"INSERT INTO {table} {columns} VALUES {values} ";
            return this;
        }
        /*
         * Cara Kerja Insert Raw
         * new DB().insertRAW($"INSERT INTO CUSTOMER(ID,NAME) VALUES ('1','BRUH')").execute();
         * !! unsanitize, jangan dipake klo nerima input dari user
         */
        public DB insertRAW(string statement)
        {
            resetStatement();
            this.statement = statement;
            return this;
        }
        /*
         * Cara Kerja Delete
         * new DB("user").delete().where("id","1").execute();
         * new DB("user").delete().where("id","1",">").execute();
         */
        public DB delete(string id)
        {
            resetStatement();
            statement += $"DELETE {table} WHERE {table}.ID = '{sanitize(id)}'";
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
                string comma = (i == param.Length - 2) ? "" : ",";

                if (param[i + 1] is DateTime)
                {

                    param[i] = sanitize(param[i].ToString());
                    DateTime dt = (DateTime)param[i + 1];
                    param[i + 1] = $"TO_DATE('{dt:dd-MM-yyyy}','dd-mm-yyyy')";

                    string petik = param[i + 1].ToString().Contains("TO_") ? "" : "'";
                    str += $" {param[i]} = {petik}{param[i + 1]}{petik} {comma}";
                }
                else if (param[i + 1] is int)
                {

                    param[i] = sanitize(param[i].ToString());

                    str += $" {param[i]} = {param[i + 1]} {comma}";
                }
                else
                {
                    param[i] = sanitize(param[i].ToString());
                    param[i + 1] = sanitize(param[i + 1].ToString());

                    string petik = param[i + 1].ToString().Contains("TO_") ? "" : "'";
                    str += $" {param[i]} = {petik}{param[i + 1]}{petik} {comma}";
                }
            }
            statement += $"UPDATE {table} SET {str} ";
            //MessageBox.Show(statement);

            return this;
        }
        
        /*
         * Digunakan jika where harus "AND"
         */
        public DB where(string column, string value, string Operator="=")
        {
            string where = statement.Contains("WHERE") ? " AND " : " WHERE ";
            statement += $" {where} {sanitize(column)} {opSanitize(Operator)} '{sanitize(value)}' ";
            return this;
        }

        public DB where(string column, DateTime value, string Operator = "=", string dateFormatSQL = "dd-mm-yyyy", string dateFormatCS = "dd-MM-yyyy")
        {
            string where = statement.Contains("WHERE") ? " AND " : " WHERE ";
            statement += $" {where} to_date('{sanitize(column)}', '{dateFormatSQL}') {opSanitize(Operator)} to_date('{value.ToString(dateFormatCS)}', '{dateFormatSQL}') ";
            return this;
        }

        /*
         * Digunakan jika where harus "OR"
         */
        public DB orWhere(string column, string value, string Operator = "=")
        {
            string where = statement.Contains("WHERE") ? " OR " : " WHERE ";
            statement += $" {where} {sanitize(column)} {opSanitize(Operator)} '{sanitize(value)}' ";
            return this;
        }

        public DB orWhere(string column, DateTime value, string Operator = "=", string dateFormatSQL = "dd-mm-yyyy", string dateFormatCS = "dd-MM-yyyy")
        {
            string where = statement.Contains("WHERE") ? " OR " : " WHERE ";
            statement += $" {where} to_date('{sanitize(column)}', '{dateFormatSQL}') {opSanitize(Operator)} to_date('{value.ToString(dateFormatCS)}', '{dateFormatSQL}') ";
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
                statement += $"SELECT ";
                for (int i = 0; i < param.Length; i++)
                {
                    param[i] = param[i].ToString().Contains("TO_") ? param[i].ToString() : sanitize(param[i].ToString());
                    string temp = param[i].ToString();
                    string comma = (i == param.Length - 1) ? "" : ",";
                    statement += $" {temp} {comma} ";
                }
                statement += $" FROM {table} ";
            }
            return this;
        }
        /*
         * Cara Kerja Join
         *
         * Catatan :                     Table Join  Join dgn siapa   Reference
         * new DB("barang").select().join("category","barang","id","=","ID").where("price","1000",">").get();
         * 
         */
        public DB join(string table,string joinTable, string id ,string Operator, string reference)
        {
            statement += $" join {sanitize(table)} on {sanitize(joinTable)}.{sanitize(id)} {opSanitize(Operator)} {sanitize(table)}.{sanitize(reference)} ";
            return this;
        }
        public DB leftJoin(string table,string joinTable, string id, string Operator, string reference)
        {
            statement += $" left join {sanitize(table)} on {sanitize(joinTable)}.{sanitize(id)} {opSanitize(Operator)} {sanitize(table)}.{sanitize(reference)} ";
            return this;
        }
        public DB rightJoin(string table,string joinTable, string id, string Operator, string reference)
        {
            statement += $" right join {sanitize(table)} on {sanitize(joinTable)}.{sanitize(id)} {opSanitize(Operator)} {sanitize(table)}.{sanitize(reference)} ";
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
            try
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
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public DataTable get( bool debug =false)
        {
            DataTable table = new DataTable();
            try
            {
                if (debug) Console.WriteLine(statement);
                App.openConnection(out _);
                OracleDataAdapter adapter = new OracleDataAdapter(statement, App.connection);
                App.closeConnection(out _);
                adapter.Fill(table);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return table;
        }
        public DataRow getFirst(bool debug = false)
        {
            DataRow row = null;
            try
            {
                //MessageBox.Show(get().Rows.Count.ToString());
                row = get(debug).Rows[0];
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
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
        public static string to_date(string date, string pattern)
        {
            return $"TO_DATE('{sanitize(date)}','{pattern}')";
        }
        public static string to_char(string value, string pattern)
        {
            return $"TO_DATE('{sanitize(value)}','{pattern}')";
        }
    }
}
