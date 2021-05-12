using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Tukupedia.Models
{
    public class Model
    {
        public DataTable Table { get; set; }
        public string TableName { get; set; }
        public OracleDataAdapter Adapter { get; set; }
        public OracleCommandBuilder Builder { get; set; }
        public string statement { get; set; }
        public string where { get; set; }

        public Model()
        {
            TableName = "";
            Table = new DataTable();
        }
        public void init()
        {
            Table = new DataTable();
            statement = $"SELECT * FROM {TableName}";
            Adapter = new OracleDataAdapter(statement, App.connection);
            Builder = new OracleCommandBuilder(Adapter);
            Adapter.Fill(Table);
        }
        public void initAdapter(string statement)
        {
            try
            {
                Table = new DataTable();
                this.statement = statement;
                Adapter = new OracleDataAdapter(statement, App.connection);
                Builder = new OracleCommandBuilder(Adapter);
                Adapter.Fill(Table);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public void update()
        {
            Builder = new OracleCommandBuilder(Adapter);
            Adapter.Update(Table);
        }
        public void deleteRowAt(int idx)
        {
            DataRow dr = Table.Rows[idx];
            dr.Delete();
            update();
        }
        public void delete(DataRow row)
        {
            Table.Rows.Remove(row);
            update();
        }
        //Cara Pakai.. .insert("ID",1,"Column 2","Hello World")
        public void insert(params object[] param)
        {
            DataRow row = Table.NewRow();

            for (int i = 0; i < param.Length/2; i++)
            {
                var col = param[i].ToString();
                var val = param[i + 1].ToString();
                row[col] = val;
            }
            Table.Rows.Add(row);
            update();
        }
        public void insert(DataRow row)
        {
            Table.Rows.Add(row);
            update();
        }
        public void updateRow(DataRow row,params object[] param)
        {
            for (int i = 0; i < param.Length/2; i++)
            {
                var col = param[i].ToString();
                var val = param[i + 1].ToString();
                row[col] = val;
            }
            update();
        }

        public void resetWhere()
        {
            where = "";
        }
        public void addWhere(string column, string val,string opera="=", bool apostrophe=true)
        {
            string and = where == "" ? "" : "AND";
            string apos = apostrophe ? "'" : "";
            where += $" {and} {column} {opera} {apos}{val}{apos} ";
        }
        public void addOrWhere(string column, string val,string opera="=", bool apostrophe=true)
        {
            string or = where == "" ? "" : "OR";
            string apos = apostrophe ? "'" : "";
            where += $" {or} {column} {opera} {apos}{val}{apos} ";
        }

        public DataRow[] get()
        {
            return Table.Select(statement);
        }
        
    }
}
