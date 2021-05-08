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
        CustomerViewModel cvm;
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
            cvm = new CustomerViewModel();
            dgCustomer.ItemsSource = cvm.getDataTable().DefaultView;
        }

        private void btSeller_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btTransaction_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dgCustomer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(dgCustomer.SelectedIndex != -1)
            {
                DataRow dr = cvm.selectData(dgCustomer.SelectedIndex);
                tbNamaCustomer.Text = dr[2].ToString();
                tbAlamatCustomer.Text = dr[3].ToString();
                tbEmailCustomer.Text = dr[1].ToString();
                tbNotelpCustomer.Text = dr[4].ToString();
                tbLahirCustomer.SelectedDate = DateTime.Parse(dr[5].ToString());

            }
        }

        void resetInput()
        {
            tbNamaCustomer.Text = "";
            tbEmailCustomer.Text = "";
            tbAlamatCustomer.Text = "";
            tbNotelpCustomer.Text = "";
            tbLahirCustomer.SelectedDate = null;
        }
        private void btUpdateCustomer_Click(object sender, RoutedEventArgs e)
        {
            cvm.update(tbNamaCustomer.Text, tbEmailCustomer.Text, tbAlamatCustomer.Text, tbNotelpCustomer.Text, tbLahirCustomer.SelectedDate.Value);
            resetInput();
        }
    }
}
