using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Tukupedia.ViewModels.Seller;
using Tukupedia.Helpers.Utils;

namespace Tukupedia.Views.Seller {
    /// <summary>
    /// Interaction logic for SellerView.xaml
    /// </summary>
    public partial class SellerView : Window {
        public SellerView() {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            SellerViewModel.InitializeView(this);
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

        private void btnStatistikToko_Click(object sender, RoutedEventArgs e) {

        }

        private void btnLogout_Click(object sender, RoutedEventArgs e) {
            SellerViewModel.logout();
        }
        // Header

        // Pesanan
        // Pesanan Header
        private void btnSemuaPesanan_Click(object sender, RoutedEventArgs e) {

        }

        private void btnPesananBaru_Click(object sender, RoutedEventArgs e) {

        }

        private void btnDalamPengiriman_Click(object sender, RoutedEventArgs e) {

        }

        private void btnPesananSelesai_Click(object sender, RoutedEventArgs e) {

        }

        private void btnPesananDibatalkan_Click(object sender, RoutedEventArgs e) {

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

        }

        private void datagridProdukPesanan_MouseDoubleClick(object sender, MouseButtonEventArgs e) {

        }

        private void datagridProdukPesanan_LoadingRow(object sender, DataGridRowEventArgs e) {

        }

        private void checkboxTerimaSemua_Checked(object sender, RoutedEventArgs e) {

        }

        private void btnKonfirmasiPesanan_Click(object sender, RoutedEventArgs e) {

        }

        private void btnBatalPesanan_Click_1(object sender, RoutedEventArgs e) {

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

        private void btnDelete_Click(object sender, RoutedEventArgs e) {
            SellerViewModel.pageProduk.deleteProduk();
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
            SellerViewModel.pageInfoToko.resetInfo();
        }

        // Info
    }
}
