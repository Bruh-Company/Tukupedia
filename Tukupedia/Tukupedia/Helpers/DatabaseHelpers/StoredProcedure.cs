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
    public class StoredProcedure
    {
        public string prochedure_name { get; set; }
        public OracleCommand cmd { get; set; }
        Dictionary<string, ParameterDirection> directions;
        //Contoh Cara Pakai
        //StoredProchedure autogen = new StoredProchedure("autogenNota");
        //autogen.addParam("I", "tgl",tgl,255,OracleDbType.Varchar2);
        //autogen.addParam("R", "ret",255, OracleDbType.Varchar2);
        //tbNomorNota.Text = autogen.getValue();

        public StoredProcedure(string prochedure_name)
        {
            this.prochedure_name = prochedure_name;
            directions = new Dictionary<string, ParameterDirection>();
            directions.Add("I", ParameterDirection.Input);
            directions.Add("IO", ParameterDirection.InputOutput);
            directions.Add("R", ParameterDirection.ReturnValue);
            directions.Add("O", ParameterDirection.Output);
            initProchedure();
        }

        public void initProchedure()
        {
            cmd = new OracleCommand()
            {
                Connection = App.connection,
                CommandText = prochedure_name,
                CommandType = CommandType.StoredProcedure
            };
        }
        public void addParam(string direction, string paramName, int size, OracleDbType type)
        {
            cmd.Parameters.Add(new OracleParameter()
            {
                Direction = directions[direction],
                ParameterName = paramName,
                OracleDbType = type,
                Size = size
            });
        }
        public void addParam(string direction, string paramName, string input, int size, OracleDbType type)
        {
            cmd.Parameters.Add(new OracleParameter()
            {
                Direction = directions[direction],
                ParameterName = paramName,
                OracleDbType = type,
                Size = size,
                Value = input,
            });

        }
        public string getValue(string ret = "ret")
        {
            App.openConnection(out _);
            cmd.ExecuteNonQuery();
            string value = cmd.Parameters[ret].Value.ToString();
            App.closeConnection(out _);
            
            return value;
        }
    }
}
