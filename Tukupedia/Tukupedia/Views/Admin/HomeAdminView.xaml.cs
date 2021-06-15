using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Tukupedia.Helpers.Utils;
using Tukupedia.Report;
using Tukupedia.ViewModels;
using Tukupedia.ViewModels.Admin;

namespace Tukupedia.Views.Admin
{
    /// <summary>
    /// Interaction logic for HomeAdminView.xaml
    /// </summary>
    public partial class HomeAdminView : Window
    {
        private delegate void swapCallback();

        CustomerViewModel cvm;
        SellerViewModel svm;
        CategoryViewModel cavm;
        TransactionViewModel tvm;
        CourierViewModel covm;
        JenisPembayaranViewModel jvm;
        PromoViewModel pvm;
        JenisPromoViewModel jpvm;
        OfficialStoreViewModel osvm;
        HomeViewModel hvm;
        Canvas[] canvas;

        private const int transFPS = 100;
        private const double speedMargin = 0.3;
        private const double speedOpacity = 0.4;
        private const double multiplier = 80;

        private static Transition transition;
        public HomeAdminView()
        {
            InitializeComponent();
            transition = new Transition(transFPS);
            
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            parent.Height = 638;
            parent.Width = 1274;
            initState();
            hvm = new HomeViewModel();
        }
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        private void populateListBox(ListBox lb, List<CheckBox> lcb)
        {
            lb.Items.Clear();
            foreach (CheckBox cb in lcb)
            {
                lb.Items.Add(cb);
            }
        }
        private void initCReport()
        {
            cbSemuaJenisPembayaran.IsChecked = true;
            cbSemuaKategori.IsChecked = true;
            cbSemuaKurir.IsChecked = true;
            cbSemuaPromo.IsChecked = true;
            populateListBox(lbJenisPembayaran, hvm.getJenisPembayaran());
            populateListBox(lbKurir, hvm.getKurir());
            populateListBox(lbJenisKategori, hvm.getKategori());
            populateListBox(lbJenisPromo, hvm.getPromo());
            lbJenisPembayaran.Visibility = Visibility.Hidden;
            lbKurir.Visibility = Visibility.Hidden;
            lbJenisKategori.Visibility = Visibility.Hidden;
            lbJenisPromo.Visibility = Visibility.Hidden;
            dpTanggalAwalReport.SelectedDate = DateTime.Now.AddDays(-7);
            dpTanggalAkhirReport.SelectedDate = DateTime.Now;
            cbisOfficial.SelectedIndex = 0;
        }
        private List<int> getSelected(ListBox lb)
        {
            List<int> selected = new List<int>();
            for (int i = 0; i < lb.Items.Count; i++)
            {
                var item = lb.Items[i];
                CheckBox cb = (CheckBox)item;
                if(cb.IsChecked.Value)
                {
                    selected.Add(i);
                }
            }
            
            return selected;
        }
        private void setChecked(ListBox lb,bool check)
        {
            for (int i = 0; i < lb.Items.Count; i++)
            {
                var item = lb.Items[i];
                CheckBox cb = (CheckBox)item;
                cb.IsChecked = check;
            }
        }
        private void initReport()
        {
            initCReport();
            //ReportAdmin cr = new ReportAdmin();
            //cr.SetDatabaseLogon(App.username, App.password, App.datasource, "");
            //reportViewer.ViewerCore.ReportSource = cr;
            //List<KeyValuePair<string, int>> k = new List<KeyValuePair<string, int>>();
            //k.Add(new KeyValuePair<string, int>("Mei", 20));
            //k.Add(new KeyValuePair<string, int>("Juni", 50));
            //k.Add(new KeyValuePair<string, int>("Juli", 100));
            //TransaksiperHari.ItemsSource = k;
            List<string> s = new List<string>();
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Jumlah Transaksi",
                    Values = hvm.getJumlahTransaksi(),
                    DataLabels = true
                }
                //,
                //new LineSeries
                //{
                //    Title = "Series 2",
                //    Values = new ChartValues<double> { 6, 7, 3, 4 ,6 },
                //    PointGeometry = null
                //},
                //new LineSeries
                //{
                //    Title = "Series 3",
                //    Values = new ChartValues<double> { 4,2,7,2,7 },
                //    PointGeometry = DefaultGeometries.Square,
                //    PointGeometrySize = 15
                //}
            };

            //Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            Labels = hvm.getLabelJumlahTransaksi().ToArray();
            //YFormatter = value => value.ToString("C");

            //modifying the series collection will animate and update the chart
            //SeriesCollection.Add(new LineSeries
            //{
            //    Title = "Series 4",
            //    Values = new ChartValues<double> { 5, 3, 2, 4 },
            //    LineSmoothness = 0, //0: straight lines, 1: really smooth lines
            //    PointGeometry = Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
            //    PointGeometrySize = 50,
            //    PointForeground = Brushes.Gray
            //});

            //modifying any series values will also animate and update the chart
            //SeriesCollection[3].Values.Add(5d);

            DataContext = this;
            chartJumlahTransaksi.Update(true, true);
        }
        void updateChart()
        {
            SeriesCollection[0].Values = null;
            SeriesCollection[0].Values = hvm.getJumlahTransaksi();
            Labels = null;
            Labels = hvm.getLabelJumlahTransaksi().ToArray();
            DataContext = Labels;
            DataContext = this;
        }

        void initState()
        {
            dpTanggalAwal.SelectedDate = DateTime.Now.AddDays(-7);
            dpTanggalAkhir.SelectedDate = DateTime.Now;
            hvm = new HomeViewModel();

            initReport();
            canvas = new Canvas[] { CanvasrootHome, CanvasrootCategory, CanvasrootCustomer, CanvasrootSeller, CanvasrootTransaction, CanvasrootCourier, CanvasrootJenisPembayaran, CanvasrootPromo, CanvasrootOfficialStore};
            CanvasrootHome.Margin = MarginPosition.Middle;
            CanvasrootCategory.Margin = MarginPosition.Right;//
            CanvasrootCustomer.Margin = MarginPosition.Right;
            CanvasrootSeller.Margin = MarginPosition.Right;
            CanvasrootJenisPembayaran.Margin = MarginPosition.Right;//
            CanvasrootPromo.Margin = MarginPosition.Right;
            CanvasrootCourier.Margin = MarginPosition.Right;
            CanvasrootTransaction.Margin = MarginPosition.Right;
            CanvasrootOfficialStore.Margin = MarginPosition.Right;

            ComponentHelper.changeVisibilityComponent(CanvasrootHome, Visibility.Visible);
            ComponentHelper.changeVisibilityComponent(CanvasrootCategory, Visibility.Hidden);
            ComponentHelper.changeVisibilityComponent(CanvasrootCustomer, Visibility.Hidden);
            ComponentHelper.changeVisibilityComponent(CanvasrootSeller, Visibility.Hidden);
            ComponentHelper.changeVisibilityComponent(CanvasrootJenisPembayaran, Visibility.Hidden);

            ComponentHelper.changeVisibilityComponent(CanvasrootPromo, Visibility.Hidden);
            ComponentHelper.changeVisibilityComponent(CanvasrootCourier, Visibility.Hidden);
            ComponentHelper.changeVisibilityComponent(CanvasrootTransaction, Visibility.Hidden);
            ComponentHelper.changeVisibilityComponent(CanvasrootOfficialStore, Visibility.Hidden);

            SwapJenisPromo(0);
            swapPromoCallback(0)();

        }

        private void SwapJenisPromo(int pos)
        {
            
            if(pos == 0)
            {
                setTransform(CanvasJenisPromo, Visibility.Hidden, MarginPosition.Left);
                setTransform(CanvasPromo, Visibility.Hidden, MarginPosition.Right);
                transition.setCallback(swapPromoCallback(pos));
                transition.playTransition();
                ComponentHelper.changeZIndexComponent(CanvasJenisPromo, Visibility.Hidden);
                ComponentHelper.changeZIndexComponent(CanvasPromo, Visibility.Hidden);
            }
            if (pos == 1)
            {
                setTransform(CanvasJenisPromo, Visibility.Visible, MarginPosition.Middle);
                setTransform(CanvasPromo, Visibility.Hidden, MarginPosition.Right);
                transition.setCallback(swapPromoCallback(pos));
                transition.playTransition();
                ComponentHelper.changeZIndexComponent(CanvasJenisPromo, Visibility.Visible);
                ComponentHelper.changeZIndexComponent(CanvasPromo, Visibility.Hidden);

            }
            if (pos == 2)
            {
                setTransform(CanvasJenisPromo, Visibility.Hidden, MarginPosition.Left);
                setTransform(CanvasPromo, Visibility.Visible, MarginPosition.Middle);
                transition.setCallback(swapPromoCallback(pos));
                transition.playTransition();
                ComponentHelper.changeZIndexComponent(CanvasJenisPromo, Visibility.Hidden);
                ComponentHelper.changeZIndexComponent(CanvasPromo, Visibility.Visible);

            }
        }

        private Action swapPromoCallback(int i)
        {
            if (i == 1)
            {
                void fun() {
                    ComponentHelper.changeVisibilityComponent(CanvasJenisPromo, Visibility.Visible);
                    ComponentHelper.changeVisibilityComponent(CanvasPromo, Visibility.Hidden);
                }
                return fun;
            }
            else if (i == 2)
            {
                void fun() {
                    ComponentHelper.changeVisibilityComponent(CanvasJenisPromo, Visibility.Hidden);
                    ComponentHelper.changeVisibilityComponent(CanvasPromo, Visibility.Visible);
                }
                return fun;
            }
            else
            {
                void fun() {
                    ComponentHelper.changeVisibilityComponent(CanvasJenisPromo, Visibility.Hidden);
                    ComponentHelper.changeVisibilityComponent(CanvasPromo, Visibility.Hidden);
                }
                return fun;
            }
        }

        public void setCanvas(int pos)
        {
            for(int i = 0; i< canvas.Length; i++)
            {
                if(i == pos)
                {
                    setTransform(canvas[i], Visibility.Visible, MarginPosition.Middle);
                }
                else if(i<pos)
                {
                    setTransform(canvas[i], Visibility.Hidden, MarginPosition.Left);
                }
                else
                {
                    setTransform(canvas[i], Visibility.Hidden, MarginPosition.Right);
                }
            }
            transition.playTransition();

            for(int i = 0; i<canvas.Length; i++)
            {
                if(i == pos)
                {
                    ComponentHelper.changeZIndexComponent(canvas[i],Visibility.Visible);
                }
                else
                {
                    ComponentHelper.changeZIndexComponent(canvas[i],Visibility.Hidden);
                }
            }
        }
        private static void setTransform(FrameworkElement fe, Visibility v, Thickness m)
        {
            if (m == null) m = MarginPosition.Middle;
            if (v == Visibility.Visible)
            {
                transition.makeTransition(fe,
                    m, 1,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");
            }
            else
            {
                transition.makeTransition(fe,
                    m, 0,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");
            }
        }

        private void btHome_Click(object sender, RoutedEventArgs e)
        {
            setCanvas(0);
            hvm = new HomeViewModel();
            initReport();
        }

        private void btCategory_Click(object sender, RoutedEventArgs e)
        {
            setCanvas(1);
            reloadCategory();
            tbNamaKategori.Text = "";
            btTambahKategori.Visibility = Visibility.Visible;
            btToggleKategori.Visibility = Visibility.Hidden;
            btUpdateKategori.Visibility = Visibility.Hidden;
            cancelCategory.Visibility = Visibility.Hidden;
        }
        void reloadCategory()
        {
            cavm = new CategoryViewModel();
            dgCategory.ItemsSource = cavm.getDataTable().DefaultView;
            tbNamaKategori.Text = "";
        }

        private void btCustomer_Click(object sender, RoutedEventArgs e)
        {
            setCanvas(2);
            reloadCustomer();
            canvasCustomer.Visibility = Visibility.Hidden;
        }

        private void btSeller_Click(object sender, RoutedEventArgs e)
        {
            setCanvas(3);
            reloadSeller();
            canvasSeller.Visibility = Visibility.Hidden;
        }

        private void btTransaction_Click(object sender, RoutedEventArgs e)
        {
            setCanvas(4);
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
                tbLahirCustomer.SelectedDate = DateTime.ParseExact(dr[5].ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture);
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
            cancelCategory.Visibility = Visibility.Visible;
            tbNamaKategori.Text = dr[1].ToString();
            if (dr[2].ToString() == "Aktif") btToggleKategori.Content = "Matikan";
            else btToggleKategori.Content = "Nyalakan";
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
            reloadCategory();
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
                lbTanggalTransaksi.Content = dr[2].ToString();
                lbNamaUser.Content = dr[1].ToString();
                DataTable dt = tvm.getDTrans();
                int jumlah = 0, total = 0;
                foreach(DataRow deer in dt.Rows)
                {
                    jumlah += Convert.ToInt32(deer[3].ToString());
                    total += Convert.ToInt32(deer[4].ToString());
                }
                lbJumlah.Content = jumlah.ToString();
                lbTotal.Content = Utility.formatMoney(Convert.ToInt32(total.ToString()));
                Utility.toCurrency(dt, 2);
                Utility.toCurrency(dt, 4);
                dgD_Trans.ItemsSource = dt.DefaultView;


            }
        }

        private void dgH_Trans_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgH_Trans.SelectedIndex == -1)
            {
                canvasD_Trans.Visibility = Visibility.Hidden;
            }
        }

        

        private void btKurir_Click(object sender, RoutedEventArgs e)
        {
            setCanvas(5);
            reloadCourier();
            
        }
        void reloadCourier()
        {
            covm = new CourierViewModel();
            
            dgCourier.ItemsSource = covm.getDataTable().DefaultView;
            btUpdateCourier.Visibility = Visibility.Hidden;
            btBanCourier.Visibility = Visibility.Hidden;
            btInsertCourier.Visibility = Visibility.Visible;
            cancelCourier.Visibility = Visibility.Hidden;
            resetCourier();
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
                tbHargaKurir.Text = Utility.toNumber(dr[2].ToString());
                btBanCourier.Visibility = Visibility.Visible;
                btInsertCourier.Visibility = Visibility.Hidden;
                btUpdateCourier.Visibility = Visibility.Visible;
                cancelCourier.Visibility = Visibility.Visible;
                if (dr[3].ToString() == "Aktif")
                {
                    btBanCourier.Content = "Ban Kurir";
                }
                else btBanCourier.Content = "Aktifkan Kurir";
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
            tbHargaKurir.Text = temp.All(char.IsDigit) ? tbHargaKurir.Text : tbHargaKurir.Text.Remove(tbHargaKurir.Text.Length - 1);
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
                cancelJenisPembayaran.Visibility = Visibility.Visible;
                if (dr[1].ToString() == "Aktif")
                {
                    btToggleJenisPembayaran.Content = "Matikan";
                }
                else btToggleJenisPembayaran.Content = "Aktifkan";

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
            setCanvas(6);
            reloadJenisPembayaran();
            tbNamaJenisPembayaran.Text = "";
            btToggleJenisPembayaran.Visibility = Visibility.Hidden;
            btUpdateJenisPembayaran.Visibility = Visibility.Hidden;
            btTambahJenisPembayaran.Visibility = Visibility.Visible;
            cancelJenisPembayaran.Visibility = Visibility.Hidden;
        }
        void reloadJenisPembayaran()
        {
            jvm = new JenisPembayaranViewModel();
            dgJenisPembayaran.ItemsSource = jvm.getDataTable().DefaultView;
        }

        private void btPromoroot_Click(object sender, RoutedEventArgs e)
        {
            SwapJenisPromo(0);
            setCanvas(7);
            reloadPromo();
        }
        void reloadPromo()
        {
            pvm = new PromoViewModel();
            dgPromo.ItemsSource = pvm.getDataTable().DefaultView;
            cbJenisPromo.ItemsSource = pvm.getForCb().DefaultView;
            cbJenisPromo.DisplayMemberPath = "NAMA";
            cbJenisPromo.SelectedValuePath = "ID";
        }
        private void btPromo_Click(object sender, RoutedEventArgs e)
        {
            SwapJenisPromo(2);

            reloadPromo();
            btTambahPromo.Visibility = Visibility.Visible;
            btUpdatePromo.Visibility = Visibility.Hidden;
            btHapusPromo.Visibility = Visibility.Hidden;
            cancelPromo.Visibility = Visibility.Hidden;
            resetPromo();
        }
        void resetPromo()
        {
            tbKodePromo.Text = "";
            tbPotongan.Text = "";
            tbPotonganMax.Text = "";
            tbHargaMin.Text = "";
            cbJenisPotongan.SelectedIndex = -1;
            cbJenisPromo.SelectedIndex = -1;
            dpAkhirPromo.SelectedDate = null;
            dpAwalPromo.SelectedDate = null;
        }

        private void dgPromo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(dgPromo.SelectedIndex != -1)
            {
                DataRow dr = pvm.selectData(dgPromo.SelectedIndex);
                if (dr == null) return;
                tbKodePromo.Text = dr["Kode"].ToString();
                tbPotongan.Text = dr["Potongan"].ToString();
                tbPotonganMax.Text = dr["Potongan Maximal"].ToString();
                tbHargaMin.Text = dr["Harga Minimal"].ToString();
                // 0 = Discount
                // 1 = Cashback
                cbJenisPotongan.SelectedIndex = dr["Jenis Potongan"].ToString() == "Persenan" ? 0 : 1;
                int konter = 0;
                foreach (DataRow dr1 in pvm.getForCb().Rows)
                {
                    if (dr["Nama Promo"].ToString() == dr1[1].ToString())
                    {
                        cbJenisPromo.SelectedIndex = konter;
                        break;
                        //MessageBox.Show(dr1[0].ToString());

                    }
                    konter++;
                }
                DataRow drdate = pvm.getMasaBerlaku();
                dpAwalPromo.SelectedDate = DateTime.ParseExact(drdate[0].ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                dpAkhirPromo.SelectedDate = DateTime.ParseExact(drdate[1].ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                btTambahPromo.Visibility = Visibility.Hidden;
                btUpdatePromo.Visibility = Visibility.Visible;
                btHapusPromo.Visibility = Visibility.Visible;
                cancelPromo.Visibility = Visibility.Visible;
                if (dr["Status"].ToString() == "Aktif")
                {
                    btHapusPromo.Content = "Matikan";
                } 
                else btHapusPromo.Content = "Nyalakan";
            }
        }

        private void btTambahPromo_Click(object sender, RoutedEventArgs e)
        {
            string kode = tbKodePromo.Text;
            string potongan = tbPotongan.Text;
            string potonganmax = tbPotonganMax.Text;
            string hargamin = tbHargaMin.Text;
            
            string jenispotongan = cbJenisPotongan.SelectedIndex == 0 ? "P" : "F";
            string id_jenis_promo = cbJenisPromo.SelectedValue.ToString();
            if (cbJenisPotongan.SelectedIndex == -1)
            {
                MessageBox.Show("Mohon isi jenis potongan");
            }
            else if (dpAwalPromo.SelectedDate == null)
            {
                MessageBox.Show("Tanggal awal masih kosong");
            }
            else if(dpAkhirPromo.SelectedDate == null)
            {
                MessageBox.Show("Tanggal akhir masih kosong");
            }
            else
            {
                if(pvm.insert(kode, potongan, potonganmax, hargamin, jenispotongan, id_jenis_promo, dpAwalPromo.SelectedDate.Value, dpAkhirPromo.SelectedDate.Value))
                {
                    reloadPromo();
                    btTambahPromo.Visibility = Visibility.Visible;
                    btUpdatePromo.Visibility = Visibility.Hidden;
                    btHapusPromo.Visibility = Visibility.Hidden;
                    resetPromo();
                }

            }
        }

        private void tbPotongan_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Utility.NumberValidationTextBox(tbPotongan);
        }

        private void btUpdatePromo_Click(object sender, RoutedEventArgs e)
        {
            string kode = tbKodePromo.Text;
            string potongan = tbPotongan.Text;
            string potonganmax = tbPotonganMax.Text;
            string hargamin = tbHargaMin.Text;

            string jenispotongan = cbJenisPotongan.SelectedIndex == 0 ? "P" : "F";
            string id_jenis_promo = cbJenisPromo.SelectedValue.ToString();
            if (cbJenisPotongan.SelectedIndex == -1)
            {
                MessageBox.Show("Mohon isi jenis potongan");
            }
            else if (dpAwalPromo.SelectedDate == null)
            {
                MessageBox.Show("Tanggal awal masih kosong");
            }
            else if (dpAkhirPromo.SelectedDate == null)
            {
                MessageBox.Show("Tanggal akhir masih kosong");
            }
            else
            {
                pvm.update(kode, potongan, potonganmax, hargamin, jenispotongan, id_jenis_promo, dpAwalPromo.SelectedDate.Value, dpAkhirPromo.SelectedDate.Value);
                reloadPromo();
                btTambahPromo.Visibility = Visibility.Visible;
                btUpdatePromo.Visibility = Visibility.Hidden;
                btHapusPromo.Visibility = Visibility.Hidden;
                resetPromo();


            }
        }

        private void btHapusPromo_Click(object sender, RoutedEventArgs e)
        {
            pvm.delete();
            reloadPromo();
            btTambahPromo.Visibility = Visibility.Visible;
            btUpdatePromo.Visibility = Visibility.Hidden;
            btHapusPromo.Visibility = Visibility.Hidden;
            resetPromo();
        }

        private void dgPromo_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }

        private void dgJenisPromo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(dgJenisPromo.SelectedIndex != -1)
            {
                int konter = 0;
                DataRow dr = jpvm.selectData(dgJenisPromo.SelectedIndex);
                
                if (dr == null) return;
                tbJenisPromo.Text = dr[0].ToString();
                if(dr["Status"].ToString() == "Aktif")
                {
                    btHapusJenisPromo.Content = "Matikan";
                    btHapusJenisPromo.Style = (Style)Application.Current.Resources["btn-danger"];
                }
                else
                {
                    btHapusJenisPromo.Content = "Nyalakan";
                    btHapusJenisPromo.Style = (Style)Application.Current.Resources["btn-primary"];

                }
                if (dr["Kategori"].ToString() != "-")
                {
                    foreach (DataRow dr1 in jpvm.getCategory().Rows)
                    {
                        if (dr["Kategori"].ToString() == dr1[1].ToString())
                        {
                            cbKategori.SelectedIndex = konter;
                            cebeKategori.IsChecked = true;
                            cbKategori.Visibility = Visibility.Visible;
                            break;
                            //MessageBox.Show(dr1[0].ToString());

                        }
                        konter++;
                    }
                }
                konter = 0;

                if (dr["Kurir"].ToString() != "-")
                {
                    foreach (DataRow dr1 in jpvm.getCourier().Rows)
                    {
                        if (dr["Kurir"].ToString() == dr1[1].ToString())
                        {
                            cbKurir.SelectedIndex = konter;
                            cebeKurir.IsChecked = true;
                            cbKurir.Visibility = Visibility.Visible;
                            break;
                            //MessageBox.Show(dr1[0].ToString());

                        }
                        konter++;
                    }
                }
                konter = 0;

                if (dr["Seller"].ToString() != "-")
                {
                    foreach (DataRow dr1 in jpvm.getSeller().Rows)
                    {
                        if (dr["Seller"].ToString() == dr1[1].ToString())
                        {
                            cbSeller.SelectedIndex = konter;
                            cebeSeller.IsChecked = true;
                            cbSeller.Visibility = Visibility.Visible;
                            break;
                            //MessageBox.Show(dr1[0].ToString());

                        }
                        konter++;
                    }
                }
                konter = 0;
                if (dr["Metode Pembayaran"].ToString() != "-")
                {
                    foreach (DataRow dr1 in jpvm.getMetode_Pembayaran().Rows)
                    {
                        if (dr["Metode Pembayaran"].ToString() == dr1[1].ToString())
                        {
                            cbMetodePembayaran.SelectedIndex = konter;
                            cebeMetodePembayaran.IsChecked = true;
                            cbMetodePembayaran.Visibility = Visibility.Visible;
                            break;
                            //MessageBox.Show(dr1[0].ToString());

                        }
                        konter++;
                    }
                }
                konter = 0;
                btTambahJenisPromo.Visibility = Visibility.Hidden;
                btUpdateJenisPromo.Visibility = Visibility.Visible;
                cancelJenisPromo.Visibility = Visibility.Visible;
                btHapusJenisPromo.Visibility = Visibility.Visible;

            }
        }

        private void btJenisPromo_Click(object sender, RoutedEventArgs e)
        {
            SwapJenisPromo(1);
            reloadJenisPromo();
            resetJenisPromo();
        }
        void reloadJenisPromo()
        {
            jpvm = new JenisPromoViewModel();
            dgJenisPromo.ItemsSource = jpvm.getDataTable().DefaultView;
            
            cbKategori.ItemsSource = jpvm.getCategory().DefaultView;
            cbKategori.DisplayMemberPath = "NAMA";
            cbKategori.SelectedValuePath = "ID";

            cbKurir.ItemsSource = jpvm.getCourier().DefaultView;
            cbKurir.DisplayMemberPath = "NAMA";
            cbKurir.SelectedValuePath = "ID";

            cbSeller.ItemsSource = jpvm.getSeller().DefaultView;
            cbSeller.DisplayMemberPath = "NAMA_TOKO";
            cbSeller.SelectedValuePath = "ID";

            cbMetodePembayaran.ItemsSource = jpvm.getMetode_Pembayaran().DefaultView;
            cbMetodePembayaran.DisplayMemberPath = "NAMA";
            cbMetodePembayaran.SelectedValuePath = "ID";


        }
        void resetJenisPromo()
        {
            cbKurir.Visibility = Visibility.Hidden;
            cbKategori.Visibility = Visibility.Hidden;
            cbSeller.Visibility = Visibility.Hidden;
            cbMetodePembayaran.Visibility = Visibility.Hidden;
            tbJenisPromo.Text = "";

            btTambahJenisPromo.Visibility = Visibility.Visible;
            btUpdateJenisPromo.Visibility = Visibility.Hidden;
            btHapusJenisPromo.Visibility = Visibility.Hidden;
            cancelJenisPromo.Visibility = Visibility.Hidden;
            cebeKurir.IsChecked = false;
            cebeKategori.IsChecked = false;
            cebeSeller.IsChecked = false;
            cebeMetodePembayaran.IsChecked = false;
        }

        private void cebeKategori_Click(object sender, RoutedEventArgs e)
        {
            if(cebeKategori.IsChecked == true)
            {
                cbKategori.Visibility = Visibility.Visible;
            }
            else
            {
                cbKategori.Visibility = Visibility.Hidden;
            }
        }

        private void cebeKurir_Click(object sender, RoutedEventArgs e)
        {
            if (cebeKurir.IsChecked == true)
            {
                cbKurir.Visibility = Visibility.Visible;
            }
            else
            {
                cbKurir.Visibility = Visibility.Hidden;
            }
        }

        private void cebeSeller_Click(object sender, RoutedEventArgs e)
        {
            if (cebeSeller.IsChecked == true)
            {
                cbSeller.Visibility = Visibility.Visible;
            }
            else
            {
                cbSeller.Visibility = Visibility.Hidden;
            }
        }

        private void cebeMetodePembayaran_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void btTambahJenisPromo_Click(object sender, RoutedEventArgs e)
        {
            string jenispromo = tbJenisPromo.Text, kategori = "", kurir = "", seller = "", metodepembayaran = "";
            if(cebeKategori.IsChecked == true)
            {
                if(cbKategori.SelectedIndex == -1)
                {
                    MessageBox.Show("Mohon isi kategori");
                    return;
                }
                else
                {
                    kategori = cbKategori.SelectedValue.ToString();
                }
            }
            if (cebeKurir.IsChecked == true)
            {
                if (cbKurir.SelectedIndex == -1)
                {
                    MessageBox.Show("Mohon isi kurir");
                    return;
                }
                else
                {
                    kurir = cbKurir.SelectedValue.ToString();
                }
            }
            if (cebeSeller.IsChecked == true)
            {
                if (cbSeller.SelectedIndex == -1)
                {
                    MessageBox.Show("Mohon isi seller");
                    return;
                }
                else
                {
                    seller = cbSeller.SelectedValue.ToString();
                }
            }
            if (cebeMetodePembayaran.IsChecked == true)
            {
                if (cbMetodePembayaran.SelectedIndex == -1)
                {
                    MessageBox.Show("Mohon isi metode pembayaran");
                    return;
                }
                else
                {
                    metodepembayaran = cbMetodePembayaran.SelectedValue.ToString();
                }
            }
            if(jpvm.insert(jenispromo, kategori, kurir, seller, metodepembayaran))
            {
                reloadJenisPromo();
                resetJenisPromo();
            }
        }

        private void btUpdateJenisPromo_Click(object sender, RoutedEventArgs e)
        {
            string jenispromo = tbJenisPromo.Text, kategori = "", kurir = "", seller = "", metodepembayaran = "";
            if (cebeKategori.IsChecked == true)
            {
                if (cbKategori.SelectedIndex == -1)
                {
                    MessageBox.Show("Mohon isi kategori");
                    return;
                }
                else
                {
                    kategori = cbKategori.SelectedValue.ToString();
                }
            }
            if (cebeKurir.IsChecked == true)
            {
                if (cbKurir.SelectedIndex == -1)
                {
                    MessageBox.Show("Mohon isi kurir");
                    return;
                }
                else
                {
                    kurir = cbKurir.SelectedValue.ToString();
                }
            }
            if (cebeSeller.IsChecked == true)
            {
                if (cbSeller.SelectedIndex == -1)
                {
                    MessageBox.Show("Mohon isi seller");
                    return;
                }
                else
                {
                    seller = cbSeller.SelectedValue.ToString();
                }
            }
            if (cebeMetodePembayaran.IsChecked == true)
            {
                if (cbMetodePembayaran.SelectedIndex == -1)
                {
                    MessageBox.Show("Mohon isi metode pembayaran");
                    return;
                }
                else
                {
                    metodepembayaran = cbMetodePembayaran.SelectedValue.ToString();
                }
            }
            jpvm.update(jenispromo, kategori, kurir, seller, metodepembayaran);
            reloadJenisPromo();
            resetJenisPromo();
            
        }

        private void btOfficialStore_Click(object sender, RoutedEventArgs e)
        {
            setCanvas(8);
            reloadOfficialStore();
        }
        void reloadOfficialStore()
        {
            osvm = new OfficialStoreViewModel();
            dgOS.ItemsSource = osvm.getHtom().DefaultView;
            CanvasDetailOfficialStore.Visibility = Visibility.Hidden;
        }

        private void dgOS_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(dgOS.SelectedIndex != -1)
            {
                DataRow dr = osvm.getDetailToko(dgOS.SelectedIndex);
                if (dr == null) return;
                tbNamaTokoOS.Text = dr[0].ToString();
                tbEmailOS.Text = dr[1].ToString();
                tbAlamatOS.Text = dr[2].ToString();
                tbNoTelpOS.Text = dr[3].ToString();
                tbNamaSellerOS.Text = dr[4].ToString();
                tbMendaftarSejakOS.Text = dr[5].ToString();
                CanvasDetailOfficialStore.Visibility = Visibility.Visible;
                if (osvm.getOS_Status())
                {
                    btTerimaOS.Visibility = Visibility.Hidden;
                    btTolakOS.Visibility = Visibility.Hidden;
                }
                else
                {
                    btTerimaOS.Visibility = Visibility.Visible;
                    btTolakOS.Visibility = Visibility.Visible;
                }
                DataRow temp = osvm.getHtomHelper();
                if (temp[1].ToString() == "A") btTolakOS.Visibility = Visibility.Visible;
            }
        }

        private void btTolakOS_Click(object sender, RoutedEventArgs e)
        {
            osvm.ChangeStatus(false);
            CanvasDetailOfficialStore.Visibility = Visibility.Hidden;
            reloadOfficialStore();
        }

        private void btTerimaOS_Click(object sender, RoutedEventArgs e)
        {
            osvm.ChangeStatus(true);
            CanvasDetailOfficialStore.Visibility = Visibility.Hidden;
            reloadOfficialStore();
        }

        private void cebeMetodePembayaran_Click(object sender, RoutedEventArgs e)
        {
            if (cebeMetodePembayaran.IsChecked == true)
            {
                cbMetodePembayaran.Visibility = Visibility.Visible;
            }
            else
            {
                cbMetodePembayaran.Visibility = Visibility.Hidden;
            }
        }

        private void btLogout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dr = MessageBox.Show("Apakah anda yakin mau logout", "Logout", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(dr == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void btHapusJenisPromo_Click(object sender, RoutedEventArgs e)
        {
            jpvm.delete();
            reloadJenisPromo();
            resetJenisPromo();
        }

        private void tbPotongan_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Utility.NumberValidationTextBox(sender, e);
        }

        private void tbPotonganMax_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Utility.NumberValidationTextBox(sender, e);
        }

        private void tbHargaMin_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Utility.NumberValidationTextBox(sender, e);
        }

        private void tbNotelpSeller_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Utility.NumberValidationTextBox(sender, e);
        }

        private void tbNotelpCustomer_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Utility.NumberValidationTextBox(sender, e);
        }

        private void tbHargaKurir_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Utility.NumberValidationTextBox(sender, e);
        }

        private void tbNoTelpOS_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Utility.NumberValidationTextBox(sender, e);
        }

        private void btSelectDate_Click(object sender, RoutedEventArgs e)
        {
            hvm.refreshJumlahTransaksi(dpTanggalAwal.SelectedDate.Value, dpTanggalAkhir.SelectedDate.Value);
            //initReport();
            //chartJumlahTransaksi.Update(true,true);
            updateChart();

        }

        

        private void cbSemuaPromo_Click(object sender, RoutedEventArgs e)
        {
            setChecked(lbJenisPromo, cbSemuaPromo.IsChecked.Value);
            if(cbSemuaPromo.IsChecked == true)
            {
                lbJenisPromo.Visibility = Visibility.Hidden;
            }
            else
            {
                lbJenisPromo.Visibility = Visibility.Visible;
            }
        }

        private void cbSemuaKategori_Click(object sender, RoutedEventArgs e)
        {
            setChecked(lbJenisKategori, cbSemuaKategori.IsChecked.Value);
            if (cbSemuaKategori.IsChecked == true)
            {
                lbJenisKategori.Visibility = Visibility.Hidden;
            }
            else
            {
                lbJenisKategori.Visibility = Visibility.Visible;
            }
        }

        private void cbSemuaKurir_Click(object sender, RoutedEventArgs e)
        {
            setChecked(lbKurir, cbSemuaKurir.IsChecked.Value);
            if (cbSemuaKurir.IsChecked == true)
            {
                lbKurir.Visibility = Visibility.Hidden;
            }
            else
            {
                lbKurir.Visibility = Visibility.Visible;
            }
        }

        private void cbSemuaJenisPembayaran_Click(object sender, RoutedEventArgs e)
        {
            setChecked(lbJenisPembayaran, cbSemuaJenisPembayaran.IsChecked.Value);
            if (cbSemuaJenisPembayaran.IsChecked == true)
            {
                lbJenisPembayaran.Visibility = Visibility.Hidden;
            }
            else
            {
                lbJenisPembayaran.Visibility = Visibility.Visible;
            }
        }
        private void btGenerateReport_Click(object sender, RoutedEventArgs e)
        {
            hvm.generateReport(getSelected(lbJenisPembayaran), getSelected(lbKurir), getSelected(lbJenisKategori), getSelected(lbJenisPromo), dpTanggalAwalReport.SelectedDate.Value, dpTanggalAkhirReport.SelectedDate.Value, cbisOfficial.SelectedIndex);
        }

        private void cancelJenisPembayaran_Click(object sender, RoutedEventArgs e)
        {
            btJenisPembayaran_Click(null, null);
        }

        private void cancelCategory_Click(object sender, RoutedEventArgs e)
        {
            btCategory_Click(null, null);
        }

        private void cancelCourier_Click(object sender, RoutedEventArgs e)
        {
            btKurir_Click(null, null);
        }

        private void cancelPromo_Click(object sender, RoutedEventArgs e)
        {
            btPromo_Click(null, null);
        }

        private void cancelJenisPromo_Click(object sender, RoutedEventArgs e)
        {
            resetJenisPromo();
        }
    }
}
