using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Tukupedia.Helpers.DatabaseHelpers;
using Tukupedia.Models;
using Tukupedia.Views.Admin;
using Tukupedia.Views;
using System.Windows.Controls;
using Tukupedia.Helpers.Utils;

namespace Tukupedia.ViewModels
{
    public static class LoginRegisterViewModel
    {
        public enum CustomerSellerStage
        {
            Customer,
            Seller
        }

        public enum CardPage
        {
            LoginPage,
            RegisterFirstPage,
            RegisterSecondPage,
            RegisterThirdPage
        }

        //view component and stage status
        public static LoginRegisterView ViewComponent;
        private static CustomerSellerStage UserStage; //customer seller
        private static CardPage CardStage; //login register

        //transition
        private static Transition transition;
        private const int transFPS = 100;

        public static void InitializeView(LoginRegisterView view)
        {
            ViewComponent = view;

            UserStage = CustomerSellerStage.Customer;
            CardStage = CardPage.LoginPage;

            transition = new Transition(transFPS);
        }

        public static bool validateRegisterUser(DataTable table,string email)
        {
            int counter = table.Select($"email = '{email}'").Length;
            return counter == 0;
        }

        public static void LoginCustomer(string username, string password)
        {
            if (username == "admin" && password == "admin")
            {
                MessageBox.Show("Berhasil Login Admin");
                Session.isLogin = true;
                Session.Login(null, "Admin");
            }
            DataRow customer = new DB("customer").select()
                .where("email", username)
                .where("password", password)
                .getFirst();

            if (customer == null)
            {
                MessageBox.Show("Gagal Login Customer");
            }
            else
            {
                MessageBox.Show("Berhasil Login Customer \n Selamat Datang "+customer["NAMA"].ToString());
                Session.Login(customer,"customer");
                Session.isLogin = true;
            }

        }

        public static void LoginSeller(string username, string password)
        {
            if (username == "admin" && password == "admin")
            {
                MessageBox.Show("Berhasil Login Admin");
                Session.isLogin = true;
                Session.Login(null, "Admin");
            }
            DataRow seller = new DB("seller").select()
                .where("email", username)
                .where("password", password)
                .getFirst();

            if (seller == null)
            {
                MessageBox.Show("Gagal Login Seller");
            }
            else
            {
                MessageBox.Show("Berhasil Login Seller \n Selamat Datang " + seller["NAMA"].ToString());
                Session.Login(seller, "Seller");
                Session.isLogin = true;
            }

        }

        public static bool RegisterCustomer(string username,string nama, DateTime lahir, string alamat, string notelp, string password, string email)
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
                MessageBox.Show("Gagal Daftar Customer");
            }
            return validation;
        }

        public static bool RegisterSeller(string username, string nama, string alamat, string notelp, string password, string email, DateTime lahir)
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

        public static void swapCard()
        {
            const double speedMargin = 0.3;
            const double speedOpacity = 0.5;
            const double multiplier = 80;

            if (UserStage == CustomerSellerStage.Customer)
            {
                UserStage = CustomerSellerStage.Seller;

                transition.makeTransition(ViewComponent.CardCustomer,
                    new Thickness(0, -100, 0, 100), 0,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");
                transition.makeTransition(ViewComponent.CardSeller,
                    new Thickness(0, 0, 0, 0), 1,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");

                transition.playTransition();

                ComponentHelper.changeVisibilityComponent(
                    ViewComponent.CardCustomer,
                    Visibility.Hidden);
                ComponentHelper.changeVisibilityComponent(
                    ViewComponent.CardSeller,
                    Visibility.Visible);
            }
            else
            {
                UserStage = CustomerSellerStage.Customer;
                
                transition.makeTransition(ViewComponent.CardCustomer,
                    new Thickness(0, 0, 0, 0), 1,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");
                transition.makeTransition(ViewComponent.CardSeller,
                    new Thickness(0, 100, 0, -100), 0,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");

                transition.playTransition();

                ComponentHelper.changeVisibilityComponent(
                    ViewComponent.CardSeller,
                    Visibility.Hidden);
                ComponentHelper.changeVisibilityComponent(
                    ViewComponent.CardCustomer,
                    Visibility.Visible);
            }
        }

        public static void swapPage()
        {

        }
    }
}
