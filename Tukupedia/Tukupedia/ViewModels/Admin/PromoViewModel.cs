using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
            cm.initAdapter($"select p.KODE as \"Kode\", p.POTONGAN as \"Potongan\", p.POTONGAN_MAKS as \"Potongan Maximal\", p.HARGA_MIN as \"Harga Minimal\", case p.JENIS_POTONGAN when 'P' then 'Persenan' else 'Fixed' end as \"Jenis Potongan\", j.NAMA as \"Nama Promo\", to_char(p.TANGGAL_AWAL,'dd-mm-yyyy') || ' s/d ' || to_char(p.TANGGAL_AKHIR,'dd-mm-yyyy') as \"Masa Berlaku\", case p.STATUS when '1' then 'Aktif' else 'Non Aktif' end as \"Status\", to_char(p.CREATED_AT,'dd-mm-yyyy') as \"Dibuat Pada\" from PROMO p, JENIS_PROMO j where p.ID_JENIS_PROMO = j.ID order by KODE");
            forid.initAdapter("select p.id, p.ID_JENIS_PROMO from PROMO p, JENIS_PROMO j where p.ID_JENIS_PROMO = j.ID order by KODE");
            forcb.initAdapter("select ID, NAMA from JENIS_PROMO order by NAMA");
            masaberlaku.initAdapter("select to_char(p.TANGGAL_AWAL,'dd-mm-yyyy'), to_char(p.TANGGAL_AKHIR,'dd-mm-yyyy') from PROMO p, JENIS_PROMO j where p.ID_JENIS_PROMO = j.ID order by KODE");
        }
        public DataRow getMasaBerlaku() {
            try
            {
                return masaberlaku.Table.Rows[selected];
            }
            catch (Exception ex)
            {
                return null;
            }
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
            if (awal > akhir)
            {
                MessageBox.Show("Tanggal Awal melebihi tanggal akhir");
                return false;
            }
            if(kode == "" || idjenispromo == "" || jenispotongan == "")
            {
                MessageBox.Show("Kode, jenis promo atau jenis potongan masih kosong, gagal insert promo");
                return false;
            }
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
        public bool update(string kode, string potongan, string potonganmax, string hargamin, string jenispotongan, string idjenispromo, DateTime awal, DateTime akhir)
        {
            if (awal > akhir)
            {
                MessageBox.Show("Tanggal Awal melebihi tanggal akhir");
                return false;
            }
            if (akhir > DateTime.Today)
            {
                MessageBox.Show("Tanggal akhir kurang dari hari ini, status dimatikan");
                delete(1);
            }
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
            return true;
        }
        public void delete(int pos = 0)
        {
            DataRow dr = cm.Table.Rows[selected];
            DataRow id = forid.Table.Rows[selected];
            if (dr["Status"].ToString() == "Aktif" || pos == 1)
            {
                new DB("PROMO").update("STATUS", "0").where("ID", id[0].ToString()).execute();
            }
            else
            {
                DB debe = new DB();
                debe.statement = $"select status from jenis_promo where id = '{id[1].ToString()}'";
                DataRow datarow = debe.getFirst();
                if(datarow[0].ToString() == "0")
                {
                    MessageBox.Show("Jenis Promo ini mati, tidak dapat menghidupkan promo");
                    return;
                }
                DateTime datetime = DateTime.ParseExact(masaberlaku.Table.Rows[selected][1].ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                if(datetime < DateTime.Today)
                {
                    MessageBox.Show("Tanggal akhir kurang dari hari ini, gagal mengaktifkan promo");
                    return;
                }
                new DB("PROMO").update("STATUS", "1").where("ID", id[0].ToString()).execute();
            }
        }
    }
}
