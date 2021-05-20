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
using Tukupedia.Models;

namespace Tukupedia.Views
{
    /// <summary>
    /// Interaction logic for ChangePasswordView.xaml
    /// </summary>
    public partial class ChangePasswordView : Window
    {
        PasswordModel pm;
        public ChangePasswordView(string tabel, string id)
        {
            pm = new PasswordModel(tabel, id);
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            if(pm.changePassword(tbPasswordLama.Password, tbPasswordBaru.Password, tbConfirmPassword.Password))
            {
                this.Close();
            }
        }
    }
}
