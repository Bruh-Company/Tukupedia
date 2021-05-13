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
using System.Windows.Controls;

namespace Tukupedia.ViewModels.Seller {
    public static class SellerViewModelProduk {
        private static SellerView ViewComponent;
        private static Transition transition;
        private static ItemModel itemModel;
        private static bool toggleBtnInsertProduk = true;
        private static DataRow seller;

        // PAGE PRODUK
        public static void initPageProduk() {
            seller = Session.User;
            fillCmbSort();
            fillCmbKategori();
            fillDgvProduk();
            fillCmbBerat();
            switchBtnInsert();
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
                $"i.HARGA as \"HARGA\", " +
                $"i.RATING as \"RATING\", " +
                $"i.STATUS as \"STATUS\" " +
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
                $"i.RATING as \"RATING\", " +
                $"i.STATUS as \"STATUS\" " +
                $"FROM ITEM i, CATEGORY c " +
                $"WHERE i.ID_SELLER = '{seller["ID"]}' " +
                $"and i.ID_CATEGORY = c.ID " +
                $"and (i.NAMA like '%{keyword}%' " +
                $"or c.NAMA like '%{keyword}%' " +
                $"or i.KODE like '%{keyword}%')";
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
            int selectedIndex = ViewComponent.datagridProduk.SelectedIndex;
            if (selectedIndex == -1)
                return;

            toggleBtnInsertProduk = false;
            switchBtnInsert();

            DataRow row = new DB("ITEM").select().where("KODE", itemModel.Table.Rows[selectedIndex][0].ToString()).getFirst();

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

        public static void cancelProduk() {
            toggleBtnInsertProduk = true;
            switchBtnInsert();
        }

        public static void insertProduk() {
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
            newItem["KODE"] = " ";
            newItem["NAMA"] = nama.ToUpper();
            newItem["DESKRIPSI"] = deskripsi.ToUpper();
            newItem["ID_CATEGORY"] = idCategory;
            newItem["ID_SELLER"] = seller["ID"];
            newItem["STOK"] = stok;
            newItem["BERAT"] = berat;
            newItem["HARGA"] = harga;
            newItem["STATUS"] = status;
            model.insert(newItem);

            resetPageProduk();
        }

        public static void updateProduk() {
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

            int selectedIndex = ViewComponent.datagridProduk.SelectedIndex;
            if (selectedIndex == -1)
                return;
            DataRow rowTarget = new DB("ITEM").select().where("KODE", itemModel.Table.Rows[selectedIndex][0].ToString()).getFirst();
            model.updateRow(rowTarget, "NAMA", nama, "DESKRIPSI", deskripsi, "ID_CATEGORY", idCategory, "STOK", stok, "BERAT", berat, "HARGA", harga, "STATUS", status);
            resetPageProduk();
        }

        public static void deleteProduk() {
            ItemModel model = new ItemModel();
            model.init();

            int selectedIndex = ViewComponent.datagridProduk.SelectedIndex;
            if (selectedIndex == -1)
                return;
            DataRow rowTarget = new DB("ITEM").select().where("KODE", itemModel.Table.Rows[selectedIndex][0].ToString()).getFirst();
            model.delete(rowTarget);
            resetPageProduk();
        }

        public static void checkStok(DataGridRow dgRow) {
            //DataRow data = dgRow.DataContext;
            //DataRow row = new DB("ITEM").select().where("KODE", data[0].ToString()).getFirst();
            //int stok = Convert.ToInt32(row["STOK"].ToString());

            //Color color = (Color)ColorConverter.ConvertFromString("#E23434");
            //if (stok == 0) dgRow.Background = new SolidColorBrush(color);
        }


        // PRIVATE METHODS

        private static void switchBtnInsert() {
            if (!toggleBtnInsertProduk) {
                ViewComponent.btnCancel.Visibility = Visibility.Visible;
                ViewComponent.btnInsert.Visibility = Visibility.Hidden;
            }
            else {
                ViewComponent.btnInsert.Visibility = Visibility.Visible;
                ViewComponent.btnCancel.Visibility = Visibility.Hidden;
            }
            resetPageProduk();
        }

        private static string[] getData() {

            string nama = ViewComponent.textboxNamaProduk.Text,
                idCategory = (ViewComponent.comboboxKategori.SelectedIndex + 1) + "",
                harga = ViewComponent.textboxHarga.Text,
                stok = ViewComponent.textboxStok.Text,
                berat = ViewComponent.textboxBerat.Text,
                deskripsi = ViewComponent.textboxDeskripsi.Text;

            if (ViewComponent.comboboxBerat.SelectedIndex == 1)
                berat = (Convert.ToInt32(ViewComponent.textboxBerat.Text) / 1000) + "";

            string[] data = { nama, idCategory, harga, stok, berat, deskripsi };
            return data;
        }

        private static bool dataValidation(string[] data) {
            if (data.Length < 5) return false;
            foreach (string item in data) {
                if (item == "") {
                    MessageBox.Show("Insert Gagal");
                    return false;
                }
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
            ViewComponent.datagridProduk.SelectedIndex = -1;
            fillDgvProduk();
        }
    }
}
