using Tukupedia.Views;
using Tukupedia.Views.Seller;
using Tukupedia.Helpers.Utils;
using Tukupedia.Helpers.DatabaseHelpers;
using System.Windows;
using System.Data;
using System;
using System.Windows.Forms;
using System.IO;
using System.Windows.Media.Imaging;

namespace Tukupedia.ViewModels.Seller {
    public static class SellerViewModel {
        public enum page { Pesanan, Produk, InfoToko, Ulasan }
        public static PagePesanan pagePesanan;
        public static PageProduk pageProduk;
        public static PageUlasan pageUlasan;
        public static PageInfoToko pageInfoToko;

        private static SellerView ViewComponent;
        private static Transition transition;
        private const int transFPS = 50;
        private const double speedMargin = 0.3;
        private const double speedOpacity = 0.4;
        private const double multiplier = 80;
        private static DataRow seller;


        public static void InitializeView(SellerView view) {
            TESTING();
            seller = Session.User;
            ViewComponent = view;
            pagePesanan = new PagePesanan(view, seller);
            pageProduk = new PageProduk(view, seller);
            pageUlasan = new PageUlasan(view, seller);
            pageInfoToko = new PageInfoToko(view, seller);
            transition = new Transition(transFPS);
            initState();
            initHeader();
            pagePesanan.initPagePesanan();
        }

        public static void TESTING() {
            Session.User = new DB("SELLER").select().where("ID", "1").getFirst();
            Session.role = "SELLER";
        }

        public static void initState() {
            ViewComponent.canvasPesanan.Margin = MarginPosition.Middle;
            ViewComponent.canvasProduk.Margin = MarginPosition.Right;
            ViewComponent.canvasUlasan.Margin = MarginPosition.Right;
            ViewComponent.canvasInfoToko.Margin = MarginPosition.Right;

            ComponentHelper.changeVisibilityComponent(ViewComponent.canvasPesanan, Visibility.Visible);
            ComponentHelper.changeVisibilityComponent(ViewComponent.canvasProduk, Visibility.Hidden);
            ComponentHelper.changeVisibilityComponent(ViewComponent.canvasUlasan, Visibility.Hidden);
            ComponentHelper.changeVisibilityComponent(ViewComponent.canvasInfoToko, Visibility.Hidden);
        }

        public static void initHeader() {
            ViewComponent.labelNamaToko.Content = seller["NAMA_TOKO"].ToString();
            ViewComponent.labelSaldo.Content = "Rp " + Utility.formatNumber(Convert.ToInt32(seller["SALDO"].ToString()));

            if (seller["IS_OFFICIAL"].ToString() == "1") ViewComponent.labelStatusToko.Content = "OFFICIAL STORE";
            else ViewComponent.labelStatusToko.Content = "PEASANT MERCHANT";

            pageInfoToko.initImageToko();
        }
        
        public static void logout() {
            Session.Logout();
            //new LoginRegisterView().Show();
            ViewComponent.Close();
        }

        public static void swapTo(page p) {
            if (p == page.Pesanan) swapToPagePesanan();
            if (p == page.Produk) swapToPageProduk();
            if (p == page.Ulasan) swapToPageUlasan();
            if (p == page.InfoToko) swapToPageInfoToko();
        }

        public static void swapToPagePesanan() {
            transition.setCallback(pagePesanan.initPagePesanan);

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

            transition.makeTransition(ViewComponent.canvasUlasan,
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
                ViewComponent.canvasUlasan,
                Visibility.Hidden);
            ComponentHelper.changeZIndexComponent(
                ViewComponent.canvasInfoToko,
                Visibility.Hidden);
        }

        public static void swapToPageProduk() {
            transition.setCallback(pageProduk.initPageProduk);

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

            transition.makeTransition(ViewComponent.canvasUlasan,
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
                ViewComponent.canvasUlasan,
                Visibility.Hidden);
            ComponentHelper.changeZIndexComponent(
                ViewComponent.canvasInfoToko,
                Visibility.Hidden);
        }

        public static void swapToPageUlasan() {
            transition.setCallback(pageUlasan.initPageUlasan);

            transition.makeTransition(ViewComponent.canvasUlasan,
                MarginPosition.Middle, 1,
                speedMargin * multiplier / transFPS,
                speedOpacity * multiplier / transFPS,
                "with previous");

            transition.makeTransition(ViewComponent.canvasPesanan,
                MarginPosition.Right, 0,
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
                ViewComponent.canvasUlasan,
                Visibility.Visible);
            ComponentHelper.changeZIndexComponent(
                ViewComponent.canvasPesanan,
                Visibility.Hidden);
            ComponentHelper.changeZIndexComponent(
                ViewComponent.canvasProduk,
                Visibility.Hidden);
            ComponentHelper.changeZIndexComponent(
                ViewComponent.canvasInfoToko,
                Visibility.Hidden);
        }

        public static void swapToPageInfoToko() {
            transition.setCallback(pageInfoToko.initPageInfoToko);

            transition.makeTransition(ViewComponent.canvasInfoToko,
                MarginPosition.Middle, 1,
                speedMargin * multiplier / transFPS,
                speedOpacity * multiplier / transFPS,
                "with previous");

            transition.makeTransition(ViewComponent.canvasPesanan,
                MarginPosition.Right, 0,
                speedMargin * multiplier / transFPS,
                speedOpacity * multiplier / transFPS,
                "with previous");

            transition.makeTransition(ViewComponent.canvasProduk,
                MarginPosition.Right, 0,
                speedMargin * multiplier / transFPS,
                speedOpacity * multiplier / transFPS,
                "with previous");

            transition.makeTransition(ViewComponent.canvasUlasan,
                MarginPosition.Right, 0,
                speedMargin * multiplier / transFPS,
                speedOpacity * multiplier / transFPS,
                "with previous");

            transition.playTransition();
            ComponentHelper.changeZIndexComponent(
                ViewComponent.canvasInfoToko,
                Visibility.Visible);
            ComponentHelper.changeZIndexComponent(
                ViewComponent.canvasPesanan,
                Visibility.Hidden);
            ComponentHelper.changeZIndexComponent(
                ViewComponent.canvasProduk,
                Visibility.Hidden);
            ComponentHelper.changeZIndexComponent(
                ViewComponent.canvasUlasan,
                Visibility.Hidden);
        }
    }
}
