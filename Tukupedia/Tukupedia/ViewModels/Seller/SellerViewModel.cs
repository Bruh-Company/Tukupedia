using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tukupedia.Models;
using Tukupedia.Views;
using Tukupedia.Views.Seller;
using Tukupedia.Helpers.Utils;
using Tukupedia.Helpers.DatabaseHelpers;
using System.Windows;
using System.Data;
using System.Windows.Media;

namespace Tukupedia.ViewModels {
    public static class SellerViewModel {
        public enum page { Pesanan, Produk, InfoToko }

        private static SellerView ViewComponent;
        private static Transition transition;
        private const int transFPS = 50;
        private const double speedMargin = 0.3;
        private const double speedOpacity = 0.4;
        private const double multiplier = 80;

        private static DataRow seller;
        private static ItemModel itemModel;
        public static void InitializeView(SellerView view) {
            seller = Session.User;
            ViewComponent = view;
            transition = new Transition(transFPS);
            initState();
            initHeader();
        }

        public static void TESTING() {
            seller = new DB("SELLER").select().where("ID", "1").getFirst();
        }

        public static void initState() {
            ViewComponent.canvasPesanan.Margin = MarginPosition.Middle;
            ViewComponent.canvasProduk.Margin = MarginPosition.Right;
            ViewComponent.canvasInfoToko.Margin = MarginPosition.Right;

            ComponentHelper.changeVisibilityComponent(ViewComponent.canvasPesanan, Visibility.Visible);
            ComponentHelper.changeVisibilityComponent(ViewComponent.canvasProduk, Visibility.Hidden);
            ComponentHelper.changeVisibilityComponent(ViewComponent.canvasInfoToko, Visibility.Hidden);
        }

        public static void initHeader() {
            TESTING();
            ViewComponent.labelNamaToko.Content = seller["NAMA_TOKO"].ToString();
            ViewComponent.labelNamaPenjual.Content = seller["NAMA_SELLER"].ToString();
            ViewComponent.labelSaldo.Content = "Rp " + seller["SALDO"].ToString();
        }

        public static void swapTo(page p) {
            if (p == page.Pesanan) {
                transition.makeTransition(ViewComponent.canvasPesanan,
                    MarginPosition.Middle, 1,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");

                transition.makeTransition(ViewComponent.canvasProduk,
                    MarginPosition.Right, 0,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");

                transition.makeTransition(ViewComponent.canvasInfoToko,
                    MarginPosition.Right, 0,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");

                transition.playTransition();
                ComponentHelper.changeZIndexComponent(
                    ViewComponent.canvasPesanan,
                    Visibility.Visible);
                ComponentHelper.changeZIndexComponent(
                    ViewComponent.canvasProduk,
                    Visibility.Hidden);
                ComponentHelper.changeZIndexComponent(
                    ViewComponent.canvasInfoToko,
                    Visibility.Hidden);
            }

            if (p == page.Produk) {
                transition.setCallback(initPageProduk);

                transition.makeTransition(ViewComponent.canvasProduk,
                    MarginPosition.Middle, 1,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");

                transition.makeTransition(ViewComponent.canvasPesanan,
                    MarginPosition.Right, 0,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");

                transition.makeTransition(ViewComponent.canvasInfoToko,
                    MarginPosition.Right, 0,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");


                transition.playTransition();
                ComponentHelper.changeZIndexComponent(
                    ViewComponent.canvasProduk,
                    Visibility.Visible);
                ComponentHelper.changeZIndexComponent(
                    ViewComponent.canvasPesanan,
                    Visibility.Hidden);
                ComponentHelper.changeZIndexComponent(
                    ViewComponent.canvasInfoToko,
                    Visibility.Hidden);
            }

            if (p == page.InfoToko) {
                transition.makeTransition(ViewComponent.canvasInfoToko,
                    MarginPosition.Middle, 1,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");

                transition.makeTransition(ViewComponent.canvasProduk,
                    MarginPosition.Right, 0,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");

                transition.makeTransition(ViewComponent.canvasPesanan,
                    MarginPosition.Right, 0,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");

                transition.playTransition();
                ComponentHelper.changeZIndexComponent(
                    ViewComponent.canvasInfoToko,
                    Visibility.Visible);
                ComponentHelper.changeZIndexComponent(
                    ViewComponent.canvasProduk,
                    Visibility.Hidden);
                ComponentHelper.changeZIndexComponent(
                    ViewComponent.canvasPesanan,
                    Visibility.Hidden);
            }
        }
        
        public static void logout() {
            Session.Logout();
            new LoginRegisterView().Show();
            ViewComponent.Close();
        }


        // PAGE PRODUK
        public static void initPageProduk() {
            fillCmbSort();
            fillCmbKategori();
            fillDgvProduk();
            fillCmbBerat();
        }

        public static void fillCmbKategori() {
            CategoryModel categoryModel = new CategoryModel();
            ViewComponent.comboboxKategori.ItemsSource = "";
            ViewComponent.comboboxKategori.ItemsSource = categoryModel.Table.DefaultView;
            ViewComponent.comboboxKategori.DisplayMemberPath = "NAMA";
            ViewComponent.comboboxKategori.SelectedValuePath = "ID";
        }

        public static void fillCmbSort() {
            ViewComponent.comboboxSortProduk.Items.Clear();
            ViewComponent.comboboxSortProduk.Items.Add("Harga Tertinggi");
            ViewComponent.comboboxSortProduk.Items.Add("Harga Terendah");
            ViewComponent.comboboxSortProduk.Items.Add("Nama: A-Z");
            ViewComponent.comboboxSortProduk.Items.Add("Nama: Z-A");
            ViewComponent.comboboxSortProduk.Items.Add("Stok Tersedikit");
            ViewComponent.comboboxSortProduk.Items.Add("Stok Terbanyak");
        }

        public static void fillCmbBerat() {
            ViewComponent.comboboxBerat.Items.Clear();
            ViewComponent.comboboxBerat.Items.Add("Gram");
            ViewComponent.comboboxBerat.Items.Add("KG");
            ViewComponent.comboboxBerat.SelectedIndex = 0;
        }

        public static void fillDgvProduk() {
            string statement = $"SELECT " +
                $"i.KODE as \"KODE BARANG\", " +
                $"i.NAMA as \"NAMA BARANG\", " +
                $"c.NAMA as \"KATEGORI\", " +
                $"i.HARGA as \"HARGA\", " +
                $"i.STOK as \"STOK\", " +
                $"i.RATING as \"RATING\" " +
                $"FROM ITEM i, CATEGORY c " +
                $"WHERE i.ID_SELLER = '{seller["ID"]}' " +
                $"and i.ID_CATEGORY = c.ID";
            //MessageBox.Show(statement);
            itemModel = new ItemModel();
            itemModel.initAdapter(statement);

            ViewComponent.datagridProduk.ItemsSource = null;
            ViewComponent.datagridProduk.ItemsSource = itemModel.Table.DefaultView;
        }

        public static void fillDgvProduk(string keyword) {
            string statement = $"SELECT " +
                $"i.KODE as \"KODE BARANG\", " +
                $"i.NAMA as \"NAMA BARANG\", " +
                $"c.NAMA as \"KATEGORI\", " +
                $"i.HARGA as \"HARGA\", " +
                $"i.STOK as \"STOK\", " +
                $"i.RATING as \"RATING\" " +
                $"FROM ITEM i, CATEGORY c " +
                $"WHERE i.ID_SELLER = '{seller["ID"]}' " +
                $"and i.ID_CATEGORY = c.ID " +
                $"and (i.NAMA like '%{keyword}%' or " +
                $"c.NAMA like '%{keyword}%')";
            //MessageBox.Show(statement);
            itemModel = new ItemModel();
            itemModel.initAdapter(statement);

            ViewComponent.datagridProduk.ItemsSource = null;
            ViewComponent.datagridProduk.ItemsSource = itemModel.Table.DefaultView;
        }

        public static void searchProduk() {
            string keyword = ViewComponent.textboxCariProduk.Text.ToUpper();
            if (keyword == "") return;
            fillDgvProduk(keyword);
        }

        public static void sortProduk() {
            int selectedIndex = ViewComponent.comboboxSortProduk.SelectedIndex;

            if (selectedIndex == 0) itemModel.Table.DefaultView.Sort = "HARGA desc";
            if (selectedIndex == 1) itemModel.Table.DefaultView.Sort = "HARGA asc";
            if (selectedIndex == 2) itemModel.Table.DefaultView.Sort = "NAMA asc";
            if (selectedIndex == 3) itemModel.Table.DefaultView.Sort = "NAMA desc";
            if (selectedIndex == 4) itemModel.Table.DefaultView.Sort = "STOK asc";
            if (selectedIndex == 5) itemModel.Table.DefaultView.Sort = "STOK desc";
        }

        public static void selectProduk() {
            if (ViewComponent.datagridProduk.SelectedIndex == -1)
                return;

            DataRow row = new DB("ITEM").select().where("KODE", itemModel.Table.Rows[0].ToString()).getFirst();

            if (row == null) return;

            ViewComponent.textboxNamaProduk.Text = row["NAMA"].ToString();
            ViewComponent.comboboxKategori.SelectedIndex = Convert.ToInt32(row["ID_CATEGORY"].ToString());
            ViewComponent.textboxHarga.Text = row["HARGA"].ToString();
            ViewComponent.textboxStok.Text = row["STOK"].ToString();
            ViewComponent.textboxBerat.Text = row["BERAT"].ToString();
            if (row["STATUS"].ToString() == "0") ViewComponent.checkboxStatusProduk.IsChecked = false;
            else ViewComponent.checkboxStatusProduk.IsChecked = true;
            ViewComponent.textboxDeskripsi.Text = row["DESKRIPSI"].ToString();
        }

        public static void cbBeratSelectionChanged() {
            if (ViewComponent.comboboxBerat.SelectedIndex == 1) {
                int berat = Convert.ToInt32(ViewComponent.textboxBerat.Text)/1000;
                ViewComponent.textboxBerat.Text = berat + "";
            }
        }

        private static string[] getData() {
            string nama = ViewComponent.textboxNamaProduk.Text,
                idCategory = (ViewComponent.comboboxKategori.SelectedIndex + 1) + "",
                harga = ViewComponent.textboxHarga.Text,
                stok = ViewComponent.textboxStok.Text,
                berat = ViewComponent.textboxBerat.Text,
                deskripsi = ViewComponent.textboxDeskripsi.Text;
            string[] data = { nama, idCategory, harga, stok, berat, deskripsi };
            return data;
        }

        private static bool dataValidation(string[] data) {
            if (data.Length < 5) return false;
            foreach (string item in data) {
                if (item == "") return false;
            }
            return true;
        }

        private static void resetPageProduk() {
            ViewComponent.textboxNamaProduk.Text = "";
            ViewComponent.textboxHarga.Text = "";
            ViewComponent.textboxStok.Text = "";
            ViewComponent.textboxBerat.Text = "";
            ViewComponent.textboxDeskripsi.Text = "";
            ViewComponent.comboboxKategori.SelectedIndex = -1;
            ViewComponent.checkboxStatusProduk.IsChecked = false;
            fillDgvProduk();
        }


        public static void insert() {
            string[] data = getData();
            if (!dataValidation(data)) return;

            string nama = data[0],
                deskripsi = data[5];
            int idCategory = Convert.ToInt32(data[1]),
                harga = Convert.ToInt32(data[2]),
                stok = Convert.ToInt32(data[3]),
                berat = Convert.ToInt32(data[4]);
            char status = ViewComponent.checkboxStatusProduk.IsChecked == false ? status = '0' : '1';

            ItemModel model = new ItemModel();
            model.init();
            DataRow newItem = model.Table.NewRow();
            newItem["ID"] = 0;
            newItem["KODE"] = "";
            newItem["NAMA"] = nama.ToUpper();
            newItem["DESKRIPSI"] = deskripsi.ToUpper();
            newItem["ID_CATEGORY"] = idCategory;
            newItem["STOK"] = stok;
            newItem["BERAT"] = berat;
            newItem["HARGA"] = harga;
            newItem["STATUS"] = status;
            model.insert(newItem);

            resetPageProduk();
        }
    }
}
