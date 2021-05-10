using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tukupedia.Models;
using Tukupedia.Helpers.Utils;
using Tukupedia.Helpers.DatabaseHelpers;
using System.Windows;

namespace Tukupedia.ViewModels.Admin
{
    class CategoryViewModel
    {
        CategoryModel cm;
        int selected = -1;
        public CategoryViewModel()
        {
            cm = new CategoryModel();
            reload();
        }

        void reload()
        {
            cm.initAdapter($"select KODE as \"Kode\", NAMA as \"Nama Kategori\", case STATUS when '1' then 'Aktif' else 'Non Aktif' end as \"Status Kategori\" from CATEGORY where STATUS = '1' order by KODE");
        }

        public DataTable getDataTable()
        {
            //MessageBox.Show(cm.statement);
            return cm.Table;
        }
        public DataRow selectData(int pos)
        {
            selected = pos;
            return cm.Table.Rows[pos];
        }
        public void update(string nama)
        {
            DataRow dr = cm.Table.Rows[selected];
            new DB("category").update("NAMA", nama).where("KODE", dr[0].ToString()).execute();
            //dr[1] = nama;
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
                string[] kotak = nama.Split(' ');
                string kode = "";
                if (kotak.Length == 1)
                {
                    kode += kotak[0].Substring(0, 2);
                }
                else
                {
                    kode += kotak[0].Substring(0, 1) + kotak[1].Substring(0, 1);
                }
                int konter = 1;
                foreach(DataRow dr in cm.Table.Rows){
                    if (dr[0].ToString().Contains(kode.ToUpper()))konter++ ;
                }
                kode += Utility.translate(konter, 3);
                DB cmd = new DB();
                cmd.statement = $"insert into CATEGORY(ID, KODE, NAMA) VALUES (100,'{kode.ToUpper()}','{nama}')";
                cmd.execute();
                return true;
            }
        }
        public void delete()
        {
            DataRow dr = cm.Table.Rows[selected];
            if (dr["Status"].ToString() == "Aktif")
            {
                new DB("customer").update("STATUS", "0").where("KODE", dr[0].ToString()).execute();
            }
            else
            {
                new DB("customer").update("STATUS", "1").where("KODE", dr[0].ToString()).execute();
            }
        }
        public int nice(DataTable nice, string kode)
        {
            int wow = Utility.checkMax(nice, "Kode", 1, 3, $"Kode like'%{kode}%'")+1;
            return wow;
        }
    }
}
