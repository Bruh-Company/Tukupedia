﻿using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tukupedia.Models
{
    public class Model
    {
        public DataTable Table { get; set; }
        public string TableName { get; set; }
        public OracleDataAdapter Adapter { get; set; }
        public OracleCommandBuilder Builder { get; set; }
        public string statement { get; set; }

        public Model()
        {
            
        }
        public void init()
        {
            statement = $"SELECT * FROM {TableName}";
            Adapter = new OracleDataAdapter(statement, App.connection);
            Builder = new OracleCommandBuilder(Adapter);
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
        public void insert(params object[] param)
        {
            DataRow row = Table.NewRow();

            for (int i = 0; i < Table.Columns.Count; i++)
            {
                var item = param[i] as string[];
                row[item[0]] = item[1];
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
            for (int i = 0; i < Table.Columns.Count; i++)
            {
                var item = param[i] as string[];
                row[item[0]] = item[1];
            }
            update();
        }
    }
}
