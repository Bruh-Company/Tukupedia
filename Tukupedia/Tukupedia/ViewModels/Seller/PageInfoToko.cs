﻿using Tukupedia.Models;
using Tukupedia.Views.Seller;
using Tukupedia.Helpers.DatabaseHelpers;
using System.Windows;
using System.Data;
using System;
using System.Collections.Generic;
using Oracle.DataAccess.Client;
using Tukupedia.Helpers.Utils;
using Tukupedia.Views;

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
        }

        public void addKurir() {
            int id = ViewComponent.comboboxKurirInfo.SelectedIndex;
            if (id == -1) return;
            string idKurir = ViewComponent.comboboxKurirInfo.SelectedValue.ToString();
            KurirModel model = new KurirModel();
            model.init();
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
            if (ViewComponent.listboxListKurirInfo.SelectedIndex == -1) return;
            lbKurirTable.Rows.RemoveAt(ViewComponent.listboxListKurirInfo.SelectedIndex);
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
            else ImageHelper.loadImage(ViewComponent.imageToko, seller["IMAGE"].ToString());
        }


        // Private Void
        private void resetInfo() {
            seller = new DB("SELLER").select().where("ID", seller["ID"].ToString()).getFirst();
            imagePath = seller["IMAGE"].ToString();
            ViewComponent.labelNamaToko.Content = seller["NAMA_TOKO"].ToString();
            lbKurirTable = new DB("KURIR_SELLER").select().join("KURIR", "KURIR_SELLER", "ID_KURIR", "=", "ID").get();

            initImageToko();
            fillTextbox();
            reloadImageInfo();
            resetKurir();
        }

        private void resetKurir() {
            fillLbKurir();
            fillCmbKurir();
        }

        private void saveKurirSeller() {
            Kurir_SellerModel model = new Kurir_SellerModel();
            model.init();
            model.addWhere("ID_SELLER", seller["ID"].ToString());
            foreach (DataRow row in model.get()) {
                new DB("KURIR_SELLER").delete(seller["ID"].ToString()).execute();
            }

            foreach (DataRow row in lbKurirTable.Rows) {
                DataRow target = new DB("KURIR_SELLER").select("count(*)").where("ID_SELLER", seller["ID"].ToString()).where("ID_KURIR", row["ID"].ToString()).getFirst();
                if (target[0].ToString() == "0") {
                    model.insert("ID_SELLER", seller["ID"].ToString(), "ID_KURIR", row["ID"].ToString());
                }
            }
        }

        private void saveDataSeller() {
            List<string> data = getData();
            if (!dataValidation(data)) return;

            string idSeller = seller["ID"].ToString();

            SellerModel model = new SellerModel();
            model.init();
            model.addWhere("ID", idSeller, "=", false);
            foreach (DataRow row in model.get()) {
                string img = "";
                if (imagePath != "") img = ImageHelper.saveImage(imagePath, seller["KODE"].ToString(), ImageHelper.target.seller, true);
                model.updateRow(row, "NAMA_TOKO", data[0], "NAMA_SELLER", data[1], "EMAIL", data[2], "NO_TELP", data[3], "ALAMAT", data[4], "IMAGE", img);
            }
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
            else ImageHelper.loadImage(ViewComponent.imageInfo, seller["IMAGE"].ToString());
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
            KurirModel model = new KurirModel();
            model.init();

            cbKurirTable = new DataTable();
            cbKurirTable.Columns.Add("ID");
            cbKurirTable.Columns.Add("NAMA");
            
            for (int i = 0; i < model.Table.Rows.Count; i++) { 
                DataRow row = model.Table.Rows[i];
                bool valid = true;
                foreach (DataRow lbRow in lbKurirTable.Rows) {
                    string lbID = lbRow["ID"].ToString();
                    string tempID = row["ID"].ToString();

                    if (lbID == tempID) valid = false;
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

        public void ChangePassword()
        {
            ChangePasswordView cp = new ChangePasswordView("SELLER", seller["ID"].ToString());
            ViewComponent.Hide();
            cp.ShowDialog();
            ViewComponent.Show();

        }
    }
}
