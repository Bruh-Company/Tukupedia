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
        SellerViewModel svm;
        CategoryViewModel cavm;
        public HomeAdminView()
        {
            InitializeComponent();

        }
        void hideall()
        {
            CanvasCategory.Visibility = Visibility.Hidden;
            CanvasrootCustomer.Visibility = Visibility.Hidden;
            CanvasHome.Visibility = Visibility.Hidden;
            CanvasrootSeller.Visibility = Visibility.Hidden;


        }

        private void btHome_Click(object sender, RoutedEventArgs e)
        {
            CanvasHome.Visibility = Visibility.Visible;
            HomeViewModel.initHome();
        }

        private void btCategory_Click(object sender, RoutedEventArgs e)
        {
            reloadCategory();
            btTambahKategori.Visibility = Visibility.Hidden;
            btToggleKategori.Visibility = Visibility.Hidden;
            btUpdateKategori.Visibility = Visibility.Visible;
        }
        void reloadCategory()
        {
            cavm = new CategoryViewModel();
            dgCategory.ItemsSource = cavm.getDataTable().DefaultView;
        }

        private void btCustomer_Click(object sender, RoutedEventArgs e)
        {
            reloadCustomer();
            canvasCustomer.Visibility = Visibility.Hidden;
        }

        private void btSeller_Click(object sender, RoutedEventArgs e)
        {
            reloadSeller();
            canvasSeller.Visibility = Visibility.Hidden;
        }

        private void btTransaction_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dgCustomer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgCustomer.SelectedIndex != -1)
            {
                DataRow dr = cvm.selectData(dgCustomer.SelectedIndex);
                tbNamaCustomer.Text = dr[2].ToString();
                tbAlamatCustomer.Text = dr[3].ToString();
                tbEmailCustomer.Text = dr[1].ToString();
                tbNotelpCustomer.Text = dr[4].ToString();
                tbLahirCustomer.SelectedDate = DateTime.Parse(dr[5].ToString());
                if (dr[6].ToString() == "Aktif")
                {
                    btBanCustomer.Content = "Ban Customer";
                    btBanCustomer.Background = Brushes.DarkRed; // Color.FromRgb(103, 58, 183);
                    btBanCustomer.BorderBrush = Brushes.DarkRed;
                }
                else
                {
                    btBanCustomer.Content = "unBan Customer";
                    btBanCustomer.Background = Brushes.MediumPurple; // Color.FromRgb(103, 58, 183);
                    btBanCustomer.BorderBrush = Brushes.MediumPurple;

                }

                canvasCustomer.Visibility = Visibility.Visible;

            }
        }

        void resetInput()
        {
            tbNamaCustomer.Text = "";
            tbEmailCustomer.Text = "";
            tbAlamatCustomer.Text = "";
            tbNotelpCustomer.Text = "";
            tbLahirCustomer.SelectedDate = null;

            tbNamaSeller.Text = "";
            tbEmailSeller.Text = "";
            tbAlamatSeller.Text = "";
            tbNotelpSeller.Text = "";
            tbLahirSeller.SelectedDate = null;
            cbisOfficialSeller.SelectedIndex = -1;
        }
        void reloadCustomer()
        {
            cvm = new CustomerViewModel();
            dgCustomer.ItemsSource = cvm.getDataTable().DefaultView;

        }
        void reloadSeller()
        {
            svm = new SellerViewModel();
            dgSeller.ItemsSource = svm.getDataTable().DefaultView;
        }
        private void btUpdateCustomer_Click(object sender, RoutedEventArgs e)
        {
            cvm.update(tbNamaCustomer.Text, tbEmailCustomer.Text, tbAlamatCustomer.Text, tbNotelpCustomer.Text, tbLahirCustomer.SelectedDate.Value);
            resetInput();
            reloadCustomer();
        }

        private void btBanCustomer_Click(object sender, RoutedEventArgs e)
        {
            cvm.ban();
            resetInput();
            reloadCustomer();
        }

        private void dgCustomer_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgCustomer.SelectedIndex == -1)
            {
                canvasCustomer.Visibility = Visibility.Hidden;
            }
            else
            {

            }
        }

        private void dgSeller_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgSeller.SelectedIndex != -1)
            {
                DataRow dr = svm.selectData(dgSeller.SelectedIndex);
                tbNamaSeller.Text = dr[2].ToString();
                tbAlamatSeller.Text = dr[3].ToString();
                tbEmailSeller.Text = dr[1].ToString();
                tbNotelpSeller.Text = dr[4].ToString();
                tbLahirSeller.SelectedDate = DateTime.Parse(dr[5].ToString());
                if (dr[6].ToString() == "Aktif")
                {
                    btBanSeller.Content = "Ban Seller";
                    btBanSeller.Background = Brushes.DarkRed; // Color.FromRgb(103, 58, 183);
                    btBanSeller.BorderBrush = Brushes.DarkRed;
                }
                else
                {
                    btBanSeller.Content = "unBan Seller";
                    btBanSeller.Background = Brushes.MediumPurple; // Color.FromRgb(103, 58, 183);
                    btBanSeller.BorderBrush = Brushes.MediumPurple;

                }
                if (dr[7].ToString() == "Yes")
                {
                    cbisOfficialSeller.SelectedIndex = 1;
                }
                else
                {
                    cbisOfficialSeller.SelectedIndex = 0;
                }

                canvasSeller.Visibility = Visibility.Visible;

            }
        }

        private void dgSeller_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgSeller.SelectedIndex == -1)
            {
                canvasSeller.Visibility = Visibility.Hidden;
            }
            else
            {

            }
        }

        private void btUpdateSeller_Click(object sender, RoutedEventArgs e)
        {
            svm.update(tbNamaSeller.Text, tbEmailSeller.Text, tbAlamatSeller.Text, tbNotelpSeller.Text, tbLahirSeller.SelectedDate.Value, cbisOfficialSeller.SelectedIndex);
            resetInput();
            reloadSeller();
        }

        private void btBanSeller_Click(object sender, RoutedEventArgs e)
        {
            svm.ban();
            resetInput();
            reloadSeller();
        }

        private void dgCategory_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRow dr = cavm.selectData(dgCategory.SelectedIndex);
            btTambahKategori.Visibility = Visibility.Hidden;
            btToggleKategori.Visibility = Visibility.Visible;
            btUpdateKategori.Visibility = Visibility.Visible;
            tbNamaKategori.Text = dr[1].ToString();
        }

        private void btTambahKategori_Click(object sender, RoutedEventArgs e)
        {
            if (cavm.insert(tbNamaKategori.Text))
            {
                tbNamaKategori.Text = "";
                reloadCategory();
            }
        }

        private void btUpdateKategori_Click(object sender, RoutedEventArgs e)
        {
            cavm.update(tbNamaKategori.Text);
            tbNamaKategori.Text = "";
            reloadCategory();
        }

        private void btToogleKategori_Click(object sender, RoutedEventArgs e)
        {
            cavm.delete();
        }

        private void dgCategory_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgCategory.SelectedIndex == -1)
            {
                btTambahKategori.Visibility = Visibility.Visible;
                btToggleKategori.Visibility = Visibility.Hidden;
                btUpdateKategori.Visibility = Visibility.Hidden;
            }
        }
    }
}
