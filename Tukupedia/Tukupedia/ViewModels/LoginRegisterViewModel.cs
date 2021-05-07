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
                .orWhere("username",username)
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

        public static bool registerUser(string username,string nama, DateTime lahir, string alamat, string notelp, string password)
        {

            if (username == "admin")
            {
                MessageBox.Show("Dilarang jadi Admin");
                return false;
            }
            new DB("customer").insert(
                "nama", nama,
                "email", "test",
                "tanggal_lahir", "TO_DATE('070903', 'MMDDYY')",
                "alamat", alamat,
                "no_telp", notelp,
                "password", password
                ).execute();
            return false;
            
        }
        public static bool registerseller(string username, string nama, string alamat, string notelp, string password)
        {

            if (username == "admin")
            {
                MessageBox.Show("Dilarang jadi Admin");
                return false;
            }
            DB db = new DB("customer");
            DataRow customer = db.select()
                .where("email", username)
                .where("password", password)
                .getFirst();
            if (customer == null)
            {
                //MessageBox.Show("Gagal Customer");
            }
            else
            {
                MessageBox.Show("Berhasil Login Customer" + customer[0].ToString());
                return true;
            }

            db = new DB("seller");
            DataRow seller = db.select()
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

    }
}
