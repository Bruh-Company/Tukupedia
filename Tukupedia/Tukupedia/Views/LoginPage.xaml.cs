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
    }
}
