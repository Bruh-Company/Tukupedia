using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tukupedia.Helpers.DatabaseHelpers;
using Tukupedia.Models;

namespace Tukupedia.ViewModels.Admin
{
    class JenisPromoViewModel
    {
        Jenis_PromoModel cm;
        Jenis_PromoModel forid;
        int selected = -1;
        Jenis_PromoModel category;
        Jenis_PromoModel kurir;
        Jenis_PromoModel seller;
        Jenis_PromoModel metode_pembayaran;
        public JenisPromoViewModel()
        {
            cm = new Jenis_PromoModel();
            forid = new Jenis_PromoModel();
            category = new Jenis_PromoModel();
            kurir = new Jenis_PromoModel();
            seller = new Jenis_PromoModel();
            metode_pembayaran = new Jenis_PromoModel();
            reload();
        }

        void reload()
        {
            cm.initAdapter($"select jp.NAMA as \"Jenis Promo\", nvl(c.NAMA,'-') as \"Kategori\", nvl(k.NAMA,'-') as \"Kurir\", nvl(s.NAMA_TOKO,'-') as \"Seller\", nvl(m.NAMA,'-') as \"Metode Pembayaran\", case jp.STATUS when '1' then 'Aktif' else 'Non Aktif' end as \"Status\" from JENIS_PROMO jp left join CATEGORY c on jp.ID_CATEGORY = c.ID left join KURIR k on k.ID = jp.ID_KURIR left join SELLER s on s.ID = jp.ID_SELLER left join METODE_PEMBAYARAN m on jp.ID_METODE_PEMBAYARAN = m.ID order by jp.nama");
            forid.initAdapter("select jp.ID from JENIS_PROMO jp left join CATEGORY c on jp.ID_CATEGORY = c.ID left join KURIR k on k.ID = jp.ID_KURIR left join SELLER s on s.ID = jp.ID_SELLER left join METODE_PEMBAYARAN m on jp.ID_METODE_PEMBAYARAN = m.ID order by jp.nama");
            category.initAdapter("select ID, NAMA from CATEGORY where STATUS = '1'");
            kurir.initAdapter("select ID, NAMA from KURIR where STATUS = '1'");
            seller.initAdapter("select ID, NAMA_TOKO from SELLER where STATUS = '1'");
            metode_pembayaran.initAdapter("select ID, NAMA from METODE_PEMBAYARAN where STATUS = '1'");
        }

        public DataTable getCategory()
        {
            return category.Table;
        }
        public DataTable getCourier()
        {
            return kurir.Table;
        }
        public DataTable getSeller()
        {
            return seller.Table;
        }
        public DataTable getMetode_Pembayaran()
        {
            return metode_pembayaran.Table;
        }

        public DataTable getDataTable()
        {
            //MessageBox.Show(cm.statement);
            return cm.Table;
        }
        public DataRow selectData(int pos)
        {
            try
            {
                selected = pos;
                return cm.Table.Rows[pos];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool insert(string nama, string id_category, string id_kurir, string id_seller, string id_metode_pembayaran)
        {
            if(nama == "" || id_category == "")
            {
                MessageBox.Show("Nama atau Kategori tidak boleh kosong");
                return false;
            }
            try
            {
                new DB("JENIS_PROMO").insert(
                "NAMA", nama,
                "ID_CATEGORY", id_category,
                "ID_KURIR", id_kurir,
                "ID_SELLER", id_seller,
                "ID_METODE_PEMBAYARAN", id_metode_pembayaran).execute();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            return true;


        }
        public void update(string nama, string id_category, string id_kurir, string id_seller, string id_metode_pembayaran)
        {
            try
            {
                DataRow dr = forid.Table.Rows[selected];
                new DB("JENIS_PROMO").update(
                "NAMA", nama,
                "ID_CATEGORY", id_category,
                "ID_KURIR", id_kurir,
                "ID_SELLER", id_seller,
                "ID_METODE_PEMBAYARAN", id_metode_pembayaran).where("ID",dr[0].ToString()).execute();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        public void delete()
        {
            DataRow dr = forid.Table.Rows[selected];
            if (cm.Table.Rows[selected]["Status"].ToString() == "Aktif")
            {
                new DB("JENIS_PROMO").update("STATUS", "0").where("ID", dr[0].ToString()).execute();
                new DB("PROMO").update("STATUS", "0").where("ID_JENIS_PROMO", dr[0].ToString()).execute();
            }
            else
            {
                new DB("JENIS_PROMO").update("STATUS", "1").where("ID", dr[0].ToString()).execute();
                new DB("PROMO").update("STATUS", "1").where("ID_JENIS_PROMO", dr[0].ToString()).execute();
            }
        }
    }
}
