using Tukupedia.Models;
using Tukupedia.Views.Seller;
using Tukupedia.Helpers.DatabaseHelpers;
using System.Data;
using System.Windows.Media;
using System.Windows.Controls;
using System;
using Tukupedia.Helpers.Utils;
using System.Windows;
using Oracle.DataAccess.Client;
using System.Collections.Generic;
using Tukupedia.Components;
using MaterialDesignThemes.Wpf;
using Tukupedia.Helpers.Classes;

namespace Tukupedia.ViewModels.Seller {
    public class PageProduk {
        private SellerView ViewComponent;
        private ItemModel itemModel;
        private DataRow seller;
        private bool toggleBtnInsertProduk = true;
        private string imagePath;
        private int itemId;

        public PageProduk(SellerView viewComponent, DataRow seller) {
            ViewComponent = viewComponent;
            this.seller = seller;
            //initPageProduk();
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
            resetPageProduk();
            fillCmbSort();
            fillCmbKategori();
            fillDgvProduk();
            fillCmbBerat();
            switchBtnInsert();

            //DEBUG();
        }

        public void fillCmbKategori() {
            CategoryModel categoryModel = new CategoryModel();
            categoryModel.sort = "ID";
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
            ViewComponent.comboboxSortProduk.Items.Add("Status");
        }

        public void fillCmbBerat() {
            ViewComponent.comboboxBerat.Items.Clear();
            ViewComponent.comboboxBerat.Items.Add("Gram");
            ViewComponent.comboboxBerat.Items.Add("KG");
            ViewComponent.comboboxBerat.SelectedIndex = 0;
        }

        public void initDgvProduk(string keyword = "") {
            string statement;
            if (keyword == "") {
                statement = $"SELECT " +
                    $"i.KODE as \"KODE BARANG\", " +
                    $"i.NAMA as \"NAMA BARANG\", " +
                    $"TO_CHAR(i.HARGA) as \"HARGA\", " +
                    $"c.NAMA as \"KATEGORI\", " +
                    $"NVL(i.RATING, 0) as \"RATING\", " +
                    $"(CASE WHEN i.STATUS = '0' THEN 'TIDAK AKTIF' ELSE 'AKTIF' END) as \"STATUS\" " +
                    $"FROM ITEM i, CATEGORY c " +
                    $"WHERE i.ID_SELLER = '{seller["ID"]}' " +
                    $"and i.ID_CATEGORY = c.ID";
            } else {
                statement = $"SELECT " +
                $"i.KODE as \"KODE BARANG\", " +
                $"i.NAMA as \"NAMA BARANG\", " +
                $"TO_CHAR(i.HARGA) as \"HARGA\", " +
                $"c.NAMA as \"KATEGORI\", " +
                $"NVL(i.RATING, 0) as \"RATING\", " +
                $"(CASE WHEN i.STATUS = '0' THEN 'TIDAK AKTIF' ELSE 'AKTIF' END) as \"STATUS\" " +
                $"FROM ITEM i, CATEGORY c " +
                $"WHERE i.ID_SELLER = '{seller["ID"]}' " +
                $"and i.ID_CATEGORY = c.ID " +
                $"and (i.NAMA like '%{keyword}%' " +
                $"or c.NAMA like '%{keyword}%' " +
                $"or i.KODE like '%{keyword}%')";
            }
            if (statement == "") {
                Console.WriteLine("Failed init DGV PRODUK");
                return;
            }
            itemModel = new ItemModel();
            itemModel.initAdapter(statement);
        }

        public void fillDgvProduk(string keyword = "") {
            initDgvProduk(keyword);
            formatNumberCell();
            ViewComponent.datagridProduk.ItemsSource = "";
            ViewComponent.datagridProduk.ItemsSource = itemModel.Table.DefaultView;
        }

        private void formatNumberCell() {
            foreach (DataRow dr in itemModel.Table.Rows) {
                dr["HARGA"] = "Rp " + Utility.formatNumber(Convert.ToInt32(dr["HARGA"].ToString()));
            }
        }

        public void searchProduk() {
            string keyword = ViewComponent.textboxCariProduk.Text.ToUpper(); 
            fillDgvProduk(keyword);
        }

        public void sortProduk() {
            int selectedIndex = ViewComponent.comboboxSortProduk.SelectedIndex;

            if (selectedIndex == 0) itemModel.Table.DefaultView.Sort = "HARGA desc";
            if (selectedIndex == 1) itemModel.Table.DefaultView.Sort = "HARGA asc";
            if (selectedIndex == 2) itemModel.Table.DefaultView.Sort = "NAMA BARANG asc";
            if (selectedIndex == 3) itemModel.Table.DefaultView.Sort = "NAMA BARANG desc";
            if (selectedIndex == 4) itemModel.Table.DefaultView.Sort = "STATUS asc";
        }

        public void insertImage() {
            imagePath = ImageHelper.openFileDialog(ViewComponent.imageProduk);
        }

        public void cancelProduk() {
            itemId = -1;
            toggleBtnInsertProduk = true;
            ViewComponent.spanelDiscussion.Children.Clear();
            switchBtnInsert();
            resetPageProduk();
        }

        private int getIndexCBCat(string key) {
            for (int i = 0; i < ViewComponent.comboboxKategori.Items.Count; i++) {
                DataRowView drv = (DataRowView)ViewComponent.comboboxKategori.Items[i];
                DataRow row = drv.Row;
                string id = row[0].ToString();
                if (id == key) return i;
            }
            return -1;
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
            ViewComponent.comboboxKategori.SelectedIndex = getIndexCBCat(row["ID_CATEGORY"].ToString());
            ViewComponent.comboboxBerat.SelectedIndex = 0;
            if (row["STATUS"].ToString() == "0") ViewComponent.checkboxStatusProduk.IsChecked = false;
            else ViewComponent.checkboxStatusProduk.IsChecked = true;

            imagePath = ImageHelper.getItemImagePath(row["IMAGE"].ToString());
            ImageHelper.loadImage(ViewComponent.imageProduk, row["IMAGE"].ToString(), ImageHelper.target.item);

            itemId = Convert.ToInt32(row["ID"].ToString());
            resetDiskusi();
            
            toggleBtnInsertProduk = false;
            switchBtnInsert();
        }

        public void insertProduk() {
            List<string> data = getData();
            if (!dataValidation(data)) return;

            string nama = data[0],
                deskripsi = data[5];
            int idCategory = Convert.ToInt32(data[1]),
                harga = Convert.ToInt32(data[2]),
                stok = Convert.ToInt32(data[3]),
                berat = Convert.ToInt32(data[4]);
            char status = ViewComponent.checkboxStatusProduk.IsChecked == false ? '0' : '1';

            StoredProcedure procedure = new StoredProcedure("GENERATE_KODE_ITEM");
            procedure.addParam("R", "ret", 30, OracleDbType.Varchar2);
            procedure.addParam("I", "nama", nama, 255, OracleDbType.Varchar2);

            string img = ImageHelper.saveImage(imagePath, procedure.getValue(), ImageHelper.target.item);
            if (img == null) return;
            ItemModel model = new ItemModel();
            model.init();

            DataRow newItem = model.Table.NewRow();
            newItem["ID"] = 1;
            newItem["KODE"] = " ";
            newItem["NAMA"] = nama;
            newItem["DESKRIPSI"] = deskripsi;
            newItem["ID_CATEGORY"] = idCategory;
            newItem["ID_SELLER"] = seller["ID"];
            newItem["STOK"] = stok;
            newItem["BERAT"] = berat;
            newItem["HARGA"] = harga;
            newItem["STATUS"] = status;
            newItem["IMAGE"] = img;
            model.insert(newItem);

            resetPageProduk();
        }

        public void updateProduk() {
            List<string> data = getData();
            if (!dataValidation(data)) return;

            int selectedIndex = ViewComponent.datagridProduk.SelectedIndex;
            if (selectedIndex == -1) return;

            string nama = data[0],
                deskripsi = data[5];                
            int idCategory = Convert.ToInt32(data[1]),
                harga = Convert.ToInt32(data[2]),
                stok = Convert.ToInt32(data[3]),
                berat = Convert.ToInt32(data[4]);
            char status = ViewComponent.checkboxStatusProduk.IsChecked == false ? status = '0' : '1';

            ItemModel model = new ItemModel();
            model.init();
            model.addWhere("KODE", itemModel.Table.Rows[selectedIndex][0].ToString());
            foreach (DataRow row in model.get()) {
                string img = ImageHelper.saveImage(imagePath, row["KODE"].ToString(), ImageHelper.target.item);
                if (img == null) return;
                model.updateRow(
                    row,
                    "NAMA", nama,
                    "DESKRIPSI", deskripsi,
                    "ID_CATEGORY", idCategory,
                    "STOK", stok,
                    "BERAT", berat,
                    "HARGA", harga,
                    "STATUS", status,
                    "IMAGE", img
                );
            }
            cancelProduk();
        }

        public void checkRow(DataGridRow dgRow) {
            if (dgRow.Item is DataRowView item) {
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

        public void changeBerat() {
            if (ViewComponent.textboxBerat.Text != "") {
                if (ViewComponent.comboboxBerat.SelectedIndex == 0)
                    ViewComponent.textboxBerat.Text = (Convert.ToInt32(ViewComponent.textboxBerat.Text) * 1000).ToString();
                if (ViewComponent.comboboxBerat.SelectedIndex == 1)
                    ViewComponent.textboxBerat.Text = (Convert.ToInt32(ViewComponent.textboxBerat.Text) / 1000).ToString();
            }
        }

        public void resetDiskusi() {
            ViewComponent.spanelDiscussion.Children.Clear();
            loadDiskusi(itemId);
        }

        public void checkKategoriItem() {
            if (ViewComponent.comboboxKategori.SelectedIndex != -1) {
                string id = ViewComponent.comboboxKategori.SelectedValue.ToString();
                string status = new DB("CATEGORY").select("STATUS").where("ID", id).getFirst()[0].ToString();
                if (status == "0") {
                    ViewComponent.checkboxStatusProduk.IsEnabled = false;
                }
            }
        }

        public void loadDiskusi(int id) {
            H_DiskusiModel model = new H_DiskusiModel();
            model.addWhere("ID_ITEM", id.ToString());
            if (model.get().Length <= 0) {
                ViewComponent.textblockDiskusiProduk.Text = "BELUM ADA DISKUSI";
                Color color = (Color)ColorConverter.ConvertFromString("#E23434");
                ViewComponent.textblockDiskusiProduk.Foreground = new SolidColorBrush(color);
                return;
            }
            else {
                ViewComponent.textblockDiskusiProduk.Text = "DISKUSI";
                Color color = (Color)ColorConverter.ConvertFromString("#FFFFFF");
                ViewComponent.textblockDiskusiProduk.Foreground = new SolidColorBrush(color);
            }
            model.addOrderBy("CREATED_AT ASC");
            foreach (DataRow row in model.get()) {
                DiscussionCard dc = new DiscussionCard(ViewComponent.spanelDiscussion.ActualWidth);
                DataRow customer = new DB("CUSTOMER").select().@where("ID", row["ID_CUSTOMER"].ToString()).getFirst();
                dc.initMainComment(
                    message: row["MESSAGE"].ToString(),
                    commenterName: customer["NAMA"].ToString(),
                    date: Utility.formatDate(row["CREATED_AT"].ToString()),
                    url: customer["IMAGE"].ToString()
                    );
                dc.initComments(Convert.ToInt32(row["ID"]));
                ViewComponent.spanelDiscussion.Children.Add(dc);
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

        private List<string> getData() {
            List<string> data = new List<string>();
            data.Add(ViewComponent.textboxNamaProduk.Text);
            data.Add((ViewComponent.comboboxKategori.SelectedValue).ToString());
            data.Add(ViewComponent.textboxHarga.Text);
            data.Add(ViewComponent.textboxStok.Text);

            if (ViewComponent.comboboxBerat.SelectedIndex == 0)
                data.Add(ViewComponent.textboxBerat.Text);
            else data.Add((Convert.ToInt32(ViewComponent.textboxBerat.Text) * 1000).ToString());
            data.Add(ViewComponent.textboxDeskripsi.Text);

            return data;
        }

        private bool dataValidation(List<string> data) {
            foreach (string item in data)
                if (item == "") {
                    Console.WriteLine("ERROR! data invalid");
                    return false;
                }
            return imagePath != "";
        }

        private void resetPageProduk() {
            ViewComponent.textboxNamaProduk.Text = "";
            ViewComponent.textboxHarga.Text = "";
            ViewComponent.textboxStok.Text = "";
            ViewComponent.textboxBerat.Text = "";
            ViewComponent.textboxDeskripsi.Text = "";
            ViewComponent.comboboxKategori.SelectedIndex = -1;
            ViewComponent.comboboxBerat.SelectedIndex = 0;
            ViewComponent.checkboxStatusProduk.IsChecked = false;
            ViewComponent.datagridProduk.SelectedIndex = -1;
            ViewComponent.imageProduk.Source = null;
            ViewComponent.checkboxStatusProduk.IsEnabled = true;
            imagePath = "";
            fillDgvProduk();
        }
    }
}
