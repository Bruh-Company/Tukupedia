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
using System.Text.RegularExpressions;

namespace Tukupedia.Views
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginRegisterView : Window
    {
        EmptyAnimation ea;

        public LoginRegisterView()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            LoginRegisterViewModel.InitializeView(this);
            loadInit();

            ea = new EmptyAnimation(100);

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

        private void btnToLogin_click(object sender, RoutedEventArgs e)
        {
            LoginRegisterViewModel.swapPage(LoginRegisterViewModel.CardPage.LoginPage);
        }

        private void btnToRegister1_click(object sender, RoutedEventArgs e)
        {
            LoginRegisterViewModel.swapPage(LoginRegisterViewModel.CardPage.RegisterFirstPage);
        }

        private void btnToRegister2_click(object sender, RoutedEventArgs e)
        {
            List<FrameworkElement> comp = validateRegister(sender);

            if (comp.Count > 0)
            {
                foreach (Control nn in comp)
                {
                    nn.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                }
                ea.makeAnimation(comp);
                ea.playAnim();

                return;
            }

            LoginRegisterViewModel.swapPage(LoginRegisterViewModel.CardPage.RegisterSecondPage);
        }

        private void btnToRegister3_click(object sender, RoutedEventArgs e)
        {
            List<FrameworkElement> comp = validateRegister(sender);

            if (comp.Count > 0)
            {
                foreach (FrameworkElement nn in comp)
                {
                    if (nn is Control)
                    {
                        ((Control)nn).BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                    }
                }
                ea.makeAnimation(comp);
                ea.playAnim();

                return;
            }

            LoginRegisterViewModel.swapPage(LoginRegisterViewModel.CardPage.RegisterThirdPage);
        }

        private void btnSwap_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void btnSwap_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        //not done
        private List<FrameworkElement> validateRegister(object sender)
        {
            List<FrameworkElement> FE = new List<FrameworkElement>();
            
            if (sender == btnCustomerRegisterNext1)
            {
                if (tbCustomerFullNameRegister.Text.ToString() == "")
                    FE.Add(tbCustomerFullNameRegister);
                if (tbCustomerEmailRegister.Text.ToString() == "")
                    FE.Add(tbCustomerEmailRegister);
            }
            else if (sender == btnCustomerRegisterNext2)
            {
                if (tbCustomerPhoneNumberRegister.Text.ToString() == "")
                    FE.Add(tbCustomerPhoneNumberRegister);
                if (dpCustomerBornDateRegister.SelectedDate.ToString() == "")
                    FE.Add(dpCustomerBornDateRegister);
                if (tbCustomerAddressRegister.Text.ToString() == "") 
                    FE.Add(tbCustomerAddressRegister);
            }
            else if (sender == btnCustomerRegister)
            {
                if (!validatePasswordAndConfirmPassword(LoginRegisterViewModel.CustomerSellerStage.Customer))
                    FE.Add(tbCustomerConfirmPasswordRegister);
                if (!stronkPassword(tbCustomerPasswordRegister.Password.ToString()))
                    FE.Add(tbCustomerPasswordRegister);
            }
            else if (sender == btnSellerRegisterNext1)
            {
                if (tbSellerShopEmailRegister.Text.ToString() == "")
                    FE.Add(tbSellerShopEmailRegister);
                if (tbSellerShopNameRegister.Text.ToString() == "")
                    FE.Add(tbSellerShopEmailRegister);
                if (tbShopPhoneNumberRegister.Text.ToString() == "")
                    FE.Add(tbShopPhoneNumberRegister);
            }
            else if (sender == btnSellerRegisterNext2)
            {
                if (tbSellerIdentityNumberRegister.Text.ToString() == "")
                    FE.Add(tbSellerIdentityNumberRegister);
                if (tbSellerFullNameRegister.Text.ToString() == "")
                    FE.Add(tbSellerFullNameRegister);
                if (tbSellerAddressRegister.Text.ToString() == "")
                    FE.Add(tbSellerAddressRegister);
            }
            else if (sender == btnSellerRegister)
            {
                if (!validatePasswordAndConfirmPassword(LoginRegisterViewModel.CustomerSellerStage.Seller))
                    FE.Add(tbSellerConfirmPasswordRegister);
                if (!stronkPassword(tbSellerPasswordRegister.Password.ToString()))
                    FE.Add(tbSellerPasswordRegister);
            }

            return FE;
        }

        private bool validatePasswordAndConfirmPassword(LoginRegisterViewModel.CustomerSellerStage e)
        {
            string pass, conpass;
            if (e == LoginRegisterViewModel.CustomerSellerStage.Customer)
            {
                pass = tbCustomerPasswordRegister.Password;
                conpass = tbCustomerConfirmPasswordRegister.Password;
            }
            else
            {
                pass = tbSellerPasswordRegister.Password;
                conpass = tbSellerConfirmPasswordRegister.Password;
            }
            return pass == conpass;
        }

        //private bool validatePassword(object sender)
        //{

        //}

        private void btnCustomerRegister_Click(object sender, RoutedEventArgs e)
        {
            List<FrameworkElement> comp = validateRegister(sender);

            if (comp.Count > 0)
            {
                foreach (FrameworkElement nn in comp)
                {
                    if (nn is Control)
                    {
                        ((Control)nn).BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                    }
                }
                ea.makeAnimation(comp);
                ea.playAnim();

                lblCustomerPasswordCommenter.Background = new SolidColorBrush(Color.FromArgb(127, 255, 0, 0));
                lblCustomerPasswordCommenter.Content = "GACOCOK BANGSAT!!!!!";
                lblCustomerPasswordCommenter.Opacity = 100;

                return;
            }

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

        private void dataChanged(object sender, EventArgs e)
        {
            ((Control)sender).BorderBrush = new SolidColorBrush(Color.FromArgb(138, 255, 255, 255));
        }

        private bool stronkPassword(string pass)
        {
            bool valid = true;
            valid &= pass.Length >= 8;

            Regex rgx = new Regex(@"^(?=.*[a-zA-Z])(?=.*[0-9])");
            valid &= rgx.Matches(pass).Count > 0;

            return valid;
        }

        private void validatePassword(object sender, RoutedEventArgs e)
        {
            string pass = tbCustomerPasswordRegister.Password;
            if (!stronkPassword(pass))
            {
                lblCustomerPasswordCommenter.Background = new SolidColorBrush(Color.FromArgb(127, 255, 0, 0));
                lblCustomerPasswordCommenter.Content = "password kok lemah amat";
                lblCustomerPasswordCommenter.Opacity = 100;
                return;
            }

            passwordChanged(sender, e);
        }

        private void passwordChanged(object sender, RoutedEventArgs e)
        {
            dataChanged(sender, e);

            lblCustomerPasswordCommenter.Background = new SolidColorBrush(Color.FromArgb(127, 0, 255, 0));
            lblCustomerPasswordCommenter.Content = "Hidup anda tenang";
        }
    }
}