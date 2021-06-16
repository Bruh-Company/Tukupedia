using Tukupedia.Models;
using Tukupedia.Views.Seller;
using Tukupedia.Helpers.DatabaseHelpers;
using System.Windows;
using System.Data;
using System;
using System.Collections.Generic;
using Oracle.DataAccess.Client;
using Tukupedia.Helpers.Utils;
using Tukupedia.Views;
using System.Windows.Media;

namespace Tukupedia.ViewModels.Seller {
    public class PageInfoToko {
        private SellerView ViewComponent;
        private SellerModel sellerModel;
        private DataRow seller;
        private DataTable lbKurirTable;
        private DataTable cbKurirTable;
        private bool toggleChange = false;
        private string imagePath;

        public PageInfoToko(SellerView viewComponent, DataRow seller) {
            ViewComponent = viewComponent;
            this.seller = seller;
            initPageInfoToko();
        }

        public void initPageInfoToko() {
            sellerModel = new SellerModel();
            sellerModel.init();
            resetInfo();
            changeState();
        }

        public void toggleState() {
            toggleChange = !toggleChange;
            changeState();
        }

        public void registerOS() {
            if (MessageBox.Show("Yakin untuk daftar sebagai Official Store?", "Konfirmasi", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes) {
                App.openConnection(out _);
                using (OracleTransaction trans = App.connection.BeginTransaction()) {
                    try {
                        Trans_OSModel model = new Trans_OSModel();
                        model.init();
                        model.insert("ID", 0, "KODE", "", "TANGGAL_TRANSAKSI", DateTime.Now, "STATUS", 'R', "ID_SELLER", seller["ID"].ToString());
                        trans.Commit();
                    }
                    catch (OracleException) {
                        trans.Rollback();
                    }
                }
                App.closeConnection(out _);
            }
            resetOSMessage();
        }

        public void addKurir() {
            int id = ViewComponent.comboboxKurirInfo.SelectedIndex;
            if (id == -1) return;

            string idKurir = ViewComponent.comboboxKurirInfo.SelectedValue.ToString();
            KurirModel model = new KurirModel();
            model.addWhere("ID", idKurir, "=", false);

            foreach (DataRow row in model.get()) {
                DataRow newKurir = lbKurirTable.NewRow();
                newKurir["ID"] = row["ID"];
                newKurir["NAMA"] = row["NAMA"];
                lbKurirTable.Rows.Add(newKurir);
            }
            resetKurir();
        }

        public void deleteKurir() {
            int index = ViewComponent.listboxListKurirInfo.SelectedIndex;
            if (index == -1) return;
            lbKurirTable.Rows.RemoveAt(index);
            resetKurir();
        }

        public void saveInfo() {
            saveDataSeller();
            saveKurirSeller();
            toggleState();
            resetInfo();
        }

        public void cancelInfo() {
            toggleState();
            resetInfo();
        }

        public void changeTokoPic() {
            imagePath = ImageHelper.openFileDialog(ViewComponent.imageInfo);
        }

        public void initImageToko() {
            if (seller["IMAGE"].ToString() == "") ImageHelper.loadImageCheems(ViewComponent.imageToko);
            else ImageHelper.loadImage(ViewComponent.imageToko, seller["IMAGE"].ToString(), ImageHelper.target.seller);
        }


        // Private Void
        private void resetInfo() {
            seller = new DB("SELLER").select().where("ID", seller["ID"].ToString()).getFirst();

            ViewComponent.labelNamaToko.Content = seller["NAMA_TOKO"].ToString();


            lbKurirTable = new DB("KURIR_SELLER")
                .select("KURIR.ID as ID", "KURIR.NAMA as NAMA")
                .join("KURIR", "KURIR_SELLER", "ID_KURIR", "=", "ID")
                .where("ID_SELLER", seller["ID"].ToString())
                .get();

            initImageToko();
            fillTextbox();
            reloadImageInfo();
            resetKurir();
            resetOSMessage();
        }

        private void resetOSMessage() {
            ViewComponent.labelOSMessage.Visibility = Visibility.Hidden;
            ViewComponent.btnDaftarOS.Visibility = Visibility.Hidden;

            Trans_OSModel model = new Trans_OSModel();
            model.addWhere($"ID_SELLER", seller["ID"].ToString(), "=", false);
            model.addOrderBy("TANGGAL_TRANSAKSI desc");
            if (model.get().Length <= 0) return;
            DataRow row = model.get()[0];
            if (row == null) return;
            ViewComponent.labelOSMessage.Visibility = Visibility.Visible;
            ViewComponent.btnDaftarOS.Visibility = Visibility.Visible;
            if (row["STATUS"].ToString() == "R") {
                ViewComponent.labelOSMessage.Text = "Pendaftaran Official Store masih diproses";
                Color color = (Color)ColorConverter.ConvertFromString("#FFC548");
                ViewComponent.labelOSMessage.Foreground = new SolidColorBrush(color);
                ViewComponent.btnDaftarOS.IsEnabled = false;
            }
            else if (row["STATUS"].ToString() == "D") {
                ViewComponent.labelOSMessage.Text = "Pendaftaran Official Store ditolak";
                Color color = (Color)ColorConverter.ConvertFromString("#E23434");
                ViewComponent.labelOSMessage.Foreground = new SolidColorBrush(color);
                ViewComponent.btnDaftarOS.IsEnabled = true;
            }
            else {
                ViewComponent.labelOSMessage.Visibility = Visibility.Hidden;
                ViewComponent.btnDaftarOS.Visibility = Visibility.Hidden;
            }
        }

        private void resetKurir() {
            fillLbKurir();
            fillCmbKurir();
        }

        private void saveKurirSeller() {
            string sellerID = seller["ID"].ToString();

            Kurir_SellerModel model = new Kurir_SellerModel();
            model.addWhere("ID_SELLER", sellerID, "=", false);

            foreach (DataRow row in model.get()) {
                new DB("KURIR_SELLER").delete(row["ID"].ToString()).execute();
            }

            foreach (DataRow row in lbKurirTable.Rows) {
                insertKurir(sellerID, row["ID"].ToString());
            }
        }

        private void insertKurir(string sellerID, string kurirID) {
            Kurir_SellerModel model = new Kurir_SellerModel();
            model.insert(
                "ID_SELLER", sellerID,
                "ID_KURIR", kurirID
                );
        }

        private void saveDataSeller() {
            List<string> data = getData();
            Admin.SellerViewModel asvm = new Admin.SellerViewModel();
            if (Session.User["EMAIL"].ToString() == data[2]) ;
            else if (!asvm.checkEmail(data[2]))
            {
                return;
            }
            string idSeller = seller["ID"].ToString();

            SellerModel model = new SellerModel();
            model.init();
            model.addWhere("ID", idSeller, "=", false);
            DataRow row = model.Table.Select($"ID = '{idSeller}'")[0];
            string img = "";
            if (imagePath != "")
            {
                img = ImageHelper.saveImage(imagePath, seller["KODE"].ToString(), ImageHelper.target.seller, true);
                row["IMAGE"] = img;
            }
            row["NAMA_TOKO"] = data[0];
            row["NAMA_SELLER"] = data[1];
            row["EMAIL"] = data[2];
            row["NO_TELP"] = data[3];
            row["ALAMAT"] = data[4];
            model.update();
        }

        private void changeState() {
            if (toggleChange) {
                ViewComponent.textboxNamaToko.IsReadOnly = false;
                ViewComponent.textboxNamaPenjual.IsReadOnly = false;
                ViewComponent.textboxEmailInfo.IsReadOnly = false;
                ViewComponent.textboxNoTelpInfo.IsReadOnly = false;
                ViewComponent.textboxAlamatInfo.IsReadOnly = false;

                ViewComponent.btnUbahInfoPenjual.Visibility = Visibility.Hidden;
                ViewComponent.btnSimpanInfo.Visibility = Visibility.Visible;
                ViewComponent.btnBatalInfo.Visibility = Visibility.Visible;
                ViewComponent.comboboxKurirInfo.Visibility = Visibility.Visible;
                ViewComponent.btnTambahKurirInfo.Visibility = Visibility.Visible;
                ViewComponent.btnKurangKurirInfo.Visibility = Visibility.Visible;
                ViewComponent.btnChangeImage.Visibility = Visibility.Visible;
                ViewComponent.btnChangePassword.Visibility = Visibility.Visible;
            }
            else {
                ViewComponent.textboxNamaToko.IsReadOnly = true;
                ViewComponent.textboxNamaPenjual.IsReadOnly = true;
                ViewComponent.textboxEmailInfo.IsReadOnly = true;
                ViewComponent.textboxNoTelpInfo.IsReadOnly = true;
                ViewComponent.textboxAlamatInfo.IsReadOnly = true;

                ViewComponent.btnUbahInfoPenjual.Visibility = Visibility.Visible;
                ViewComponent.btnSimpanInfo.Visibility = Visibility.Hidden;
                ViewComponent.btnBatalInfo.Visibility = Visibility.Hidden;
                ViewComponent.comboboxKurirInfo.Visibility = Visibility.Hidden;
                ViewComponent.btnTambahKurirInfo.Visibility = Visibility.Hidden;
                ViewComponent.btnKurangKurirInfo.Visibility = Visibility.Hidden;
                ViewComponent.btnChangeImage.Visibility = Visibility.Hidden;
                ViewComponent.btnChangePassword.Visibility = Visibility.Hidden;


            }
        }

        private void reloadImageInfo() {
            if (seller["IMAGE"].ToString() == "") ImageHelper.loadImageCheems(ViewComponent.imageInfo);
            else { 
                ImageHelper.loadImage(ViewComponent.imageInfo, seller["IMAGE"].ToString(), ImageHelper.target.seller);
                imagePath = ImageHelper.getSellerImagePath(seller["IMAGE"].ToString());
            }
        }

        private void fillTextbox() {
            ViewComponent.textboxNamaToko.Text = seller["NAMA_TOKO"].ToString();
            ViewComponent.textboxNamaPenjual.Text = seller["NAMA_SELLER"].ToString();
            ViewComponent.textboxEmailInfo.Text = seller["EMAIL"].ToString();
            ViewComponent.textboxNoTelpInfo.Text = seller["NO_TELP"].ToString();
            ViewComponent.textboxAlamatInfo.Text = seller["ALAMAT"].ToString();
            ViewComponent.labelBukaSejak.Content = DateTime.Parse(seller["CREATED_AT"].ToString()).ToString("dd MMMM yyyy");
        }

        private void fillCmbKurir() {
            cbKurirTable = new DataTable();
            cbKurirTable.Columns.Add("ID");
            cbKurirTable.Columns.Add("NAMA");

            KurirModel model = new KurirModel();
            for (int i = 0; i < model.Table.Rows.Count; i++) { 
                DataRow row = model.Table.Rows[i];

                bool valid = true;
                foreach (DataRow lbRow in lbKurirTable.Rows) {
                    string lbID = lbRow["ID"].ToString();
                    string tempID = row["ID"].ToString();
                    if (lbID == tempID) {
                        valid = false;
                    }
                }
                if (valid) {
                    DataRow newRow = cbKurirTable.NewRow();
                    newRow["ID"] = row["ID"].ToString();
                    newRow["NAMA"] = row["NAMA"].ToString();
                    cbKurirTable.Rows.Add(newRow);
                }
            }

            ViewComponent.comboboxKurirInfo.ItemsSource = "";
            ViewComponent.comboboxKurirInfo.ItemsSource = cbKurirTable.DefaultView;
            ViewComponent.comboboxKurirInfo.DisplayMemberPath = "NAMA";
            ViewComponent.comboboxKurirInfo.SelectedValuePath = "ID";
        }

        private void fillLbKurir() {
            ViewComponent.listboxListKurirInfo.ItemsSource = "";
            ViewComponent.listboxListKurirInfo.ItemsSource = lbKurirTable.DefaultView;
            ViewComponent.listboxListKurirInfo.DisplayMemberPath = "NAMA";
            ViewComponent.listboxListKurirInfo.SelectedValuePath = "ID";
        }

        private List<string> getData() {
            List<string> data = new List<string>();
            data.Add(ViewComponent.textboxNamaToko.Text.ToUpper());
            data.Add(ViewComponent.textboxNamaPenjual.Text.ToUpper());
            data.Add(ViewComponent.textboxEmailInfo.Text);
            data.Add(ViewComponent.textboxNoTelpInfo.Text);
            data.Add(ViewComponent.textboxAlamatInfo.Text);
            return data;
        }

        private bool dataValidation(List<string> data) {
            foreach (string item in data)
                if (item == "") return false;
            return true;
        }

        public void ChangePassword() {
            ChangePasswordView cp = new ChangePasswordView("SELLER", seller["ID"].ToString());
            cp.ShowDialog();
        }
    }
}
