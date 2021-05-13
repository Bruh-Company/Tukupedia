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
            SellerViewModel.searchProduk();
        }

        private void comboboxSortProduk_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            SellerViewModel.sortProduk();
        }

        private void comboboxBerat_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            SellerViewModel.cbBeratSelectionChanged();
        }

        private void datagridProduk_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e) {
            SellerViewModel.selectProduk();
        }

        private void btnPilihGambarProduk_Click(object sender, RoutedEventArgs e) {

        }

        private void btnInsert_Click(object sender, RoutedEventArgs e) {
            SellerViewModel.insert();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e) {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e) {

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
