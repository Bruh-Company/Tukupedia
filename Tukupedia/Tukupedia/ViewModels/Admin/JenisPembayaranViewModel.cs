using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tukupedia.Helpers.DatabaseHelpers;
using Tukupedia.Helpers.Utils;
using Tukupedia.Models;

namespace Tukupedia.ViewModels.Admin
{
    class JenisPembayaranViewModel
    {
        Metode_PembayaranModel cm;
        int selected = -1;
        Metode_PembayaranModel forid;
        public JenisPembayaranViewModel()
        {
            cm = new Metode_PembayaranModel();
            forid = new Metode_PembayaranModel();
            reload();
        }

        void reload()
        {
            forid.initAdapter($"select ID from METODE_PEMBAYARAN where STATUS = '1' order by NAMA");
            cm.initAdapter($"select NAMA as \"Jenis Pembayaran\", case STATUS when '1' then 'Aktif' else 'Non Aktif' end as \"Status Pembayaran\" from METODE_PEMBAYARAN where STATUS = '1' order by NAMA");
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
        public void update(string nama)
        {
            DataRow dr = forid.Table.Rows[selected];
            new DB("METODE_PEMBAYARAN").update("NAMA", nama).where("ID", dr[0].ToString()).execute();
            //dr[0] = nama;
            //dr[2] = nama;
            //dr[3] = alamat;
            //dr[4] = notelp;
            //dr[5] = lahir.ToString("dd-MM-yyyy");
            //cm.update();
        }
        public bool insert(string nama)
        {
            if (nama == "")
            {
                MessageBox.Show("Nama dilarang kosong");
                return false;
            }
            else if (nama.Length < 2)
            {
                MessageBox.Show("Nama tidak boleh kurang dari 2 huruf");
                return false;
            }
            else
            {
                //string kode = Utility.kodegenerator(nama);
                //int konter = 1;
                //foreach (DataRow dr in cm.Table.Rows)
                //{
                //    if (dr[0].ToString().Contains(kode.ToUpper())) konter++;
                //}
                //kode += Utility.translate(konter, 3);
                DB cmd = new DB();
                cmd.statement = $"insert into METODE_PEMBAYARAN(ID, NAMA, STATUS) VALUES (100,'{nama}', '1')";
                cmd.execute();
                return true;
            }
        }
        public void delete()
        {
            DataRow dr = forid.Table.Rows[selected];
            if (dr["Status"].ToString() == "Aktif")
            {
                new DB("METODE_PEMBAYARAN").update("STATUS", "0").where("KODE", dr[0].ToString()).execute();
            }
            else
            {
                new DB("METODE_PEMBAYARAN").update("STATUS", "1").where("KODE", dr[0].ToString()).execute();
            }
        }
    }
}
