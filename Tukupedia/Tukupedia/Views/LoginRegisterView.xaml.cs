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
            cbMendaftarSebagai_DropDownClosed(null, null);
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


        private void cbMendaftarSebagai_DropDownClosed(object sender, EventArgs e)
        {
            if(cbMendaftarSebagai.SelectedIndex == 0)
            {
                lbTanggalLahir.Visibility = Visibility.Visible;
                dpTanggalLahir.Visibility = Visibility.Visible;
            }
            else
            {
                lbTanggalLahir.Visibility = Visibility.Hidden;
                dpTanggalLahir.Visibility = Visibility.Hidden;
            }
        }
        //Untuk Register (Diganti saat sudah ganti nama Button)
        private void btRegisterRegister_Click(object sender, RoutedEventArgs e)
        {
            if(tbEmailRegister.Text == "" || tbPasswordRegister.Password == "" || tbAlamatRegister.Text == "" || tbNoTelpRegister.Text == "")
            {
                MessageBox.Show("Pastikan anda sudah mengisi semua data :)");
            }
            else if(tbPasswordRegister.Password != tbConfirmRegister.Password)
            {
                MessageBox.Show("Password dan Confirm Password tidak sama :(");
            }
            //0 = user
            //1 = Seller
            if(cbMendaftarSebagai.SelectedIndex == 0)
            {
                if(dpTanggalLahir.SelectedDate == null)
                {
                    MessageBox.Show("Tanggal Lahir tidak boleh kosong");
                }
                else
                {
                    //LoginRegisterViewModel.registerUser("","",DateTime.Now,null,null,null);
                }
            }
            else if(cbMendaftarSebagai.SelectedIndex == 1)
            {

            }
            else
            {
                
            }
            LoginRegisterViewModel.LoginCustomer(tbSellerEmailLogin.Text.ToString(), tbSellerPasswordLogin.Password.ToString());
        }

        private void swapCard(object sender, RoutedEventArgs e)
        {
            LoginRegisterViewModel.swapCard();
        }
        //Button Customer Login
        private void btnCustomerLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginRegisterViewModel
                .LoginCustomer
                (tbCustomerEmailLogin.Text,
                tbCustomerPasswordLogin.Password.ToString());
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
        //Butoon Seller Login
        private void btnSellerLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginRegisterViewModel
                .LoginSeller
                (tbSellerEmailLogin.Text, tbSellerPasswordLogin.Password.ToString());
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
                    CustomerView cv = new CustomerView();
                    cv.ShowDialog();
                }
                this.Show();
            }
        }
    }
}
