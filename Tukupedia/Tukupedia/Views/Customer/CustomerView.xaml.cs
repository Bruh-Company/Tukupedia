using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Tukupedia.Helpers.Utils;
using Tukupedia.Models;
using Tukupedia.ViewModels.Customer;
using Tukupedia.ViewModels;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using Tukupedia.Helpers.Classes;
using System.Data;

namespace Tukupedia.Views.Customer
{
    /// <summary>
    /// Interaction logic for CustomerView.xaml
    /// </summary>
    public partial class CustomerView : Window
    {
        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private const int GWL_STYLE = -16;
        private const int WS_MAXIMIZEBOX = 0x10000;
        Transition transition;
        const double speedMargin = 0.2;
        const double speedOpacity = 0.3;
        const double multiplier = 80;
        const int transFPS = 100;
        public CustomerView()
        {
            InitializeComponent();
        }

        private void CustomerView_OnLoaded(object sender, RoutedEventArgs e)
        {
            
            //debugMode();
            labelWelcome.Content = "Welcome "+ Session.User["NAMA"].ToString();
            grid_Home.Margin = MarginPosition.Middle;
            grid_Cart.Margin = MarginPosition.Right;
            grid_Transactions.Margin = MarginPosition.Right;
            grid_Settings.Margin = MarginPosition.Right;
            transition = new Transition(FPS:transFPS);

            ComponentHelper.changeVisibilityComponent(grid_Home, Visibility.Visible);
            ComponentHelper.changeVisibilityComponent(grid_Cart, Visibility.Hidden);
            ComponentHelper.changeVisibilityComponent(grid_Transactions, Visibility.Hidden);
            ComponentHelper.changeVisibilityComponent(grid_Settings, Visibility.Hidden);

            //Init Customer View Model
            CustomerViewModel.initCustomerViewModel(this);
            CustomerViewModel.loadItems();
            CustomerViewModel.loadCategory(spCategory);

            //Init Cart
            CartViewModel.initCart(this);
            CartViewModel.loadCartItem();
            CartViewModel.initHargaCart(labelTotal,tbSubTotal);
            CartViewModel.initPaymentMethod(cbPaymentMethod);
            CartViewModel.initPromotion(cbPromotion, tbDiscount,tbErrorPromotion);
            CartViewModel.updateHarga(0,0,0);

            //Init Transaction Page
            TransactionViewModel.initTransaction(this);
            TransactionViewModel.initH_Trans();

            //Init Settings Page

            SettingViewModel.init(this);

            IntPtr hwnd = new WindowInteropHelper(sender as Window).Handle;
            int value = GetWindowLong(hwnd, GWL_STYLE);
            SetWindowLong(hwnd, GWL_STYLE, value & ~WS_MAXIMIZEBOX);
            //COBA SESUATU :)
            
           

        }

        void debugMode()
        {
            Session.Login(new CustomerModel().Table.Rows[0],"Customer");
        }
        
        private void SliderMax_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tbMaxPrice.Text = Utility.formatNumber(Convert.ToInt32(SliderMax.Value));
        }

        private void SliderMin_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tbMinPrice.Text = Utility.formatNumber(Convert.ToInt32(SliderMin.Value));
        }

        private void BtnLogout_OnClick(object sender, RoutedEventArgs e)
        {
            CartViewModel.saveCart();
            Session.Logout();
            this.Close();
        }

        //Transition Settings

        private void goToHome(object sender, RoutedEventArgs e)
        {
            transition.makeTransition(grid_Home,
                    MarginPosition.Middle, 1,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");
            transition.makeTransition(grid_Cart,
                MarginPosition.Right, 0,
                speedMargin * multiplier / transFPS,
                speedOpacity * multiplier / transFPS,
                "with previous");
            transition.makeTransition(grid_Transactions,
                MarginPosition.Right, 0,
                speedMargin * multiplier / transFPS,
                speedOpacity * multiplier / transFPS,
                "with previous");
            transition.makeTransition(grid_Settings,
                MarginPosition.Right, 0,
                speedMargin * multiplier / transFPS,
                speedOpacity * multiplier / transFPS,
                "with previous");

            transition.playTransition();

            ComponentHelper.changeZIndexComponent(
                grid_Home,
                Visibility.Visible);
            ComponentHelper.changeZIndexComponent(
                grid_Cart,
                Visibility.Hidden);
            ComponentHelper.changeZIndexComponent(
                grid_Transactions,
                Visibility.Hidden);
            ComponentHelper.changeZIndexComponent(
                grid_Settings,
                Visibility.Hidden);
        }

        private void goToCart(object sender, RoutedEventArgs e)
        {
            transition.makeTransition(grid_Home,
                    MarginPosition.Left, 0,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");
            transition.makeTransition(grid_Cart,
                MarginPosition.Middle, 1,
                speedMargin * multiplier / transFPS,
                speedOpacity * multiplier / transFPS,
                "with previous");
            transition.makeTransition(grid_Transactions,
                MarginPosition.Right, 0,
                speedMargin * multiplier / transFPS,
                speedOpacity * multiplier / transFPS,
                "with previous");
            transition.makeTransition(grid_Settings,
                MarginPosition.Right, 0,
                speedMargin * multiplier / transFPS,
                speedOpacity * multiplier / transFPS,
                "with previous");

            transition.playTransition();

            ComponentHelper.changeZIndexComponent(
                grid_Home,
                Visibility.Hidden);
            ComponentHelper.changeZIndexComponent(
                grid_Cart,
                Visibility.Visible);
            ComponentHelper.changeZIndexComponent(
                grid_Transactions,
                Visibility.Hidden);
            ComponentHelper.changeZIndexComponent(
                grid_Settings,
                Visibility.Hidden);
        }

        private void goToTransactions(object sender, RoutedEventArgs e)
        {
            transition.makeTransition(grid_Home,
                    MarginPosition.Left, 0,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");
            transition.makeTransition(grid_Cart,
                MarginPosition.Left, 0,
                speedMargin * multiplier / transFPS,
                speedOpacity * multiplier / transFPS,
                "with previous");
            transition.makeTransition(grid_Transactions,
                MarginPosition.Middle, 1,
                speedMargin * multiplier / transFPS,
                speedOpacity * multiplier / transFPS,
                "with previous");
            transition.makeTransition(grid_Settings,
                MarginPosition.Right, 0,
                speedMargin * multiplier / transFPS,
                speedOpacity * multiplier / transFPS,
                "with previous");

            transition.playTransition();

            ComponentHelper.changeZIndexComponent(
                grid_Home,
                Visibility.Hidden);
            ComponentHelper.changeZIndexComponent(
                grid_Cart,
                Visibility.Hidden);
            ComponentHelper.changeZIndexComponent(
                grid_Transactions,
                Visibility.Visible);
            ComponentHelper.changeZIndexComponent(
                grid_Settings,
                Visibility.Hidden);
        }

        private void goToSettings(object sender, RoutedEventArgs e)
        {
            SettingViewModel.reload();
            transition.makeTransition(grid_Home,
                    MarginPosition.Left, 0,
                    speedMargin * multiplier / transFPS,
                    speedOpacity * multiplier / transFPS,
                    "with previous");
            transition.makeTransition(grid_Cart,
                MarginPosition.Left, 0,
                speedMargin * multiplier / transFPS,
                speedOpacity * multiplier / transFPS,
                "with previous");
            transition.makeTransition(grid_Transactions,
                MarginPosition.Left, 0,
                speedMargin * multiplier / transFPS,
                speedOpacity * multiplier / transFPS,
                "with previous");
            transition.makeTransition(grid_Settings,
                MarginPosition.Middle, 1,
                speedMargin * multiplier / transFPS,
                speedOpacity * multiplier / transFPS,
                "with previous");

            transition.playTransition();

            ComponentHelper.changeZIndexComponent(
                grid_Home,
                Visibility.Hidden);
            ComponentHelper.changeZIndexComponent(
                grid_Cart,
                Visibility.Hidden);
            ComponentHelper.changeZIndexComponent(
                grid_Transactions,
                Visibility.Hidden);
            ComponentHelper.changeZIndexComponent(
                grid_Settings,
                Visibility.Visible);
        }

        private void btnProceedToCheckout_Click(object sender, RoutedEventArgs e)
        {
            CartViewModel.proceedToCheckout();
        }

        private void cbPromotion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Promo p = (Promo) cbPromotion.SelectedItem;
            CartViewModel.promo = p;
            tbDesc.Text = p.getDescription();
            CartViewModel.checkPromotion(CartViewModel.promo);
        }

        private void cbPaymentMethod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CartViewModel.checkPromotion(CartViewModel.promo);
        }

        private void btnUbahInfoCustomer_Click(object sender, RoutedEventArgs e)
        {
            SettingViewModel.editing(true);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SettingViewModel.update();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            SettingViewModel.reload();
        }

        private void btnChangeImage_Click(object sender, RoutedEventArgs e)
        {
            SettingViewModel.changeImage();
        }




        
        //Transaction Events
        private void grid_H_Trans_MouseUp(object sender, MouseButtonEventArgs e)
        {
            TransactionViewModel.idxH_Trans = grid_H_Trans.SelectedIndex;
            TransactionViewModel.loadD_Trans();
        }

        
        private void btnBayarTrans_Click(object sender, RoutedEventArgs e)
        {
            TransactionViewModel.bayarTrans();
        }

        private void btnCancelTrans_Click(object sender, RoutedEventArgs e)
        {
            TransactionViewModel.cancelTrans();
        }

        private void btnCetakInvoice_Click(object sender, RoutedEventArgs e)
        {
            TransactionViewModel.generateInvoice();
        }
        private void grid_D_Trans_MouseUp(object sender, MouseButtonEventArgs e)
        {
            TransactionViewModel.idxItem = grid_D_Trans.SelectedIndex;
            TransactionViewModel.loadItem();
        }

        private void btnBeriUlasan_Click(object sender, RoutedEventArgs e)
        {
            TransactionViewModel.beriUlasan();
        }
        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswordView cp = new ChangePasswordView("CUSTOMER", Session.User["ID"].ToString());
            cp.ShowDialog();
            this.Show();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e) {
            CustomerViewModel.filterItems();
        }
        private void btnFilter_Click(object sender, RoutedEventArgs e) {
            CustomerViewModel.filterItems();
        }

        private void tbMaxPrice_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            Utility.NumberValidationTextBox(sender, e);
        }

        private void tbMinPrice_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            Utility.NumberValidationTextBox(sender, e);
        }

        private void rtbUlasan_GotFocus(object sender, RoutedEventArgs e)
        {
            string val = Utility.StringFromRichTextBox(rtbUlasan);
            if (val.Contains("Tulis Ulasan Disini"))
            {
                Utility.setRichTextBoxString(rtbUlasan, "");
            }
        }

        private void rtbUlasan_LostFocus(object sender, RoutedEventArgs e)
        {
            string val = Utility.StringFromRichTextBox(rtbUlasan);
            if (val.Length <= 2)
            {
                Utility.setRichTextBoxString(rtbUlasan, "Tulis Ulasan Disini");
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }

        private void btnTerimaBarang_Click(object sender, RoutedEventArgs e)
        {
            TransactionViewModel.terimaBarang();
        }
    }
}
