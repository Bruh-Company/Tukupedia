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
            loadInit();

            LoginRegisterViewModel.InitializeView(this);

            Panel.SetZIndex(CardCustomer, 1);
            Panel.SetZIndex(CardSeller, 0);
        }

        private void loadInit()
        {
            imgLogo.Source =
                new BitmapImage(new Uri(
                    AppDomain.CurrentDomain.BaseDirectory +
                    "Resource\\Logo\\TukupediaLogo.png"));

            CardSeller.Margin = new Thickness(0, 100, 0, -100);
            CardSeller.Opacity = 0;

            CardCustomer.Margin = new Thickness(0, 0, 0, 0);
            CardCustomer.Opacity = 1;
        }

        private void BtRegisterLogin_Click(object sender, RoutedEventArgs e)
        {
            

        }

        public void closeThis()
        {
            this.Close();
        }

        private void BtLoginRegister_Click(object sender, RoutedEventArgs e)
        {
            
            
        }

        private void btLoginLogin_Click(object sender, RoutedEventArgs e)
        {
            if (LoginRegisterViewModel.LoginCustomer(tbCustomerUnameLogin.Text.ToString(), tbCustomerPasswordLogin.Password.ToString()))
            {
                tbCustomerUnameLogin.Text = "";
                tbCustomerPasswordLogin.Password = "";
            }
            else
            {
                tbCustomerPasswordLogin.Password = "";
            }
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
            if (LoginRegisterViewModel.LoginCustomer(tbSellerEmailLogin.Text.ToString(), tbSellerPasswordLogin.Password.ToString()))
            {
                tbSellerEmailLogin.Text = "";
                tbSellerPasswordLogin.Password = "";
            }
            else
            {
                tbSellerPasswordLogin.Password = "";
            }
        }

        private void swapCard(object sender, RoutedEventArgs e)
        {
            LoginRegisterViewModel.swapCard();
        }
    }
}
