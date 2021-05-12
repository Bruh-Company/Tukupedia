using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tukupedia.Models
{
    public class ItemModel : Model
    {
        public DataTable datagridTable;

        public ItemModel()
        {
            TableName = "ITEM";
            init();
        }

        public void loadDataProduk(int idSeller) {
            datagridTable = new DataTable();
            statement = $"SELECT " +
                $"i.ID as \"ID\", " +
                $"i.KODE as \"KODE BARANG\", " +
                $"i.NAMA as \"NAMA BARANG\", " +
                $"c.NAMA as \"KATEGORI\", " +
                $"i.HARGA as \"HARGA\", " +
                $"i.STATUS as \"STATUS\" " +
                $"FROM ITEM i, CATEGORY c " +
                $"WHERE i.ID_SELLER = '{idSeller}' " +
                $"and i.ID_CATEGORY = c.ID";
            Adapter = new OracleDataAdapter(statement, App.connection);
            Builder = new OracleCommandBuilder(Adapter);
            Adapter.Fill(datagridTable);
        }

    }
}
