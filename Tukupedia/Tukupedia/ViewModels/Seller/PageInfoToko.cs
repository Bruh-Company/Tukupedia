using Tukupedia.Models;
using Tukupedia.Views.Seller;
using Tukupedia.Helpers.DatabaseHelpers;
using System.Windows;
using System.Data;
using System.Windows.Media;
using System.Windows.Controls;
using System;

namespace Tukupedia.ViewModels.Seller {
    public class PageInfoToko {
        private SellerView ViewComponent;
        private SellerModel sellerModel;
        private DataRow seller;
        private DataTable layananKurirTable;
        private bool toggleChange = false;

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

        public void addKurir() {
            int id = ViewComponent.comboboxKurirInfo.SelectedIndex;
            if (id == -1) return;
            string idKurir = ViewComponent.comboboxKurirInfo.SelectedValue.ToString();
            KurirModel model = new KurirModel();
            model.init();
            model.addWhere("ID", idKurir, "=", false);
            foreach (DataRow row in model.get()) {
                DataRow newKurir = layananKurirTable.NewRow();
                newKurir["ID"] = row["ID"];
                newKurir["NAMA"] = row["NAMA"];
                layananKurirTable.Rows.Add(newKurir);
            }
        }

        public void deleteKurir() {
            int id = ViewComponent.listboxListKurirInfo.SelectedIndex;
            if (id == -1) return;
            layananKurirTable.Rows.RemoveAt(id);
        }

        public void toggleState() {
            toggleChange = !toggleChange;
            changeState();
        }

        public void saveInfo() {
            string[] data = getData();
            if (!dataValidation(data)) return;

            string idSeller = seller["ID"].ToString();

            SellerModel sellerModel = new SellerModel();
            sellerModel.init();
            sellerModel.addWhere("ID", idSeller, "=", false);
            foreach (DataRow row in sellerModel.get()) {
                sellerModel.updateRow(row, "NAMA_TOKO", data[0], "NAMA_SELLER", data[1], "EMAIL", data[2], "NO_TELP", data[3], "ALAMAT", data[4]);
            }

            Kurir_SellerModel ksModel = new Kurir_SellerModel();
            ksModel.init();
            ksModel.addWhere("ID_SELLER", idSeller, "=", false);
            foreach (DataRow row in ksModel.get()) {
                ksModel.delete(row);
            }
            ksModel.resetWhere();

            foreach (DataRow row in layananKurirTable.Rows) {
                ksModel.insert("ID_SELLER", idSeller, "ID_KURIR", row["ID"]);
            }

            toggleState();
            resetInfo();
        }

        public void cancelInfo() {
            toggleState();
            resetInfo();
        }

        // Private Void
        private void resetInfo() {
            string id = seller["ID"].ToString();
            seller = new DB("SELLER").select().where("ID", id).getFirst();
            fillTextbox();
            fillCmbKurir();
            fillLbKurir();
        }

        private void changeState() {
            if (toggleChange) {
                ViewComponent.textboxNamaToko.IsReadOnly = false;
                ViewComponent.textboxNamaPenjual.IsReadOnly = false;
                ViewComponent.textboxEmailInfo.IsReadOnly = false;
                ViewComponent.textboxNoTelpInfo.IsReadOnly = false;
                ViewComponent.textboxAlamatInfo.IsReadOnly = false;
                ViewComponent.btnSimpanInfo.Visibility = Visibility.Visible;
                ViewComponent.btnBatalInfo.Visibility = Visibility.Visible;
                ViewComponent.btnUbahInfoPenjual.Visibility = Visibility.Hidden;
            }
            else {
                ViewComponent.textboxNamaToko.IsReadOnly = true;
                ViewComponent.textboxNamaPenjual.IsReadOnly = true;
                ViewComponent.textboxEmailInfo.IsReadOnly = true;
                ViewComponent.textboxNoTelpInfo.IsReadOnly = true;
                ViewComponent.textboxAlamatInfo.IsReadOnly = true;
                ViewComponent.btnSimpanInfo.Visibility = Visibility.Hidden;
                ViewComponent.btnBatalInfo.Visibility = Visibility.Hidden;
                ViewComponent.btnUbahInfoPenjual.Visibility = Visibility.Visible;
            }
        }

        private void fillTextbox() {
            ViewComponent.textboxNamaToko.Text = seller["NAMA_TOKO"].ToString();
            ViewComponent.labelBukaSejak.Content = DateTime.Parse(seller["CREATED_AT"].ToString()).ToString("dd MMMM yyyy");
            ViewComponent.textboxNamaPenjual.Text = seller["NAMA_SELLER"].ToString();
            ViewComponent.textboxEmailInfo.Text = seller["EMAIL"].ToString();
            ViewComponent.textboxNoTelpInfo.Text = seller["NO_TELP"].ToString();
            ViewComponent.textboxAlamatInfo.Text = seller["ALAMAT"].ToString();
        }

        private void fillCmbKurir() {
            KurirModel model = new KurirModel();
            ViewComponent.comboboxKurirInfo.ItemsSource = "";
            ViewComponent.comboboxKurirInfo.ItemsSource = model.Table.DefaultView;
            ViewComponent.comboboxKurirInfo.DisplayMemberPath = "NAMA";
            ViewComponent.comboboxKurirInfo.SelectedValuePath = "ID";
        }

        private void fillLbKurir() {
            layananKurirTable = new DataTable();
            layananKurirTable.Columns.Add("ID");
            layananKurirTable.Columns.Add("NAMA");
            Kurir_SellerModel model = new Kurir_SellerModel();
            model.init();
            model.addWhere("ID_SELLER", seller["ID"].ToString());
            foreach (DataRow row in model.get()) {
                DataRow data = new DB("KURIR").select("ID", "NAMA").where("ID", row["ID_KURIR"].ToString()).getFirst();
                layananKurirTable.Rows.Add(data);
            }
            ViewComponent.listboxListKurirInfo.ItemsSource = "";
            ViewComponent.listboxListKurirInfo.ItemsSource = layananKurirTable.DefaultView;
            ViewComponent.listboxListKurirInfo.DisplayMemberPath = "NAMA";
            ViewComponent.listboxListKurirInfo.SelectedValuePath = "ID";
        }

        private string[] getData() {
            string
                namaToko = ViewComponent.textboxNamaToko.Text,
                namaPenjual = ViewComponent.textboxNamaPenjual.Text,
                email = ViewComponent.textboxEmailInfo.Text,
                noTelp = ViewComponent.textboxNoTelpInfo.Text,
                alamat = ViewComponent.textboxAlamatInfo.Text;

            string[] data = { namaToko, namaPenjual, email, noTelp, alamat};
            return data;
        }

        private bool dataValidation(string[] data) {
            if (data.Length < 4) return false;
            foreach (string item in data) {
                if (item == "") {
                    MessageBox.Show("Insert Gagal");
                    return false;
                }
            }
            return true;
        }
    }
}
