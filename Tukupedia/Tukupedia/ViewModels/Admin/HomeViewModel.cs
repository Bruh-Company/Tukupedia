using LiveCharts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Tukupedia.Helpers.DatabaseHelpers;
using Tukupedia.Helpers.Utils;

namespace Tukupedia.ViewModels.Admin
{
    class HomeViewModel
    {
        DataTable dtJumlahTransaksi, dtJenisPembayaran, dtKurir, dtKategori, dtPromo;
        public HomeViewModel()
        {
            refreshJumlahTransaksi(DateTime.Today,DateTime.Today);
        }

        public void refreshJumlahTransaksi(DateTime awal,DateTime akhir)
        {
            awal = awal == DateTime.Today ? DateTime.Now.AddDays(-7) : awal;
            akhir = akhir == DateTime.Today ? DateTime.Today : akhir;
            akhir = akhir.AddDays(1);
            int rownum = (akhir.Date - awal.Date).Days;
            DB sql = new DB();
            //sql.statement = $"select k.dt, count(h.KODE) from (SELECT to_char(TRUNC (to_date('{Utility.formatDate(akhir)}','dd-mm-yyyy') - ROWNUM),'dd-mm-yyyy') dt FROM DUAL CONNECT BY ROWNUM <= {rownum}) k left join H_TRANS_ITEM h on k.dt = to_char(h.TANGGAL_TRANSAKSI, 'dd-mm-yyyy') group by k.dt, h.TANGGAL_TRANSAKSI order by k.dt asc";
            sql.statement = $"select k.dt, count(h.KODE) from (SELECT TRUNC (to_date('{Utility.formatDate(akhir)}','dd-mm-yyyy') - ROWNUM) as tanggal,to_char(TRUNC (to_date('{Utility.formatDate(akhir)}','dd-mm-yyyy') - ROWNUM),'dd-mm-yyyy') as dt  FROM DUAL CONNECT BY ROWNUM <= {rownum} order by 1 desc) k left join H_TRANS_ITEM h on k.dt = to_char(h.TANGGAL_TRANSAKSI, 'dd-mm-yyyy') group by TANGGAL_TRANSAKSI,tanggal, k.dt order by k.tanggal asc";
            dtJumlahTransaksi = sql.get();
            //return dtJumlahTransaksi;
        }

        public List<string> getLabelJumlahTransaksi()
        {
            List<string> s = new List<string>();
            foreach(DataRow dr in dtJumlahTransaksi.Rows)
            {
                s.Add(dr[0].ToString());
            }
            return s;
        }

        public ChartValues<int> getJumlahTransaksi()
        {
            ChartValues<int> cv = new ChartValues<int>();
            foreach (DataRow dr in dtJumlahTransaksi.Rows)
            {
                cv.Add(Convert.ToInt32(dr[1].ToString()));
            }
            return cv;
        }

        public List<CheckBox> getJenisPembayaran()
        {
            List<CheckBox> jp = new List<CheckBox>();
            DB sql = new DB();
            sql.statement = $"select ID, NAMA from METODE_PEMBAYARAN order by 1 asc";
            dtJenisPembayaran = sql.get();
            foreach(DataRow dt in dtJenisPembayaran.Rows)
            {
                CheckBox cb = new CheckBox();
                cb.Content = dt[1].ToString();
                cb.IsChecked = false;
                jp.Add(cb);
            }
            return jp;
        }

        public List<CheckBox> getKurir()
        {
            List<CheckBox> jp = new List<CheckBox>();
            DB sql = new DB();
            sql.statement = $"select ID, NAMA from KURIR order by 1 asc";
            dtKurir = sql.get();
            foreach (DataRow dt in dtKurir.Rows)
            {
                CheckBox cb = new CheckBox();
                cb.Content = dt[1].ToString();
                cb.IsChecked = false;
                jp.Add(cb);
            }
            return jp;
        }
        public List<CheckBox> getKategori()
        {
            List<CheckBox> jp = new List<CheckBox>();
            DB sql = new DB();
            sql.statement = $"select ID, NAMA from CATEGORY order by 1 asc";
            dtKategori = sql.get();
            foreach (DataRow dt in dtKategori.Rows)
            {
                CheckBox cb = new CheckBox();
                cb.Content = dt[1].ToString();
                cb.IsChecked = false;
                jp.Add(cb);
            }
            return jp;
        }
        public List<CheckBox> getPromo()
        {
            List<CheckBox> jp = new List<CheckBox>();
            DB sql = new DB();
            sql.statement = $"select ID, KODE from PROMO order by 1 asc";
            dtPromo = sql.get();
            foreach (DataRow dt in dtPromo.Rows)
            {
                CheckBox cb = new CheckBox();
                cb.Content = dt[1].ToString();
                cb.IsChecked = false;
                jp.Add(cb);
            }
            return jp;
        }

        public string generateQuery(List<int> jenisPembayaran, List<int> kurir, List<int> kategori, List<int> promo, DateTime awal, DateTime akhir, int isofficial)
        {
            string jenispembayarans = "", kurirs = "", kategoris = "", promos = "";
            getKurir();
            getJenisPembayaran();
            getKategori();
            getPromo();
            akhir = akhir.AddDays(1);
            awal = awal.AddDays(-1);
            foreach (int i in jenisPembayaran)
            {
                DataRow dr = dtJenisPembayaran.Rows[i];
                jenispembayarans += $" or m.ID = {dr[0].ToString()} ";
            }

            if (jenispembayarans != "")
                jenispembayarans = " and (" + jenispembayarans.Substring(4) + ")";

            foreach (int i in kurir)
            {
                DataRow dr = dtKurir.Rows[i];
                kurirs += $" or k.ID = {dr[0].ToString()} ";
            }

            if (kurirs != "")
                kurirs = " and (" + kurirs.Substring(4) + ")";

            foreach (int i in kategori)
            {
                DataRow dr = dtKategori.Rows[i];
                kategoris += $" or i.ID_CATEGORY = {dr[0].ToString()} ";
            }

            if (kategoris != "")
                kategoris = " and (" + kategoris.Substring(4) + ")";

            foreach (int i in promo)
            {
                DataRow dr = dtPromo.Rows[i];
                promos += $" or p.ID = {dr[0].ToString()} ";
            }

            if (promos != "")
                promos = " and (" + promos.Substring(4) + ")";

            string isofficials = isofficial == 0 ? "" : isofficial == 1 ?" and s.IS_OFFICIAL = 1" : " and s.IS_OFFICIAL = 0";
            string query = $"select to_char(h.TANGGAL_TRANSAKSI,'dd-mm-yyyy') as tanggal, nvl(p.KODE,'Tanpa Promo'), c.NAMA, case h.STATUS when 'C' then 'Canceled' when 'W' then 'Waiting Payment' when 'P' then 'Paid' end as status from H_TRANS_ITEM h " +
                $"left join PROMO p on p.ID = h.ID_PROMO {promos}" +
                $"left join CUSTOMER c on h.ID_CUSTOMER = c.ID " +
                $"left join D_TRANS_ITEM d on d.ID_H_TRANS_ITEM = h.ID " +
                $"left join METODE_PEMBAYARAN m on h.ID_METODE_PEMBAYARAN = m.ID {jenispembayarans}" +
                $"left join KURIR k on k.ID = d.ID_KURIR {kurirs}" +
                $"left join ITEM i on d.ID_ITEM = i.ID {kategoris}" +
                $"left join SELLER s on i.ID_SELLER = s.ID {isofficials}" +
                $"where h.TANGGAL_TRANSAKSI between to_date('{Utility.formatDate(awal)}','dd-mm-yyyy') and to_date('{Utility.formatDate(akhir)}','dd-mm-yyyy')";
            
            return query;
        }
    }
}
