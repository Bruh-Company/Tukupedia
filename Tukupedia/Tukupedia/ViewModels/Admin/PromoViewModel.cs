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
    class PromoViewModel
    {
        PromoModel cm;
        PromoModel forid;
        PromoModel forcb;
        PromoModel masaberlaku;
        int selected = -1;
        public PromoViewModel()
        {
            cm = new PromoModel();
            forid = new PromoModel();
            forcb = new PromoModel();
            masaberlaku = new PromoModel();
            reload();
        }

        void reload()
        {
            cm.initAdapter($"select p.KODE as \"Kode\", p.POTONGAN as \"Potongan\", p.POTONGAN_MAKS as \"Potongan Maximal\", p.HARGA_MIN as \"Harga Minimal\", case p.JENIS_POTONGAN when 'D' then 'Discount' else 'Potongan' end as \"Jenis Potongan\", j.NAMA as \"Nama Promo\", to_char(p.TANGGAL_AWAL,'dd-mm-yyyy') || 's/d' || to_char(p.TANGGAL_AKHIR,'dd-mm-yyyy') as \"Masa Berlaku\", case p.STATUS when '1' then 'Aktif' else 'Mati' end as \"Status\", to_char(p.CREATED_AT,'dd-mm-yyyy') as \"Dibuat Pada\" from PROMO p, JENIS_PROMO j where p.ID_JENIS_PROMO = j.ID and p.STATUS = '1' order by KODE");
            forid.initAdapter("select p.id from PROMO p, JENIS_PROMO j where p.ID_JENIS_PROMO = j.ID and p.STATUS = '1' order by KODE");
            forcb.initAdapter("select ID, NAMA from JENIS_PROMO order by NAMA");
            forcb.initAdapter("select to_char(p.TANGGAL_AWAL,'dd-mm-yyyy'), to_char(p.TANGGAL_AKHIR,'dd-mm-yyyy') as \"Masa Berlaku\", case p.STATUS when '1' then 'Aktif' else 'Mati' end as \"Status\", to_char(p.CREATED_AT,'dd-mm-yyyy') as \"Dibuat Pada\" from PROMO p, JENIS_PROMO j where p.ID_JENIS_PROMO = j.ID and p.STATUS = '1' order by KODE");
        }

        public DataTable getDataTable()
        {
            //MessageBox.Show(cm.statement);
            return cm.Table;
        }
        public DataTable getForCb()
        {
            return forcb.Table;
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
        public bool insert(string kode, string potongan, string potonganmax, string hargamin, string jenispotongan, string idjenispromo, DateTime awal, DateTime akhir)
        {
            try
            {
                new DB("PROMO").insert(
                "KODE", kode,
                "POTONGAN", potongan,
                "POTONGAN_MAKS", potonganmax,
                "HARGA_MIN", hargamin,
                "JENIS_POTONGAN", jenispotongan,
                "ID_JENIS_PROMO", idjenispromo,
                "TANGGAL_AWAL", awal,
                "TANGGAL_AKHIR", akhir).execute();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            return true;


        }
        public void update(string kode, string potongan, string potonganmax, string hargamin, string jenispotongan, string idjenispromo, DateTime awal, DateTime akhir)
        {
            try
            {
                DataRow dr = forid.Table.Rows[selected];
                new DB("PROMO").update(
                    "KODE", kode,
                    "POTONGAN", potongan,
                    "POTONGAN_MAKS", potonganmax,
                    "HARGA_MIN", hargamin,
                    "JENIS_POTONGAN", jenispotongan,
                    "ID_JENIS_PROMO", idjenispromo,
                    "TANGGAL_AWAL", awal,
                    "TANGGAL_AKHIR", akhir).where("ID",dr[0].ToString()).execute();
            }
            catch(Exception ex)
            {
                ex.ToString();
            }
        }
        public void delete()
        {
            DataRow dr = forid.Table.Rows[selected];
            if (dr["Status"].ToString() == "Aktif")
            {
                new DB("PROMO").update("STATUS", "0").where("ID", dr[0].ToString()).execute();
            }
            else
            {
                new DB("PROMO").update("STATUS", "1").where("ID", dr[0].ToString()).execute();
            }
        }
    }
}
