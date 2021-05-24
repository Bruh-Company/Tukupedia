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
    class CourierViewModel
    {
        KurirModel cm;
        int selected = -1;
        public CourierViewModel()
        {
            cm = new KurirModel();
            reload();
        }

        void reload()
        {
            cm.initAdapter($"select KODE as \"Kode\", NAMA as \"Nama Kurir\", HARGA as \"Harga\", case STATUS when '1' then 'Aktif' else 'Non Aktif' end as \"Status Kurir\" from KURIR where STATUS = '1' order by KODE");
            //foreach (DataRow dr in cm.Table.Rows)
            //{
            //    dr[2] = dr[2].ToString();
            //}
            //MessageBox.Show(cm.Table.Rows[0][2].ToString());
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
        public void update(string nama,string harga)
        {
            DataRow dr = cm.Table.Rows[selected];
            new DB("kurir").update("NAMA", nama,"HARGA", Convert.ToInt32(harga)).where("KODE", dr[0].ToString()).execute();
            //dr[1] = nama;
            //dr[2] = nama;
            //dr[3] = alamat;
            //dr[4] = notelp;
            //dr[5] = lahir.ToString("dd-MM-yyyy");
            //cm.update();
        }
        public bool insert(string nama, string harga)
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
                string kode = Utility.kodeGenerator(nama);
                int konter = 1;
                foreach (DataRow dr in cm.Table.Rows)
                {
                    if (dr[0].ToString().Contains(kode.ToUpper())) konter++;
                }
                kode += Utility.translate(konter, 3);
                DB cmd = new DB();
                cmd.statement = $"insert into KURIR(ID, KODE, NAMA, HARGA) VALUES (100,'{kode.ToUpper()}','{nama}',{harga})";
                cmd.execute();
                return true;
            }
        }
        public void delete()
        {
            DataRow dr = cm.Table.Rows[selected];
            if (true)
            {
                new DB("KURIR").update("STATUS", "0").where("KODE", dr[0].ToString()).execute();
            }
            else
            {
                new DB("KURIR").update("STATUS", "1").where("KODE", dr[0].ToString()).execute();
            }
        }
        public int nice(DataTable nice, string kode)
        {
            int wow = Utility.checkMax(nice, "Kode", 1, 3, $"Kode like'%{kode}%'") + 1;
            return wow;
        }
    }
}
