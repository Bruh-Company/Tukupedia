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
            sm.initAdapter($"select KODE as \"Kode\", EMAIL as \"Email\", NAMA_TOKO as \"Nama Toko\", ALAMAT as \"Alamat\", NO_TELP as \"Nomor Telepon\", to_char(CREATED_AT,'dd-mm-yyyy') as \"Mendaftar Sejak\", case STATUS when '1' then 'Aktif' when '0' then 'Banned' end as \"Status\", case IS_OFFICIAL when '1' then 'Yes' when '0' then 'No' end as \"is Official\" from SELLER order by KODE");
        }

        public DataTable getDataTable()
        {
            //MessageBox.Show(cm.statement);
            return sm.Table;
        }
        public DataRow selectData(int pos)
        {
            try
            {
                selected = pos;
                return sm.Table.Rows[pos];

            }catch(Exception ex)
            {
                return null;
            }
        }
        public bool checkEmail(string email)
        {
            foreach(DataRow dr in sm.Table.Rows)
            {
                if(dr[1].ToString() == email)
                {
                    MessageBox.Show("Email Sudah dipakai, gagal ganti email");
                    return false;
                }
            }
            return true;
        }
        public void update(string nama, string email, string alamat, string notelp, int official)
        {
            DataRow dr = sm.Table.Rows[selected];
            if (dr[1].ToString() == email) ;
            else if (!checkEmail(email))
            {
                return;
            }
            //new DB("seller").update("TANGGAL_LAHIR", lahir).where("KODE", dr[0].ToString()).execute();
            new DB("seller").update("IS_OFFICIAL", $"{official}", "EMAIL", email, "NAMA_TOKO",nama, "ALAMAT", alamat, "NO_TELP",notelp ).where("KODE", dr[0].ToString()).execute();
            //dr[1] = email;
            //dr[2] = nama;
            //dr[3] = alamat;
            //dr[4] = notelp;
            ////dr[5] = lahir.ToString("dd-MM-yyyy");
            //sm.update();
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
