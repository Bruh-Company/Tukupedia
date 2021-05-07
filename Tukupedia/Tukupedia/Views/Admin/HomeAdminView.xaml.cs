using System;
using System.Collections.Generic;
using System.Data;
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
using Tukupedia.ViewModels.Admin;

namespace Tukupedia.Views.Admin
{
    /// <summary>
    /// Interaction logic for HomeAdminView.xaml
    /// </summary>
    public partial class HomeAdminView : Window
    {
        DataTable dtCustomer, dtSeller;
        public HomeAdminView()
        {
            InitializeComponent();

        }
        void hideall()
        {
            CanvasCategory.Visibility = Visibility.Hidden;
            CanvasCustomer.Visibility = Visibility.Hidden;
            CanvasHome.Visibility = Visibility.Hidden;
            CanvasSeller.Visibility = Visibility.Hidden;


        }

        private void btHome_Click(object sender, RoutedEventArgs e)
        {
            CanvasHome.Visibility = Visibility.Visible;
            HomeViewModel.initHome();
        }

        private void btCategory_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btCustomer_Click(object sender, RoutedEventArgs e)
        {
            dtCustomer = null;
            CanvasCustomer.Visibility = Visibility.Visible;
            dtCustomer = CustomerViewModel.initCustomer();
            dgCustomer.ItemsSource = dtCustomer.DefaultView;
        }

        private void btSeller_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btTransaction_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
