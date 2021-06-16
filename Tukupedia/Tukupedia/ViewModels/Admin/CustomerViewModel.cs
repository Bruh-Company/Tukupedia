using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tukupedia.Models;
using Tukupedia.Helpers.DatabaseHelpers;

namespace Tukupedia.ViewModels.Admin
{
    class CustomerViewModel
    {
        CustomerModel cm;
        int selected = -1;
        public CustomerViewModel()
        {
            cm = new CustomerModel();
            reload();
        }

        void reload()
        {
            cm.initAdapter($"select KODE as \"Kode\", EMAIL as \"Email\", NAMA as \"Nama User\", ALAMAT as \"Alamat\", NO_TELP as \"Nomor Telepon\", to_char(TANGGAL_LAHIR,'dd-mm-yyyy') as \"Tanggal Lahir\", case STATUS when '1' then 'Aktif' when '0' then 'Banned' end as \"Status\" from CUSTOMER order by KODE");
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
        public bool checkEmail(string email)
        {
            foreach (DataRow dr in cm.Table.Rows)
            {
                if (dr[1].ToString() == email)
                {
                    MessageBox.Show("Email Sudah dipakai, gagal ganti email");
                    return false;
                }
            }
            return true;
        }
        public void update(string nama, string email, string alamat, string notelp, DateTime lahir)
        {
            DataRow dr = cm.Table.Rows[selected];
            if (email == dr[1].ToString()) ;
            else if (!checkEmail(email))
            {
                MessageBox.Show("Email sudah dipakai, tidak dapat mengganti email");
                return;
            }
            new DB("customer").update("TANGGAL_LAHIR", lahir).where("KODE",dr[0].ToString()).execute();
            //MessageBox.Show(dr[0].ToString());
            dr[1] = email;
            dr[2] = nama;
            dr[3] = alamat;
            dr[4] = notelp;
            //dr[5] = lahir.ToString("dd-MM-yyyy");
            cm.update();
        }
        public void ban()
        {
            DataRow dr = cm.Table.Rows[selected];
            if(dr["Status"].ToString() == "Aktif")
            {
                new DB("customer").update("STATUS", "0").where("KODE", dr[0].ToString()).execute();
            }
            else
            {
                new DB("customer").update("STATUS", "1").where("KODE", dr[0].ToString()).execute();
            }
        }
    }
}
