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

namespace Tukupedia.ViewModels
{
    public static class LoginRegisterViewModel
    {
        private static class CustomerSellerStage
        {
            public static byte Customer = 1;
            public static byte Seller = 2;
        }

        private static class LoginRegisterStage
        {
            public static byte Login = 1;
            public static byte Register = 2;
        }

        //view component and stage status
        public static LoginRegisterView ViewComponent;
        private static byte UserStage; //customer seller
        private static byte CardStage; //login register

        //transition stuff
        private static DispatcherTimer transitionTimer;
        private static TransitionQueue transQueue;

        public static void InitializeView(LoginRegisterView view)
        {
            ViewComponent = view;

            UserStage = CustomerSellerStage.Customer;
            CardStage = LoginRegisterStage.Login;

            transitionTimer = new DispatcherTimer();
        }

        public static bool LoginCustomer(string username, string password)
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

        public static bool LoginSeller(string username, string password)
        {
            if (username == "admin" && password == "admin")
            {
                MessageBox.Show("Berhasil Login Admin");
                HomeAdminView hav = new HomeAdminView();
                hav.ShowDialog();
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

        public static bool RegisterCustomer(string username,string nama, DateTime lahir, string alamat, string notelp, string password)
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

        public static bool RegisterSeller(string username, string nama, string alamat, string notelp, string password)
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

        public static void swapCard()
        {
            if (CardStage == CustomerSellerStage.Customer)
            {
                CardStage = CustomerSellerStage.Seller;

                transQueue = new TransitionQueue();
                makeTransition(ViewComponent.CardCustomer, new Thickness(0, -100, 0, 100), 0);
                makeTransition(ViewComponent.CardSeller, new Thickness(0, 0, 0, 0), 1);

                playTransition();

                Panel.SetZIndex(ViewComponent.CardCustomer, 0);
                Panel.SetZIndex(ViewComponent.CardSeller, 1);
            }
            else
            {
                CardStage = CustomerSellerStage.Customer;

                transQueue = new TransitionQueue();
                makeTransition(ViewComponent.CardCustomer, new Thickness(0, 0, 0, 0), 1);
                makeTransition(ViewComponent.CardSeller, new Thickness(0, 100, 0, -100), 0);

                playTransition();

                Panel.SetZIndex(ViewComponent.CardCustomer, 1);
                Panel.SetZIndex(ViewComponent.CardSeller, 0);
            }
        }

        //transition stuff <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        public static void makeTransition(Control element, Thickness targetMargin, double targetOpacity)
        {
            transQueue.addControlTransition(element, targetMargin, targetOpacity, "with previous");
        }

        public static void playTransition()
        {
            transitionTimer.Interval = TimeSpan.FromMilliseconds(20);
            transitionTimer.Tick += tickTransition;
            transitionTimer.Start();
        }

        public static void tickTransition(object sender, EventArgs e)
        {
            bool finish = transQueue.tick();

            if (finish)
            {
                transitionTimer.Stop();

                transitionTimer = new DispatcherTimer();
            }
        }
        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    }
}
