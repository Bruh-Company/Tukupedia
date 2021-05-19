using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tukupedia.Models;

namespace Tukupedia.ViewModels.Admin
{
    class TransactionViewModel
    {
        H_Trans_ItemModel hm;
        D_Trans_ItemModel dm;
        int selected = -1;
        public TransactionViewModel()
        {
            hm = new H_Trans_ItemModel();
            reloadHtrans();
        }

        void reloadHtrans()
        {
            hm.initAdapter($"select h.KODE as \"Kode\", c.NAMA as \"Nama Customer\", h.TANGGAL_TRANSAKSI as \"Tanggal Transaksi\",case h.STATUS when 'W' then 'Waiting Payment' when 'C' then 'Canceled' when 'P' then 'Paid' end as \"Status\" from H_TRANS_ITEM h, CUSTOMER c where h.ID_CUSTOMER = c.ID");
        }

        public DataTable getHtrans()
        {
            //MessageBox.Show(cm.statement);
            return hm.Table;
        }
        public DataRow selectData(int pos)
        {
            try
            {
                selected = pos;
                return hm.Table.Rows[pos];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable getDTrans()
        {
            DataRow dr = hm.Table.Rows[selected];
            dm = new D_Trans_ItemModel();
            dm.initAdapter($"select i.NAMA as \"Nama Item\", s.NAMA_SELLER as \"Nama Seller\", i.HARGA as \"Harga\", d.JUMLAH as \"Jumlah\", to_number(d.JUMLAH) * to_number(i.HARGA) as \"Total\" from D_TRANS_ITEM d, ITEM i, SELLER s where d.ID_ITEM = i.ID and s.ID = i.ID_SELLER and d.ID_H_TRANS_ITEM = '{dr[0].ToString()}'");
            return dm.Table;
        }
        public void update(string nama, string email, string alamat, string notelp, DateTime lahir, int official)
        {
            //DataRow dr = sm.Table.Rows[selected];
            //new DB("seller").update("TANGGAL_LAHIR", $"TO_DATE('{lahir.ToString("dd-MM-yyyy")}','dd-mm-yyyy')").where("KODE", dr[0].ToString()).execute();
            //new DB("seller").update("IS_OFFICIAL", $"{official}").where("KODE", dr[0].ToString()).execute();
            //dr[1] = email;
            //dr[2] = nama;
            //dr[3] = alamat;
            //dr[4] = notelp;
            ////dr[5] = lahir.ToString("dd-MM-yyyy");
            //sm.update();
        }
        public void ban()
        {
            //DataRow dr = sm.Table.Rows[selected];
            //if (dr["Status"].ToString() == "Aktif")
            //{
            //    new DB("seller").update("STATUS", "0").where("KODE", dr[0].ToString()).execute();
            //}
            //else
            //{
            //    new DB("seller").update("STATUS", "1").where("KODE", dr[0].ToString()).execute();
            //}
        }
    }
}
