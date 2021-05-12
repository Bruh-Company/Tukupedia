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
        TransactionViewModel tvm;
        CourierViewModel covm;
        JenisPembayaranViewModel jvm;
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
            reloadTransaction();
            canvasD_Trans.Visibility = Visibility.Hidden;
        }
        void reloadTransaction()
        {
            tvm = new TransactionViewModel();
            dgH_Trans.ItemsSource = tvm.getHtrans().DefaultView;
        }

        private void dgCustomer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgCustomer.SelectedIndex != -1)
            {
                DataRow dr = cvm.selectData(dgCustomer.SelectedIndex);
                if (dr == null) return;
                tbNamaCustomer.Text = dr[2].ToString();
                tbAlamatCustomer.Text = dr[3].ToString();
                tbEmailCustomer.Text = dr[1].ToString();
                tbNotelpCustomer.Text = dr[4].ToString();
                tbLahirCustomer.SelectedDate = DateTime.Parse(dr[5].ToString());
                if (dr[6].ToString() == "Aktif")
                {
                    btBanCustomer.Content = "Ban Customer";
                    btBanCustomer.Style = (Style)Application.Current.Resources["btn-danger"];

                }
                else
                {
                    btBanCustomer.Content = "unBan Customer";
                    btBanCustomer.Style = (Style)Application.Current.Resources["btn-primary"];

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
                if (dr == null) return;
                tbNamaSeller.Text = dr[2].ToString();
                tbAlamatSeller.Text = dr[3].ToString();
                tbEmailSeller.Text = dr[1].ToString();
                tbNotelpSeller.Text = dr[4].ToString();
                if (dr[6].ToString() == "Aktif")
                {
                    btBanSeller.Content = "Ban Seller";
                    btBanSeller.Style = (Style)Application.Current.Resources["btn-danger"];

                }
                else
                {
                    btBanSeller.Content = "unBan Seller";
                    btBanSeller.Style = (Style)Application.Current.Resources["btn-primary"];


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
            svm.update(tbNamaSeller.Text, tbEmailSeller.Text, tbAlamatSeller.Text, tbNotelpSeller.Text, cbisOfficialSeller.SelectedIndex);
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
            if (dr == null) return;
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

        private void dgH_Trans_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgH_Trans.SelectedIndex != -1) {
                canvasD_Trans.Visibility = Visibility.Visible;
                DataRow dr = tvm.selectData(dgH_Trans.SelectedIndex);
                if (dr == null) return;
                lbTanggalTransaksi.Content = dr[4].ToString();
                lbNamaUser.Content = dr[2].ToString();
                DataTable dt = tvm.getDTrans();
                int jumlah = 0, total = 0;
                foreach(DataRow deer in dt.Rows)
                {
                    jumlah += Convert.ToInt32(deer[3].ToString());
                    total += Convert.ToInt32(deer[4].ToString());
                }
                lbJumlah.Content = jumlah.ToString();
                lbTotal.Content = total.ToString();


            }
        }

        private void dgH_Trans_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgH_Trans.SelectedIndex == -1)
            {
                canvasD_Trans.Visibility = Visibility.Hidden;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void btKurir_Click(object sender, RoutedEventArgs e)
        {
            reloadCourier();
            btUpdateCourier.Visibility = Visibility.Hidden;
            btBanCourier.Visibility = Visibility.Hidden;
            btInsertCourier.Visibility = Visibility.Visible;
            resetCourier();
        }
        void reloadCourier()
        {
            covm = new CourierViewModel();
            
            dgCourier.ItemsSource = covm.getDataTable().DefaultView;
        }

        private void btInsertCourier_Click(object sender, RoutedEventArgs e)
        {
            if (covm.insert(tbNamaKurir.Text, tbHargaKurir.Text))
            {
                resetCourier();
                reloadCourier();
            }
        }

        private void btBanCourier_Click(object sender, RoutedEventArgs e)
        {
            covm.delete();
            reloadCourier();
        }

        private void btUpdateCourier_Click(object sender, RoutedEventArgs e)
        {
            covm.update(tbNamaKurir.Text, tbHargaKurir.Text);
            reloadCourier();
            resetCourier();
        }
        void resetCourier()
        {
            tbHargaKurir.Text = "";
            tbNamaKurir.Text = "";
        }

        private void dgCourier_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgCourier.SelectedIndex != -1)
            {
                DataRow dr = covm.selectData(dgCourier.SelectedIndex);
                if (dr == null) return;
                tbNamaKurir.Text = dr[1].ToString();
                tbHargaKurir.Text = dr[2].ToString();
                btBanCourier.Visibility = Visibility.Visible;
                btInsertCourier.Visibility = Visibility.Hidden;
                btUpdateCourier.Visibility = Visibility.Visible;
            }
        }

        private void dgCourier_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if(dgCourier.SelectedIndex == -1)
            {
                btUpdateCourier.Visibility = Visibility.Hidden;
                btBanCourier.Visibility = Visibility.Hidden;
                btInsertCourier.Visibility = Visibility.Visible;
                //resetCourier();
            }
        }

        private void tbHargaKurir_TextChanged(object sender, TextChangedEventArgs e)
        {
            string temp = tbHargaKurir.Text;
            tbHargaKurir.Text = temp.All(char.IsDigit) ? tbHargaKurir.Text : tbHargaKurir.Text.Remove(tbHargaKurir.Text.Length - 1); ;
            //notelp.Text = notelp.Text.Remove(notelp.Text.Length - 1);
        }

        private void btTambahJenisPembayaran_Click(object sender, RoutedEventArgs e)
        {
            if (jvm.insert(tbNamaJenisPembayaran.Text))
            {
                reloadJenisPembayaran();
                tbNamaJenisPembayaran.Text = "";
            }
        }

        private void btUpdateJenisPembayaran_Click(object sender, RoutedEventArgs e)
        {
            jvm.update(tbNamaJenisPembayaran.Text);
            reloadJenisPembayaran();
            tbNamaJenisPembayaran.Text = "";

        }

        private void btToggleJenisPembayaran_Click(object sender, RoutedEventArgs e)
        {
            jvm.delete();
            reloadJenisPembayaran();
            tbNamaJenisPembayaran.Text = "";

        }

        private void dgJenisPembayaran_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(dgJenisPembayaran.SelectedIndex != -1)
            {
                DataRow dr = jvm.selectData(dgJenisPembayaran.SelectedIndex);
                if (dr == null) return;
                tbNamaJenisPembayaran.Text = dr[0].ToString();
                btToggleJenisPembayaran.Visibility = Visibility.Visible;
                btUpdateJenisPembayaran.Visibility = Visibility.Visible;
                btTambahJenisPembayaran.Visibility = Visibility.Hidden;
            }
        }

        private void dgJenisPembayaran_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if(dgJenisPembayaran.SelectedIndex == -1)
            {
                tbNamaJenisPembayaran.Text = "";
                btToggleJenisPembayaran.Visibility = Visibility.Hidden;
                btUpdateJenisPembayaran.Visibility = Visibility.Hidden;
                btTambahJenisPembayaran.Visibility = Visibility.Visible;
            }
        }

        private void btJenisPembayaran_Click(object sender, RoutedEventArgs e)
        {
            reloadJenisPembayaran();
            tbNamaJenisPembayaran.Text = "";
            btToggleJenisPembayaran.Visibility = Visibility.Hidden;
            btUpdateJenisPembayaran.Visibility = Visibility.Hidden;
            btTambahJenisPembayaran.Visibility = Visibility.Visible;
        }
        void reloadJenisPembayaran()
        {
            jvm = new JenisPembayaranViewModel();
            dgJenisPembayaran.ItemsSource = jvm.getDataTable().DefaultView;
        }
    }
}
