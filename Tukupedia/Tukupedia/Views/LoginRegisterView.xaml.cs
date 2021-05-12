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

        private void btnCustomerToLogin_click(object sender, RoutedEventArgs e)
        {
            LoginRegisterViewModel.swapPage(LoginRegisterViewModel.CardPage.LoginPage);
        }

        private void btnCustomerToRegister1_click(object sender, RoutedEventArgs e)
        {
            LoginRegisterViewModel.swapPage(LoginRegisterViewModel.CardPage.RegisterFirstPage);
        }

        private void btnCustomerToRegister2_click(object sender, RoutedEventArgs e)
        {
            bool valid = validateCustomerRegister(sender);
            LoginRegisterViewModel.swapPage(LoginRegisterViewModel.CardPage.RegisterSecondPage);
        }

        private void btnCustomerToRegister3_click(object sender, RoutedEventArgs e)
        {
            bool valid = validateCustomerRegister(sender);
            LoginRegisterViewModel.swapPage(LoginRegisterViewModel.CardPage.RegisterThirdPage);
        }

        private void btnSwap_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void btnSwap_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        //not done
        private bool validateCustomerRegister(object sender)
        {
            bool invalid = false;
            
            if (sender == btnCustomerRegisterNext1)
            {
                invalid |= tbCustomerFullNameRegister.Text.ToString() == "";
                invalid |= tbCustomerEmailRegister.Text.ToString() == "";
            }
            else if (sender == btnCustomerRegisterNext2)
            {
                invalid |= tbCustomerPhoneNumberRegister.Text.ToString() == "";
                //invalid |= dpCustomerBornDateRegister.SelectedDate.Value;
                //invalid |= 
            }
            else if (sender == btnCustomerRegister)
            {

            }

            return !invalid;
        }

        private void btnCustomerRegister_Click(object sender, RoutedEventArgs e)
        {
            bool valid = validateCustomerRegister(sender);

            int checkpassword = passwordCheck(LoginRegisterViewModel.CustomerSellerStage.Customer);
            // 0 = Masih kosong
            // 1 = Sudah benar
            // -1 = Salah
            if(checkpassword == 0)
            {
                MessageBox.Show("Password masih kosong");
            }
            else if(checkpassword == 1)
            {
                bool success = LoginRegisterViewModel.RegisterCustomer(
                    nama    : tbCustomerFullNameRegister.Text,
                    email   : tbCustomerEmailRegister.Text,
                    lahir   : dpCustomerBornDateRegister.SelectedDate.Value,
                    alamat  : tbCustomerAddressRegister.Text,
                    notelp  : tbCustomerPhoneNumberRegister.Text,
                    password: tbCustomerPasswordRegister.Password
                    );

                if (success)
                {
                    resetInputRegister();
                    LoginRegisterViewModel.swapPage(
                        LoginRegisterViewModel.CardPage.LoginPage
                        );
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

        int passwordCheck(LoginRegisterViewModel.CustomerSellerStage stage)
        {
            if (stage == LoginRegisterViewModel.CustomerSellerStage.Customer)
            {
                int balek = tbCustomerPasswordRegister.Password == "" ? 0 : tbCustomerPasswordRegister.Password == tbCustomerConfirmPasswordRegister.Password ? 1 : -1;
                return balek;
            }
            else
            {
                int balek = tbSellerPasswordRegister.Password == "" ? 0 : tbSellerPasswordRegister.Password == tbSellerConfirmPasswordRegister.Password ? 1 : -1;
                return balek;
            }
        }

        private void btnSellerToLogin_click(object sender, RoutedEventArgs e)
        {
            LoginRegisterViewModel.swapPage(LoginRegisterViewModel.CardPage.LoginPage);
        }

        private void btnSellerToRegister1_click(object sender, RoutedEventArgs e)
        {
            LoginRegisterViewModel.swapPage(LoginRegisterViewModel.CardPage.RegisterFirstPage);
        }

        private void btnSellerToRegister2_click(object sender, RoutedEventArgs e)
        {
            LoginRegisterViewModel.swapPage(LoginRegisterViewModel.CardPage.RegisterSecondPage);
        }

        private void btnSellerToRegister3_click(object sender, RoutedEventArgs e)
        {
            LoginRegisterViewModel.swapPage(LoginRegisterViewModel.CardPage.RegisterThirdPage);
        }

        private void btnSellerRegister_Click(object sender, RoutedEventArgs e)
        {
            
            LoginRegisterViewModel.RegisterSeller(
                namaSeller: tbSellerFullNameRegister.Text,
                namaToko: tbSellerShopNameRegister.Text,
                alamat: tbSellerAddressRegister.Text,
                notelp: tbShopPhoneNumberRegister.Text,
                password: tbSellerPasswordRegister.Password,
                email: tbSellerShopEmailRegister.Text,
                nikSeller: tbSellerIdentityNumberRegister.Text
                );
            LoginRegisterViewModel.swapPage(
                        LoginRegisterViewModel.CardPage.LoginPage
                        );
        }
    }
}
