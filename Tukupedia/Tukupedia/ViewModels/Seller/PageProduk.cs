using Tukupedia.Models;
using Tukupedia.Views.Seller;
using Tukupedia.Helpers.DatabaseHelpers;
using System.Data;
using System.Windows.Media;
using System.Windows.Controls;
using System;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Tukupedia.Helpers.Utils;
using System.Windows;
using Oracle.DataAccess.Client;

namespace Tukupedia.ViewModels.Seller {
    public class PageProduk {
        private SellerView ViewComponent;
        private ItemModel itemModel;
        private DataRow seller;
        private bool toggleBtnInsertProduk = true;

        public PageProduk(SellerView viewComponent, DataRow seller) {
            ViewComponent = viewComponent;
            this.seller = seller;
        }

        private void DEBUG() {
            ViewComponent.comboboxKategori.SelectedIndex = 0;
            ViewComponent.textboxNamaProduk.Text = "TEST ITEM";
            ViewComponent.textboxHarga.Text = "100";
            ViewComponent.textboxStok.Text = "100";
            ViewComponent.textboxBerat.Text = "100";
            ViewComponent.textboxDeskripsi.Text = "TEST";
        }

        public void initPageProduk() {
            fillCmbSort();
            fillCmbKategori();
            fillDgvProduk();
            fillCmbBerat();
            switchBtnInsert();

            //DEBUG();
        }

        public void fillCmbKategori() {
            CategoryModel categoryModel = new CategoryModel();
            ViewComponent.comboboxKategori.ItemsSource = "";
            ViewComponent.comboboxKategori.ItemsSource = categoryModel.Table.DefaultView;
            ViewComponent.comboboxKategori.DisplayMemberPath = "NAMA";
            ViewComponent.comboboxKategori.SelectedValuePath = "ID";
        }

        public void fillCmbSort() {
            ViewComponent.comboboxSortProduk.Items.Clear();
            ViewComponent.comboboxSortProduk.Items.Add("Harga Tertinggi");
            ViewComponent.comboboxSortProduk.Items.Add("Harga Terendah");
            ViewComponent.comboboxSortProduk.Items.Add("Nama: A-Z");
            ViewComponent.comboboxSortProduk.Items.Add("Nama: Z-A");
        }

        public void fillCmbBerat() {
            ViewComponent.comboboxBerat.Items.Clear();
            ViewComponent.comboboxBerat.Items.Add("Gram");
            ViewComponent.comboboxBerat.Items.Add("KG");
            ViewComponent.comboboxBerat.SelectedIndex = 0;
        }

        public void fillDgvProduk() {
            string statement = $"SELECT " +
                $"i.KODE as \"KODE BARANG\", " +
                $"i.NAMA as \"NAMA BARANG\", " +
                $"i.HARGA as \"HARGA\", " +
                $"i.RATING as \"RATING\", " +
                $"(CASE WHEN i.STATUS = '0' THEN 'TIDAK AKTIF' ELSE 'AKTIF' END) as \"STATUS\" " +
                $"FROM ITEM i, CATEGORY c " +
                $"WHERE i.ID_SELLER = '{seller["ID"]}' " +
                $"and i.ID_CATEGORY = c.ID";

            itemModel = new ItemModel();
            itemModel.initAdapter(statement);

            ViewComponent.datagridProduk.ItemsSource = null;
            ViewComponent.datagridProduk.ItemsSource = itemModel.Table.DefaultView;
        }

        public void fillDgvProduk(string keyword) {
            string statement = $"SELECT " +
                $"i.KODE as \"KODE BARANG\", " +
                $"i.NAMA as \"NAMA BARANG\", " +
                $"c.NAMA as \"KATEGORI\", " +
                $"i.RATING as \"RATING\", " +
                $"(CASE WHEN i.STATUS = '0' THEN 'TIDAK AKTIF' ELSE 'AKTIF' END) as \"STATUS\" " +
                $"FROM ITEM i, CATEGORY c " +
                $"WHERE i.ID_SELLER = '{seller["ID"]}' " +
                $"and i.ID_CATEGORY = c.ID " +
                $"and (i.NAMA like '%{keyword}%' " +
                $"or c.NAMA like '%{keyword}%' " +
                $"or i.KODE like '%{keyword}%')";

            itemModel = new ItemModel();
            itemModel.initAdapter(statement);

            ViewComponent.datagridProduk.ItemsSource = null;
            ViewComponent.datagridProduk.ItemsSource = itemModel.Table.DefaultView;
        }

        public void searchProduk() {
            string keyword = ViewComponent.textboxCariProduk.Text.ToUpper();
            if (keyword == "") fillDgvProduk();
            else fillDgvProduk(keyword);
        }

        public void sortProduk() {
            int selectedIndex = ViewComponent.comboboxSortProduk.SelectedIndex;

            if (selectedIndex == 0) itemModel.Table.DefaultView.Sort = "HARGA desc";
            if (selectedIndex == 1) itemModel.Table.DefaultView.Sort = "HARGA asc";
            if (selectedIndex == 2) itemModel.Table.DefaultView.Sort = "NAMA BARANG asc";
            if (selectedIndex == 3) itemModel.Table.DefaultView.Sort = "NAMA BARANG desc";
        }

        public void selectProduk() {
            int selectedIndex = ViewComponent.datagridProduk.SelectedIndex;
            if (selectedIndex == -1) return;

            DataRow row = new DB("ITEM").select().where("KODE", itemModel.Table.Rows[selectedIndex][0].ToString()).getFirst();
            if (row == null) return;

            ViewComponent.textboxNamaProduk.Text = row["NAMA"].ToString();
            ViewComponent.textboxHarga.Text = row["HARGA"].ToString();
            ViewComponent.textboxStok.Text = row["STOK"].ToString();
            ViewComponent.textboxBerat.Text = row["BERAT"].ToString();
            ViewComponent.textboxDeskripsi.Text = row["DESKRIPSI"].ToString();
            ViewComponent.comboboxKategori.SelectedIndex = Convert.ToInt32(row["ID_CATEGORY"].ToString()) - 1;
            if (row["STATUS"].ToString() == "0") ViewComponent.checkboxStatusProduk.IsChecked = false;
            else ViewComponent.checkboxStatusProduk.IsChecked = true;
            ViewComponent.imageProduk.Source = Utility.loadImageItem(row["IMAGE"].ToString());

            toggleBtnInsertProduk = false;
            switchBtnInsert();
        }

        public void insertImage() {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                if (openFileDialog.FileName == "") return;
                Uri fileUri = new Uri(openFileDialog.FileName);
                ViewComponent.imageProduk.Source = new BitmapImage(fileUri);
            }
        }

        public void cancelProduk() {
            toggleBtnInsertProduk = true;
            switchBtnInsert();
            resetPageProduk();
        }

        public void insertProduk() {
            string[] data = getData();
            if (!dataValidation(data)) return;

            string nama = data[0],
                deskripsi = data[5],
                imageUri = data[6];
            int idCategory = Convert.ToInt32(data[1]),
                harga = Convert.ToInt32(data[2]),
                stok = Convert.ToInt32(data[3]),
                berat = Convert.ToInt32(data[4]);
            char status = ViewComponent.checkboxStatusProduk.IsChecked == false ? status = '0' : '1';

            StoredProcedure procedure = new StoredProcedure("GENERATE_KODE_ITEM");
            procedure.addParam("R", "ret", 30, OracleDbType.Varchar2);
            procedure.addParam("I", "nama", nama, 255, OracleDbType.Varchar2);

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
            newItem["IMAGE"] = Utility.saveImage(imageUri, procedure.getValue("ret"), 'i');
            model.insert(newItem);

            resetPageProduk();
        }

        public void updateProduk() {
            string[] data = getData();
            if (!dataValidation(data)) return;

            int selectedIndex = ViewComponent.datagridProduk.SelectedIndex;
            if (selectedIndex == -1)
                return;

            string nama = data[0],
                deskripsi = data[5],
                imageUri = data[6];
            int idCategory = Convert.ToInt32(data[1]),
                harga = Convert.ToInt32(data[2]),
                stok = Convert.ToInt32(data[3]),
                berat = Convert.ToInt32(data[4]);
            char status = ViewComponent.checkboxStatusProduk.IsChecked == false ? status = '0' : '1';

            ItemModel model = new ItemModel();
            model.init();
            model.addWhere("KODE", itemModel.Table.Rows[selectedIndex][0].ToString());
            foreach (DataRow row in model.get()) {
                model.updateRow(
                    row, 
                    "NAMA", nama, 
                    "DESKRIPSI", deskripsi, 
                    "ID_CATEGORY", idCategory, 
                    "STOK", stok, 
                    "BERAT", berat, 
                    "HARGA", harga, 
                    "STATUS", status,
                    "IMAGE", Utility.saveImage(imageUri, row["IMAGE"].ToString(), 'i')
                );
            }
            resetPageProduk();
        }

        public void checkRow(DataGridRow dgRow) {
            DataRowView item = dgRow.Item as DataRowView;
            if (item != null) {
                DataRow row = item.Row;
                DataRow data = new DB("ITEM").select().where("KODE", row["KODE BARANG"].ToString()).getFirst();
                int stok = Convert.ToInt32(data["STOK"].ToString());
                if (stok <= 0) {
                    Color color = (Color)ColorConverter.ConvertFromString("#E23434");
                    dgRow.Background = new SolidColorBrush(color);
                }

                char status = char.Parse(data["STATUS"].ToString());
                if (status == '0') {
                    Color color = (Color)ColorConverter.ConvertFromString("#FFC548");
                    dgRow.Background = new SolidColorBrush(color);
                }

            }
        }

        // PRIVATE METHODS

        private void switchBtnInsert() {
            if (!toggleBtnInsertProduk) {
                ViewComponent.btnCancel.Visibility = Visibility.Visible;
                ViewComponent.btnInsert.Visibility = Visibility.Hidden;
                ViewComponent.btnUpdate.IsEnabled = true;
            }
            else {
                ViewComponent.btnInsert.Visibility = Visibility.Visible;
                ViewComponent.btnCancel.Visibility = Visibility.Hidden;
                ViewComponent.btnUpdate.IsEnabled = false;
            }
        }

        private string[] getData() {
            string 
                nama = ViewComponent.textboxNamaProduk.Text,
                idCategory = (ViewComponent.comboboxKategori.SelectedIndex + 1) + "",
                harga = ViewComponent.textboxHarga.Text,
                stok = ViewComponent.textboxStok.Text,
                berat = ViewComponent.textboxBerat.Text,
                deskripsi = ViewComponent.textboxDeskripsi.Text,
                imageUri = ViewComponent.imageProduk.Source == null ? "" : ViewComponent.imageProduk.Source.ToString();

            if (ViewComponent.comboboxBerat.SelectedIndex == 1)
                berat = (Convert.ToInt32(ViewComponent.textboxBerat.Text) / 1000) + "";

            string[] data = { nama, idCategory, harga, stok, berat, deskripsi, imageUri };
            return data;
        }

        private bool dataValidation(string[] data) {
            if (data.Length < 6) return false;
            foreach (string item in data) {
                if (item == "") {
                    return false;
                }
            }
            return true;
        }

        private void resetPageProduk() {
            ViewComponent.textboxNamaProduk.Text = "";
            ViewComponent.textboxHarga.Text = "";
            ViewComponent.textboxStok.Text = "";
            ViewComponent.textboxBerat.Text = "";
            ViewComponent.textboxDeskripsi.Text = "";
            ViewComponent.comboboxKategori.SelectedIndex = -1;
            ViewComponent.checkboxStatusProduk.IsChecked = false;
            ViewComponent.datagridProduk.SelectedIndex = -1;
            ViewComponent.imageProduk.Source = null;
            fillDgvProduk();
        }
    }
}
