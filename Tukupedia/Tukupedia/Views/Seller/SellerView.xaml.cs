using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Tukupedia.ViewModels.Seller;
using Tukupedia.Helpers.Utils;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System;

namespace Tukupedia.Views.Seller {
    /// <summary>
    /// Interaction logic for SellerView.xaml
    /// </summary>
    public partial class SellerView : Window {

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private const int GWL_STYLE = -16;
        private const int WS_MAXIMIZEBOX = 0x10000;

        public SellerView() {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            SellerViewModel.InitializeView(this);

            IntPtr hwnd = new WindowInteropHelper(sender as Window).Handle;
            int value = GetWindowLong(hwnd, GWL_STYLE);
            SetWindowLong(hwnd, GWL_STYLE, value & ~WS_MAXIMIZEBOX);
        }

        // Header
        private void btnPesanan_Click(object sender, RoutedEventArgs e) {
            SellerViewModel.swapTo(SellerViewModel.page.Pesanan);
        }

        private void btnProduk_Click(object sender, RoutedEventArgs e) {
            SellerViewModel.swapTo(SellerViewModel.page.Produk);
        }

        private void btnInfoToko_Click(object sender, RoutedEventArgs e) {
            SellerViewModel.swapTo(SellerViewModel.page.InfoToko);
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e) {
            SellerViewModel.logout();
        }
        // Header

        // Pesanan
        // Pesanan Header
        private void btnSemuaPesanan_Click(object sender, RoutedEventArgs e) {
            SellerViewModel.pagePesanan.viewSemuaPesanan();
        }

        private void btnPesananBaru_Click(object sender, RoutedEventArgs e) {
            SellerViewModel.pagePesanan.viewPesananBaru();
        }

        private void btnDalamPengiriman_Click(object sender, RoutedEventArgs e) {
            SellerViewModel.pagePesanan.viewDalamPengiriman();
        }

        private void btnPesananSelesai_Click(object sender, RoutedEventArgs e) {
            SellerViewModel.pagePesanan.viewPesananSelesai();
        }

        private void btnPesananDibatalkan_Click(object sender, RoutedEventArgs e) {
            SellerViewModel.pagePesanan.viewPesananBatal();
        }
        // Pesanan Header

        private void btnCariPesanan_Click(object sender, RoutedEventArgs e) {

        }

        private void comboboxFilterKurir_SelectionChanged(object sender, SelectionChangedEventArgs e) {

        }

        private void comboboxSortPesanan_SelectionChanged(object sender, SelectionChangedEventArgs e) {

        }

        private void datagridPesanan_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            SellerViewModel.pagePesanan.selectHtrans(datagridPesanan.SelectedIndex);

        }

        private void datagridProdukPesanan_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            SellerViewModel.pagePesanan.toggleDtrans();
            
        }

        private void datagridProdukPesanan_LoadingRow(object sender, DataGridRowEventArgs e) {
            SellerViewModel.pagePesanan.checkStatus(e.Row);
        }

        private void checkboxTerimaSemua_Checked(object sender, RoutedEventArgs e) {
            SellerViewModel.pagePesanan.terimasemua((bool)checkboxTerimaSemua.IsChecked);
        }
        private void checkboxTerimaSemua_Click(object sender, RoutedEventArgs e)
        {
            SellerViewModel.pagePesanan.terimasemua((bool)checkboxTerimaSemua.IsChecked);

        }

        private void btnKonfirmasiPesanan_Click(object sender, RoutedEventArgs e) {
            SellerViewModel.pagePesanan.confirm();
        }

        private void btnBatalPesanan_Click(object sender, RoutedEventArgs e) {
            SellerViewModel.pagePesanan.reloadHtrans();
        }

        // Pesanan

        // Produk
        // Produk Header

        private void btnCariProduk_Click(object sender, RoutedEventArgs e) {
            SellerViewModel.pageProduk.searchProduk();
        }

        private void comboboxSortProduk_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            SellerViewModel.pageProduk.sortProduk();
        }

        private void datagridProduk_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e) {
            SellerViewModel.pageProduk.selectProduk();
        }

        private void datagridProduk_LoadingRow(object sender, DataGridRowEventArgs e) {
            SellerViewModel.pageProduk.checkStok(e.Row);
        }

        private void btnPilihGambarProduk_Click(object sender, RoutedEventArgs e) {
            SellerViewModel.pageProduk.insertImage();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) {
            SellerViewModel.pageProduk.cancelProduk();
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e) {
            SellerViewModel.pageProduk.insertProduk();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e) {
            SellerViewModel.pageProduk.updateProduk();
        }

        private void textboxHarga_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            Utility.NumberValidationTextBox(sender, e);
        }

        private void textboxStok_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            Utility.NumberValidationTextBox(sender, e);
        }

        private void textboxBerat_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            Utility.NumberValidationTextBox(sender, e);
        }
        // Produk

        // Info
        private void btnDaftarOS_Click(object sender, RoutedEventArgs e) {
            SellerViewModel.pageInfoToko.registerOS();
        }

        private void btnTambahKurirInfo_Click(object sender, RoutedEventArgs e) {
            SellerViewModel.pageInfoToko.addKurir();
        }

        private void btnKurangKurirInfo_Click(object sender, RoutedEventArgs e) {
            SellerViewModel.pageInfoToko.deleteKurir();
        }

        private void btnUbahInfoPenjual_Click(object sender, RoutedEventArgs e) {
            SellerViewModel.pageInfoToko.toggleState();
        }

        private void btnSimpanInfo_Click(object sender, RoutedEventArgs e) {
            SellerViewModel.pageInfoToko.saveInfo();
        }

        private void btnBatalInfo_Click(object sender, RoutedEventArgs e) {
            SellerViewModel.pageInfoToko.cancelInfo();
        }

        private void btnChangeImage_Click(object sender, RoutedEventArgs e) {
            SellerViewModel.pageInfoToko.changeTokoPic();
        }

        private void textboxNoTelpInfo_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            Utility.NumberValidationTextBox(sender, e);
        }
        // Info
    }
}
