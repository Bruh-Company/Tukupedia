using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tukupedia.Helpers.DatabaseHelpers;
using Tukupedia.Models;
using Tukupedia.Views.Admin;
using Tukupedia.Views;

namespace Tukupedia.ViewModels
{
    public class LoginRegisterViewModel
    {
        public static LoginRegisterView view;
        public static void setView(LoginRegisterView view)
        {
            LoginRegisterViewModel.view = view;
        }
        public static bool validateRegisterUser(DataTable table,string email)
        {
            int counter = table.Select($"email = '{email}'").Length;
            return counter == 0;
        }
        public static bool login(string username, string password)
        {
            if (username == "admin" && password == "admin")
            {
                MessageBox.Show("Berhasil Login Admin");
                HomeAdminView hav = new HomeAdminView();
                hav.ShowDialog();
                return true;
            }
            DataRow customer = new DB("customer").select()
                .where("email", username)
                .where("password", password)
                .getFirst();
            if (customer == null)
            {
                //MessageBox.Show("Gagal Customer");
            }
            else
            {
                MessageBox.Show("Berhasil Login Customer"+customer[0].ToString());
                return true;
            }

            DataRow seller = new DB("seller").select()
                .where("email", username)
                .where("password", password)
                .getFirst();
            if (seller == null)
            {
                //MessageBox.Show("Gagal Seller");
            }
            else
            {
                MessageBox.Show("Berhasil Login Seller" + seller[0].ToString());
                return true;

            }

            return false;
        }

        public static bool registerCustomer(string username,string nama, DateTime lahir, string alamat, string notelp, string password,string email)
        {
            //Validasi
            bool validation = true;
            if (username == "admin")
            {
                MessageBox.Show("Dilarang jadi Admin");
                validation= false;
            }
            //Check Username/Email Unique
            validation &= validateRegisterUser(new CustomerModel().Table, email);
            //Buat Kode Customer
            if (validation)
            {
                new DB("customer").insert(
                "nama", nama,
                "email", email,
                "tanggal_lahir", $"TO_DATE('{lahir.Month}{lahir.Day}{lahir.Year}', 'MMDDYYYY')",
                "alamat", alamat,
                "no_telp", notelp,
                "password", password
                ).execute();
                MessageBox.Show("Berhasil Daftar");
            }
            else
            {
                MessageBox.Show("Gagal Daftar Custoemer");
            }
            return validation;
        }
        public static bool registerSeller(string username, string nama, DateTime lahir, string alamat, string notelp, string password, string email)
        {
            //Validasi
            bool validation = true;
            if (username == "admin")
            {
                MessageBox.Show("Dilarang jadi Admin");
                validation = false;
            }
            //Check Username/Email sama belum
            validation &= validateRegisterUser(new SellerModel().Table,  email);
            //Buat Kode Customer
            if (validation)
            {
                new DB("seller").insert(
                "nama", nama,
                "email", email,
                "tanggal_lahir", $"TO_DATE('{lahir.Month}{lahir.Day}{lahir.Year}', 'MMDDYYYY')",
                "alamat", alamat,
                "no_telp", notelp,
                "password", password
                ).execute();
                MessageBox.Show("Berhasil Daftar Seller");
            }
            else
            {
                MessageBox.Show("Gagal Daftar Seller");
            }
            return validation;
        }

    }
}
