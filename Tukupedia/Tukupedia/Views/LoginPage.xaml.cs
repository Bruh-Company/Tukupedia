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
    public partial class LoginPage : Window
    {
        public LoginPage()
        {
            InitializeComponent();
            cbMendaftarSebagai_DropDownClosed(null, null);
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
            if (LoginRegisterViewModel.login(tbEmailLogin.Text.ToString(), tbPasswordLogin.Password.ToString()))
            {
                tbEmailLogin.Text = "";
                tbPasswordLogin.Password = "";
            }
            else
            {
                tbPasswordLogin.Password = "";
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
                    ViewModels.LoginRegisterViewModel.registeruser(tbEmailRegister.Text)
                }
            }
            else if(cbMendaftarSebagai.SelectedIndex == 1)
            {

            }
            else
            {
                
            }
            if (LoginRegisterViewModel.login(tbEmailLogin.Text.ToString(), tbPasswordLogin.Password.ToString()))
            {
                tbEmailLogin.Text = "";
                tbPasswordLogin.Password = "";
            }
            else
            {
                tbPasswordLogin.Password = "";
            }
        }
    }
}
