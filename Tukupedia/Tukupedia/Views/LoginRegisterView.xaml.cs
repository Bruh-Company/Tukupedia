using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Tukupedia.ViewModels;
using Tukupedia.Helpers.Utils;
using Tukupedia.Views.Admin;
using Tukupedia.Views.Customer;
using Tukupedia.Views.Seller;

namespace Tukupedia.Views
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginRegisterView : Window
    {
        public LoginRegisterView()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            LoginRegisterViewModel.InitializeView(this);
            loadInit();

            Panel.SetZIndex(CardCustomer, 1);
            Panel.SetZIndex(CardSeller, 0);
        }

        private void loadInit()
        {
            imgLogo.Source =
                new BitmapImage(new Uri(
                    AppDomain.CurrentDomain.BaseDirectory +
                    "Resource\\Logo\\TukupediaLogo.png"));

            LoginRegisterViewModel.InitializeCard();
            LoginRegisterViewModel.InitializeState();

            dpCustomerBornDateRegister.DisplayDateEnd = DateTime.Now;
        }

        public void closeThis()
        {
            this.Close();
        }


        private void swapCard(object sender, RoutedEventArgs e)
        {
            LoginRegisterViewModel.swapCard();
        }
        
        private void btnCustomerLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginRegisterViewModel
                .LoginCustomer
                (tbCustomerEmailLogin.Text, tbCustomerPasswordLogin.Password);
            if (Session.isLogin)
            {
                this.Hide();
                if (Session.role.ToUpper() == "ADMIN")
                {
                    HomeAdminView hav = new HomeAdminView();
                    hav.ShowDialog();
                }
                else
                {
                    CustomerView cv = new CustomerView();
                    cv.ShowDialog();
                }
                this.Show();
            }
        }
        //Button Seller Login
        private void btnSellerLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginRegisterViewModel
                .LoginSeller
                (tbSellerEmailLogin.Text, tbSellerPasswordLogin.Password);
            if (Session.isLogin)
            {
                this.Hide();
                if (Session.role.ToUpper() == "ADMIN")
                {
                    HomeAdminView hav = new HomeAdminView();
                    hav.ShowDialog();
                }
                else
                {
                    //Seller View (Diganti kalau sudah ada View)
                    SellerView sv = new SellerView();
                    sv.ShowDialog();
                }
                this.Show();
            }
        }

        private void btnCustomerToRegister2_click(object sender, RoutedEventArgs e)
        {
            LoginRegisterViewModel.swapPage(LoginRegisterViewModel.CardPage.RegisterSecondPage);
        }

        private void btnCustomerToLogin_click(object sender, RoutedEventArgs e)
        {
            LoginRegisterViewModel.swapPage(LoginRegisterViewModel.CardPage.LoginPage);
        }

        private void btnCustomerToRegister1_click(object sender, RoutedEventArgs e)
        {
            LoginRegisterViewModel.swapPage(LoginRegisterViewModel.CardPage.RegisterFirstPage);
        }

        private void btnCustomerToRegister3_click(object sender, RoutedEventArgs e)
        {
            LoginRegisterViewModel.swapPage(LoginRegisterViewModel.CardPage.RegisterThirdPage);
        }

        private void btnSwap_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void btnSwap_MouseLeave(object sender, MouseEventArgs e)
        {

        }
        private bool validateCustomerRegister()
        {
            bool valid = true;
            if (
                tbCustomerAddressRegister.Text == "" &&
                tbCustomerConfirmPasswordRegister.Password == "" &&
                tbCustomerEmailRegister.Text == "" &&
                tbCustomerFullNameRegister.Text == "" &&
                tbCustomerPasswordRegister.Password == "" &&
                tbCustomerPhoneNumberRegister.Text == ""
                )
                valid = false;

            return valid;
        }
        private void btnCustomerRegister_Click(object sender, RoutedEventArgs e)
        {
            //LoginRegisterViewModel.LoginCustomer(tbSellerEmailLogin.Text, tbSellerPasswordLogin.Password);
            int checkpassword = passwordCustomerCheck();
            // 0 = Masih kosong
            // 1 = Sudah benar
            // -1 = Salah
            if(checkpassword == 0)
            {
                MessageBox.Show("Password masih kosong");
            }
            else if(checkpassword == 1)
            {
                if(LoginRegisterViewModel.RegisterCustomer(tbCustomerEmailRegister.Text, tbCustomerFullNameRegister.Text, dpCustomerBornDateRegister.SelectedDate.Value, tbCustomerAddressRegister.Text, tbCustomerPhoneNumberRegister.Text, tbCustomerPasswordRegister.Password))
                {
                    resetInputRegister();
                    btnCustomerToLogin_click(null, null);
                }
            }
            else if(checkpassword == -1)
            {
                MessageBox.Show("Password dan confirm password tidak cocok");
            }
        }
        private void resetInputLogin()
        {
            tbCustomerEmailLogin.Text = "";
            tbCustomerPasswordLogin.Password = "";
            tbSellerEmailLogin.Text = "";
            tbSellerPasswordLogin.Password = "";
        }
        private void resetInputRegister()
        {
            tbCustomerEmailRegister.Text = "";
            tbCustomerFullNameRegister.Text = "";
            dpCustomerBornDateRegister.SelectedDate = null;
            tbCustomerAddressRegister.Text = "";
            tbCustomerPhoneNumberRegister.Text = "";
            tbCustomerPasswordRegister.Password = "";
            tbCustomerConfirmPasswordRegister.Password = "";
        }
        int passwordCustomerCheck()
        {
            int balek = tbCustomerPasswordRegister.Password == "" ? 0 : tbCustomerPasswordRegister.Password == tbCustomerConfirmPasswordRegister.Password ? 1 : -1;
            return balek;
        }
    }
}
