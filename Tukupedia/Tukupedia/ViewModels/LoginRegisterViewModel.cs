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
    public static class MarginPosition {
        public static Thickness Up = new Thickness(0, -100, 0, 100);
        public static Thickness Left = new Thickness(-100, 0, 100, 0);
        public static Thickness Down = new Thickness(0, 100, 0, -100);
        public static Thickness Right = new Thickness(100, 0, -100, 0);
        public static Thickness Middle = new Thickness(0, 0, 0, 0);
    }

    public static class LoginRegisterViewModel
    {
        private enum CustomerSellerStage
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

        public static void InitializeCard()
        {
            ViewComponent.CardSeller.Margin = MarginPosition.Down;
            ViewComponent.CardSeller.Width = 397;
            ViewComponent.CardSeller.Height = 433;
            ComponentHelper.changeVisibilityComponent(ViewComponent.CardSeller,
                Visibility.Hidden);

            ViewComponent.CardCustomer.Margin = MarginPosition.Middle;
            ViewComponent.CardCustomer.Width = 397;
            ViewComponent.CardCustomer.Height = 433;
            ComponentHelper.changeVisibilityComponent(ViewComponent.CardCustomer,
                Visibility.Visible);
        }

        public static void InitializeState()
        {
            ViewComponent.GridLoginCustomer.Margin = MarginPosition.Middle;
            ViewComponent.GridLoginCustomer.Width = 376.8;
            ViewComponent.GridLoginCustomer.Height = 413;
            ComponentHelper.changeVisibilityComponent(ViewComponent.GridLoginCustomer, Visibility.Visible);

            ViewComponent.GridRegisterCustomer1.Margin = MarginPosition.Right;
            ViewComponent.GridRegisterCustomer1.Opacity = 0;
            ViewComponent.GridRegisterCustomer1.Width = 376.8;
            ViewComponent.GridRegisterCustomer1.Height = 413;
            ComponentHelper.changeVisibilityComponent(ViewComponent.GridRegisterCustomer1, Visibility.Hidden);

            ViewComponent.GridRegisterCustomer2.Margin = MarginPosition.Right;
            ViewComponent.GridRegisterCustomer2.Opacity = 0;
            ViewComponent.GridRegisterCustomer2.Width = 376.8;
            ViewComponent.GridRegisterCustomer2.Height = 413;
            ComponentHelper.changeVisibilityComponent(ViewComponent.GridRegisterCustomer2, Visibility.Hidden);

            ViewComponent.GridRegisterCustomer3.Margin = MarginPosition.Right;
            ViewComponent.GridRegisterCustomer3.Opacity = 0;
            ViewComponent.GridRegisterCustomer3.Width = 376.8;
            ViewComponent.GridRegisterCustomer3.Height = 413;
            ComponentHelper.changeVisibilityComponent(ViewComponent.GridRegisterCustomer3, Visibility.Hidden);
        }

        // DATABASE STUFF >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

        public static bool validateRegisterUser(DataTable table,string email)
        {
            int counter = table.Select($"email = '{email}'").Length;
            return counter == 0;
        }

        public static string generateCode(DataTable table, string nama)
        {
            string kode = Utility.kodeGenerator(nama);
            int counter = table.Select($"nama = '{kode}'").Length;
            return kode.ToUpper() + Utility.translate(counter, 3);
        }

        public static void LoginCustomer(string username, string password)
        {
            if (username == "admin" && password == "admin")
            {
                MessageBox.Show("Berhasil Login Admin");
                Session.isLogin = true;
                Session.Login(null, "Admin");
            }
            else
            {
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
                    MessageBox.Show("Berhasil Login Customer \n Selamat Datang " + customer["NAMA"].ToString());
                    Session.Login(customer, "customer");
                    Session.isLogin = true;
                }
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
            else
            {
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

        }

        public static bool RegisterCustomer(string email,string nama, DateTime lahir, string alamat, string notelp, string password)
        {
            //Validasi
            bool validation = true;
            if (email == "admin")
            {
                MessageBox.Show("Dilarang jadi Admin");
                validation= false;
            }
            //Check Username/Email Unique
            else
            {
                validation &= validateRegisterUser(new CustomerModel().Table, email);
                //Buat Kode Customer
                if (validation)
                {
                    new DB("customer").insert(
                    "nama", nama,
                    "email", email,
                    "tanggal_lahir", lahir,
                    "alamat", alamat,
                    "no_telp", notelp,
                    "password", password,
                    "kode", generateCode(new CustomerModel().Table, nama)
                    ).execute();
                    MessageBox.Show("Berhasil Daftar");
                }
                else
                {
                    MessageBox.Show("Gagal Daftar Customer");
                }
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
            else
            {
                //Check Username/Email sama belum
                validation &= validateRegisterUser(new SellerModel().Table, email);
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
            }
            return validation;
        }

        // END OF DATABASE STUFF <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

        public static void swapCard()
        {
            const double speedMargin = 0.2;
            const double speedOpacity = 0.3;
            const double multiplier = 80;

            if (UserStage == CustomerSellerStage.Customer)
            {
                UserStage = CustomerSellerStage.Seller;

                transition.makeTransition(ViewComponent.CardCustomer,
                    MarginPosition.Up, 0,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");
                transition.makeTransition(ViewComponent.CardSeller,
                    MarginPosition.Middle, 1,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");

                transition.setCallback(swapCardCallback);
                transition.playTransition();

                ComponentHelper.changeZIndexComponent(
                    ViewComponent.CardCustomer,
                    Visibility.Hidden);
                ComponentHelper.changeZIndexComponent(
                    ViewComponent.CardSeller,
                    Visibility.Visible);

                ViewComponent.btnSwap.Content = "I'm Customer";
            }
            else
            {
                UserStage = CustomerSellerStage.Customer;
                
                transition.makeTransition(ViewComponent.CardCustomer,
                    MarginPosition.Middle, 1,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");
                transition.makeTransition(ViewComponent.CardSeller,
                    MarginPosition.Down, 0,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");

                transition.setCallback(swapCardCallback);
                transition.playTransition();

                ComponentHelper.changeZIndexComponent(
                    ViewComponent.CardSeller,
                    Visibility.Hidden);
                ComponentHelper.changeZIndexComponent(
                    ViewComponent.CardCustomer,
                    Visibility.Visible);

                ViewComponent.btnSwap.Content = "I'm Seller";
            }
        }

        public static void swapCardCallback()
        {
            if (UserStage == CustomerSellerStage.Seller)
            {
                ComponentHelper.changeVisibilityComponent(
                    ViewComponent.CardCustomer,
                    Visibility.Hidden);
                ComponentHelper.changeVisibilityComponent(
                    ViewComponent.CardSeller,
                    Visibility.Visible);
            }
            else
            {
                ComponentHelper.changeVisibilityComponent(
                    ViewComponent.CardCustomer,
                    Visibility.Visible);
                ComponentHelper.changeVisibilityComponent(
                    ViewComponent.CardSeller,
                    Visibility.Hidden);
            }
        }

        public static void swapPage(CardPage goTo)
        {
            const double speedMargin = 0.3;
            const double speedOpacity = 0.4;
            const double multiplier = 80;

            CardStage = goTo;

            if (goTo == CardPage.LoginPage)
            {
                transition.makeTransition(ViewComponent.GridLoginCustomer,
                    MarginPosition.Middle, 1,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");
                transition.makeTransition(ViewComponent.GridRegisterCustomer1,
                    MarginPosition.Right, 0,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");
                transition.makeTransition(ViewComponent.GridRegisterCustomer2,
                    MarginPosition.Right, 0,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");
                transition.makeTransition(ViewComponent.GridRegisterCustomer3,
                    MarginPosition.Right, 0,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");

                transition.playTransition();
                ComponentHelper.changeZIndexComponent(
                    ViewComponent.GridLoginCustomer,
                    Visibility.Visible);
                ComponentHelper.changeZIndexComponent(
                    ViewComponent.GridRegisterCustomer1,
                    Visibility.Hidden);
                ComponentHelper.changeZIndexComponent(
                    ViewComponent.GridRegisterCustomer2,
                    Visibility.Hidden);
                ComponentHelper.changeZIndexComponent(
                    ViewComponent.GridRegisterCustomer3,
                    Visibility.Hidden);
            }
            else if (goTo == CardPage.RegisterFirstPage)
            {
                transition.makeTransition(ViewComponent.GridLoginCustomer,
                    MarginPosition.Left, 0,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");
                transition.makeTransition(ViewComponent.GridRegisterCustomer1,
                    MarginPosition.Middle, 1,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");
                transition.makeTransition(ViewComponent.GridRegisterCustomer2,
                    MarginPosition.Right, 0,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");
                transition.makeTransition(ViewComponent.GridRegisterCustomer3,
                    MarginPosition.Right, 0,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");

                transition.playTransition();
                ComponentHelper.changeZIndexComponent(
                    ViewComponent.GridLoginCustomer,
                    Visibility.Hidden);
                ComponentHelper.changeZIndexComponent(
                    ViewComponent.GridRegisterCustomer1,
                    Visibility.Visible);
                ComponentHelper.changeZIndexComponent(
                    ViewComponent.GridRegisterCustomer2,
                    Visibility.Hidden);
                ComponentHelper.changeZIndexComponent(
                    ViewComponent.GridRegisterCustomer3,
                    Visibility.Hidden);
            }
            else if (goTo == CardPage.RegisterSecondPage)
            {
                transition.makeTransition(ViewComponent.GridLoginCustomer,
                    MarginPosition.Left, 0,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");
                transition.makeTransition(ViewComponent.GridRegisterCustomer1,
                    MarginPosition.Left, 0,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");
                transition.makeTransition(ViewComponent.GridRegisterCustomer2,
                    MarginPosition.Middle, 1,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");
                transition.makeTransition(ViewComponent.GridRegisterCustomer3,
                    MarginPosition.Right, 0,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");

                transition.playTransition();
                ComponentHelper.changeZIndexComponent(
                    ViewComponent.GridLoginCustomer,
                    Visibility.Hidden);
                ComponentHelper.changeZIndexComponent(
                    ViewComponent.GridRegisterCustomer1,
                    Visibility.Hidden);
                ComponentHelper.changeZIndexComponent(
                    ViewComponent.GridRegisterCustomer2,
                    Visibility.Visible);
                ComponentHelper.changeZIndexComponent(
                    ViewComponent.GridRegisterCustomer3,
                    Visibility.Hidden);
            }
            else if (goTo == CardPage.RegisterThirdPage)
            {
                transition.makeTransition(ViewComponent.GridLoginCustomer,
                    MarginPosition.Left, 0,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");
                transition.makeTransition(ViewComponent.GridRegisterCustomer1,
                    MarginPosition.Left, 0,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");
                transition.makeTransition(ViewComponent.GridRegisterCustomer2,
                    MarginPosition.Left, 0,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");
                transition.makeTransition(ViewComponent.GridRegisterCustomer3,
                    MarginPosition.Middle, 1,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");

                transition.playTransition();
                ComponentHelper.changeZIndexComponent(
                    ViewComponent.GridLoginCustomer,
                    Visibility.Hidden);
                ComponentHelper.changeZIndexComponent(
                    ViewComponent.GridRegisterCustomer1,
                    Visibility.Hidden);
                ComponentHelper.changeZIndexComponent(
                    ViewComponent.GridRegisterCustomer2,
                    Visibility.Visible);
                ComponentHelper.changeZIndexComponent(
                    ViewComponent.GridRegisterCustomer3,
                    Visibility.Visible);
            }
        }
    }
}
