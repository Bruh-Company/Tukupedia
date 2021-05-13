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
using MaterialDesignThemes.Wpf;
using Tukupedia.ViewModels;
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
            SellerViewModelMain.InitializeView(this);
        }

        // Header
        private void btnPesanan_Click(object sender, RoutedEventArgs e) {
            SellerViewModelMain.swapTo(SellerViewModelMain.page.Pesanan);
        }

        private void btnProduk_Click(object sender, RoutedEventArgs e) {
            SellerViewModelMain.swapTo(SellerViewModelMain.page.Produk);
        }

        private void btnInfoToko_Click(object sender, RoutedEventArgs e) {
            SellerViewModelMain.swapTo(SellerViewModelMain.page.InfoToko);
        }

        private void btnStatistikToko_Click(object sender, RoutedEventArgs e) {

        }

        private void btnLogout_Click(object sender, RoutedEventArgs e) {
            Session.Logout();
            new LoginRegisterView().Show();
            this.Close();
        }
        // Header

        // Pesanan
        // Pesanan Header
        private void btnSemuaPesanan_Click(object sender, RoutedEventArgs e) {

        }

        private void btnPesananBaru_Click(object sender, RoutedEventArgs e) {

        }

        private void btnSiapKirim_Click(object sender, RoutedEventArgs e) {

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

        private void datagridPesanan_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e) {

        }

        private void btnBatalPesanan_Click(object sender, RoutedEventArgs e) {

        }

        private void btnSuccessPesanan_Click(object sender, RoutedEventArgs e) {

        }
        // Pesanan

        // Produk
        // Produk Header

        private void btnCariProduk_Click(object sender, RoutedEventArgs e) {
            SellerViewModelProduk.searchProduk();
        }

        private void comboboxSortProduk_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            SellerViewModelProduk.sortProduk();
        }

        private void datagridProduk_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e) {
            SellerViewModelProduk.selectProduk();
        }

        private void datagridProduk_LoadingRow(object sender, DataGridRowEventArgs e) {
            SellerViewModelProduk.checkStok(e.Row);
        }

        private void btnPilihGambarProduk_Click(object sender, RoutedEventArgs e) {

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) {
            SellerViewModelProduk.cancelProduk();
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e) {
            SellerViewModelProduk.insertProduk();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e) {
            SellerViewModelProduk.updateProduk();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e) {
            SellerViewModelProduk.deleteProduk();
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
        private void btnUbahDeskripsi_Click(object sender, RoutedEventArgs e) {

        }

        private void btnSimpanDeskripsi_Click(object sender, RoutedEventArgs e) {

        }

        private void btnBatalDeskripsi_Click(object sender, RoutedEventArgs e) {

        }

        private void btnTambahKurirInfo_Click(object sender, RoutedEventArgs e) {

        }

        private void btnKurangKurirInfo_Click(object sender, RoutedEventArgs e) {

        }

        // Info
    }
}
