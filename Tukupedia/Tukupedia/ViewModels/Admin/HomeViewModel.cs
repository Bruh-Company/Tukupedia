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
using CrystalDecisions.Shared;
using Tukupedia.Report;
using Tukupedia.Views;

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
            akhir.AddDays(1);
            int rownum = (akhir.Date - awal.Date).Days;
            DB sql = new DB();
            sql.statement = $"select k.dt, count(h.KODE) from (SELECT TRUNC (to_date('{Utility.formatDate(akhir)}','dd-mm-yyyy') - ROWNUM) as tanggal,to_char(TRUNC (to_date('{Utility.formatDate(akhir)}','dd-mm-yyyy') - ROWNUM),'dd-mm-yyyy') as dt  FROM DUAL CONNECT BY ROWNUM <= {rownum} order by 1 desc) k left join H_TRANS_ITEM h on k.dt = to_char(h.TANGGAL_TRANSAKSI, 'dd-mm-yyyy') group by TANGGAL_TRANSAKSI,tanggal, k.dt order by k.tanggal asc";
            dtJumlahTransaksi = sql.get();
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
            sql.statement = $"select ID, NAMA from METODE_PEMBAYARAN WHERE STATUS = 1 order by 1 asc";
            dtJenisPembayaran = sql.get();
            foreach(DataRow dt in dtJenisPembayaran.Rows)
            {
                CheckBox cb = new CheckBox();
                cb.Content = dt[1].ToString();
                cb.IsChecked = true;
                jp.Add(cb);
            }
            return jp;
        }

        public List<CheckBox> getKurir()
        {
            List<CheckBox> jp = new List<CheckBox>();
            DB sql = new DB();
            sql.statement = $"select ID, NAMA from KURIR WHERE STATUS = 1 order by 1 asc";
            dtKurir = sql.get();
            foreach (DataRow dt in dtKurir.Rows)
            {
                CheckBox cb = new CheckBox();
                cb.Content = dt[1].ToString();
                cb.IsChecked = true;
                jp.Add(cb);
            }
            return jp;
        }
        public List<CheckBox> getKategori()
        {
            List<CheckBox> jp = new List<CheckBox>();
            DB sql = new DB();
            sql.statement = $"select ID, NAMA from CATEGORY WHERE STATUS = 1 order by 1 asc";
            dtKategori = sql.get();
            foreach (DataRow dt in dtKategori.Rows)
            {
                CheckBox cb = new CheckBox();
                cb.Content = dt[1].ToString();
                cb.IsChecked = true;
                jp.Add(cb);
            }
            return jp;
        }
        public List<CheckBox> getPromo()
        {
            List<CheckBox> jp = new List<CheckBox>();
            DB sql = new DB();
            sql.statement = $"select ID, KODE from PROMO WHERE STATUS = 1 order by 1 asc";
            dtPromo = sql.get();
            foreach (DataRow dt in dtPromo.Rows)
            {
                CheckBox cb = new CheckBox();
                cb.Content = dt[1].ToString();
                cb.IsChecked = true;
                jp.Add(cb);
            }
            return jp;
        }

        public ParameterDiscreteValue getParamVal(Object val)
        {
            ParameterDiscreteValue param = new ParameterDiscreteValue();
            param.Value = val;
            return param;
        }
        public void generateReport(List<int> jenisPembayaran, List<int> kurir, List<int> kategori, List<int> promo, DateTime awal, DateTime akhir, int isofficial)
        {
            AdminReport report = new AdminReport();
            ParameterValues pv = new ParameterValues();
            ReportView rv = new ReportView(report);
            getKurir();
            getJenisPembayaran();
            getKategori();
            getPromo();
            akhir = akhir.AddDays(1);
            awal = awal.AddDays(-1);

            pv.Clear();
            foreach (int i in jenisPembayaran)
            {
                DataRow dr = dtJenisPembayaran.Rows[i];
                pv.Add(getParamVal(dr["ID"].ToString()));
            }
            rv.setParam("payment_Methods", pv);
            pv.Clear();
            foreach (int i in kurir)
            {
                DataRow dr = dtKurir.Rows[i];
                pv.Add(getParamVal(dr["ID"].ToString()));
            }
            rv.setParam("Kurirs", pv);
            pv.Clear();
            foreach (int i in kategori)
            {
                DataRow dr = dtKategori.Rows[i];
                pv.Add(getParamVal(dr["ID"].ToString()));
                
            }
            rv.setParam("Categories", pv);
            pv.Clear();
            foreach (int i in promo)
            {
                DataRow dr = dtPromo.Rows[i];
                pv.Add(getParamVal(dr["ID"].ToString()));
            }
            rv.setParam("Promos", pv);

            rv.ShowDialog();


        }
    }
}
