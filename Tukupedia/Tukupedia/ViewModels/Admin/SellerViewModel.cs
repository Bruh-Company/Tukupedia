using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tukupedia.Helpers.DatabaseHelpers;
using Tukupedia.Models;

namespace Tukupedia.ViewModels.Admin
{
    class SellerViewModel
    {
        SellerModel sm;
        int selected = -1;
        public SellerViewModel()
        {
            sm = new SellerModel();
            reload();
        }

        void reload()
        {
            sm.initAdapter($"select KODE as \"Kode\", EMAIL as \"Email\", NAMA as \"Nama User\", ALAMAT as \"Alamat\", NO_TELP as \"Nomor Telepon\", to_char(TANGGAL_LAHIR,'dd-mm-yyyy') as \"Tanggal Lahir\", case STATUS when '1' then 'Aktif' when '0' then 'Banned' end as \"Status\", case IS_OFFICIAL when '1' then 'Yes' when '0' then 'No' end as \"is Official\" from SELLER order by KODE");
        }

        public DataTable getDataTable()
        {
            //MessageBox.Show(cm.statement);
            return sm.Table;
        }
        public DataRow selectData(int pos)
        {
            selected = pos;
            return sm.Table.Rows[pos];
        }
        public void update(string nama, string email, string alamat, string notelp, DateTime lahir, int official)
        {
            DataRow dr = sm.Table.Rows[selected];
            new DB("seller").update("TANGGAL_LAHIR", $"TO_DATE('{lahir.ToString("dd-MM-yyyy")}','dd-mm-yyyy')").where("KODE", dr[0].ToString()).execute();
            new DB("seller").update("IS_OFFICIAL", $"{official}").where("KODE", dr[0].ToString()).execute();
            dr[1] = email;
            dr[2] = nama;
            dr[3] = alamat;
            dr[4] = notelp;
            //dr[5] = lahir.ToString("dd-MM-yyyy");
            sm.update();
        }
        public void ban()
        {
            DataRow dr = sm.Table.Rows[selected];
            if (dr["Status"].ToString() == "Aktif")
            {
                new DB("seller").update("STATUS", "0").where("KODE", dr[0].ToString()).execute();
            }
            else
            {
                new DB("seller").update("STATUS", "1").where("KODE", dr[0].ToString()).execute();
            }
        }
    }
}
