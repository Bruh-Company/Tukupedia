using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tukupedia.Models
{
    public class SellerModel : Model {
        public enum page { Pesanan, Produk, InfoToko }

        public SellerModel()
        {
            TableName = "SELLER";
            init();
        }

        public void loadData(page p, int idSeller) {
            if (p == page.Pesanan) {
                Table = new DataTable();
                statement = $"SELECT * FROM ''";
                Adapter = new OracleDataAdapter(statement, App.connection);
                Builder = new OracleCommandBuilder(Adapter);
                Adapter.Fill(Table);
            }
            if (p == page.Produk) {
                Table = new DataTable();
                statement = $"SELECT * FROM ITEM WHERE ID_SELLER = '{idSeller}'";
                Adapter = new OracleDataAdapter(statement, App.connection);
                Builder = new OracleCommandBuilder(Adapter);
                Adapter.Fill(Table);
            }
        }

    }
}
