﻿using System;
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

namespace Tukupedia.ViewModels {
    public static class SellerViewModel {
        public enum page { Pesanan, Produk, InfoToko }

        private static SellerView ViewComponent;
        private static Transition transition;
        private const int transFPS = 50;
        private const double speedMargin = 0.3;
        private const double speedOpacity = 0.4;
        private const double multiplier = 80;

        private static DataRow seller = Session.User;

        private static CategoryModel categoryModel = new CategoryModel();
        private static ItemModel itemModel = new ItemModel();
        private static SellerModel sellerModel = new SellerModel();

        public static void InitializeView(SellerView view) {
            TESTING();
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
            ViewComponent.labelNamaToko.Content = seller["NAMA_TOKO"].ToString();
            ViewComponent.labelNamaPenjual.Content = seller["NAMA_SELLER"].ToString();
            ViewComponent.labelSaldo.Content = "Rp " + seller["SALDO"].ToString();
        }

        public static void swapTo(page a) {
            if (a == page.Pesanan) {
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

            if (a == page.Produk) {
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

            if (a == page.InfoToko) {
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
        // Page Pesanan
        public static void initPagePesanan() {

        }

        // Page Produk
        public static void initPageProduk() {
            itemModel.loadDataProduk(Convert.ToInt32(seller["ID"].ToString()));
            ViewComponent.datagridProduk.ItemsSource = "";
            ViewComponent.datagridProduk.ItemsSource = itemModel.datagridTable.DefaultView;

            foreach (DataRow item in categoryModel.Table.Rows) {
                ViewComponent.comboboxKategori.Items.Add(item);
            }
            ViewComponent.comboboxKategori.DisplayMemberPath = "NAMA";
            ViewComponent.comboboxKategori.SelectedValuePath = "ID";
        }

        private static bool inputValidation(string[] data) {
            foreach (string item in data) {
                if (item == "") return false;
            }
            return true;
        }

        private static string[] getInputDataProduk() {
            string nama = ViewComponent.textboxNamaProduk.Text;
            string kategori = ViewComponent.comboboxKategori.SelectedItem.ToString();
            string harga = ViewComponent.textboxHarga.Text;
            string stok = ViewComponent.textboxStok.Text;
            string berat = ViewComponent.textboxBerat.Text;
            string deskripsi = ViewComponent.textboxDeskripsi.Text;

            string[] data = { "0", "", kategori, harga, "1", nama, deskripsi, seller["ID"] + "", berat, stok, "0", "", DateTime.Now + "" };
            return data;
        }
        
        public static void insertProduk() {
            string[] data = getInputDataProduk();
            inputValidation(data);
            itemModel.insert(data);
        }

    }
}