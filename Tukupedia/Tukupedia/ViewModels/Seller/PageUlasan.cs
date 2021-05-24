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
    public class PageUlasan {
        private SellerView ViewComponent;
        private UlasanModel ulasanModel;
        private DataRow seller;

        public PageUlasan(SellerView viewComponent, DataRow seller) {
            ViewComponent = viewComponent;
            this.seller = seller;
            initPageUlasan();
        }

        public void initPageUlasan() {
            fillDgvUlasan();
            ViewComponent.btnBalasUlasan.IsEnabled = false;
            ViewComponent.btnCancelUlasan.IsEnabled = false;
        }

        public void selectUlasan() {
            int selectedIndex = ViewComponent.datagridUlasan.SelectedIndex;
            if (selectedIndex == -1) return;

            DataRow row = new DB("ULASAN").select().where("ID", ulasanModel.Table.Rows[selectedIndex][0].ToString()).getFirst();
            if (row == null) return;

            ViewComponent.ratingbarUlasan.Value = Convert.ToInt32(row["RATING"].ToString());
            ViewComponent.textboxIsiUlasan.Text = row["MESSAGE"].ToString();
            ViewComponent.textboxBalasUlasan.Text = row["REPLY"].ToString();
            ViewComponent.btnBalasUlasan.IsEnabled = true;
            ViewComponent.textboxBalasUlasan.IsEnabled = true;
        }

        public void checkTextboxUlasan() {
            string text = ViewComponent.textboxBalasUlasan.Text;
            if (text != "") {
                ViewComponent.btnBalasUlasan.IsEnabled = true;
                ViewComponent.btnCancelUlasan.IsEnabled = true;
            }
            else {
                ViewComponent.btnBalasUlasan.IsEnabled = false;
                ViewComponent.btnCancelUlasan.IsEnabled = false;
            }
        }

        public void searchUlasan() {
            string keyword = ViewComponent.textboxCariUlasan.Text;
            if (keyword != "") fillDgvUlasan(keyword);
            else fillDgvUlasan();
        }

        public void sortUlasan() {
            int selectedIndex = ViewComponent.comboboxSortUlasan.SelectedIndex;

            if (selectedIndex == 0) ulasanModel.Table.DefaultView.Sort = "KODE TRANSAKSI asc";
            if (selectedIndex == 1) ulasanModel.Table.DefaultView.Sort = "KODE TRANSAKSI desc";
            if (selectedIndex == 2) ulasanModel.Table.DefaultView.Sort = "NAMA CUSTOMER asc";
            if (selectedIndex == 3) ulasanModel.Table.DefaultView.Sort = "NAMA CUSTOMER desc";
            if (selectedIndex == 4) ulasanModel.Table.DefaultView.Sort = "RATING asc";
            if (selectedIndex == 5) ulasanModel.Table.DefaultView.Sort = "RATING desc";
        }

        public void replyUlasan() {
            int selectedIndex = ViewComponent.datagridUlasan.SelectedIndex;
            if (selectedIndex == -1) return;

            string reply = ViewComponent.textboxBalasUlasan.Text;
            if (reply == "") return;

            UlasanModel model = new UlasanModel();
            model.init();
            model.addWhere("ID", ulasanModel.Table.Rows[selectedIndex][0].ToString());
            foreach (DataRow row in model.get()) {
                model.updateRow(row, "REPLY", reply);
            }
            reset();
        }

        public void reset() {
            ViewComponent.datagridUlasan.SelectedIndex = -1;
            ViewComponent.textboxIsiUlasan.Text = "";
            ViewComponent.textboxBalasUlasan.Text = "";
            ViewComponent.btnBalasUlasan.IsEnabled = false;
            ViewComponent.textboxBalasUlasan.IsEnabled = false;
            fillDgvUlasan();
        }

        private void fillDgvUlasan() {
            string statement = $"SELECT " +
                $"U.ID as \"ID\", " +
                $"h.KODE as \"KODE TRANSAKSI\", " +
                $"c.NAMA as \"NAMA CUSTOMER\", " +
                $"u.RATING as \"RATING\" " +
                $"FROM ULASAN u, CUSTOMER c, D_TRANS_ITEM d, H_TRANS_ITEM h " +
                $"WHERE u.ID_CUSTOMER = c.ID " +
                $"and u.ID_D_TRANS_ITEM = d.ID " +
                $"and d.ID_H_TRANS_ITEM = h.ID " +
                $"and u.ID_SELLER = '{seller["ID"]}'";

            ulasanModel = new UlasanModel();
            ulasanModel.initAdapter(statement);
            ViewComponent.datagridUlasan.ItemsSource = "";
            ViewComponent.datagridUlasan.ItemsSource = ulasanModel.Table.DefaultView;
            ViewComponent.datagridUlasan.Columns[0].Visibility = Visibility.Hidden;
        }

        private void fillDgvUlasan(string keyword) {
            string statement = $"SELECT " +
                $"U.ID as \"ID\", " +
                $"h.KODE as \"KODE TRANSAKSI\", " +
                $"c.NAMA as \"NAMA PEMBELI\", " +
                $"u.RATING as \"RATING\" " +
                $"FROM ULASAN u, CUSTOMER c, D_TRANS_ITEM d, H_TRANS_ITEM h " +
                $"WHERE u.ID_CUSTOMER = c.ID " +
                $"and u.ID_D_TRANS_ITEM = d.ID " +
                $"and d.ID_H_TRANS_ITEM = h.ID " +
                $"and u.ID_SELLER = '{seller["ID"]}' " +
                $"and ( " +
                $"h.KODE = '{keyword}' or " +
                $"c.NAMA = '{keyword}')";

            ulasanModel = new UlasanModel();
            ulasanModel.initAdapter(statement);
            ViewComponent.datagridUlasan.ItemsSource = "";
            ViewComponent.datagridUlasan.ItemsSource = ulasanModel.Table.DefaultView;
            ViewComponent.datagridUlasan.Columns[0].Visibility = Visibility.Hidden;
        }

        private void fillCbSortUlasan() {
            ViewComponent.comboboxSortUlasan.Items.Clear();
            ViewComponent.comboboxSortUlasan.Items.Add("Kode Transaksi asc");
            ViewComponent.comboboxSortUlasan.Items.Add("Kode Transaksi desc");
            ViewComponent.comboboxSortUlasan.Items.Add("Customer asc");
            ViewComponent.comboboxSortUlasan.Items.Add("Customer desc");
            ViewComponent.comboboxSortUlasan.Items.Add("Rating asc");
            ViewComponent.comboboxSortUlasan.Items.Add("Rating desc");
        }

    }
}
