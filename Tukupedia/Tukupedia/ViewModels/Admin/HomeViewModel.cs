using LiveCharts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tukupedia.Helpers.DatabaseHelpers;
using Tukupedia.Helpers.Utils;

namespace Tukupedia.ViewModels.Admin
{
    class HomeViewModel
    {
        DataTable dtJumlahTransaksi;
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
    }
}
