using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Tukupedia.Helpers.Utils;
using Tukupedia.ViewModels.Customer;

namespace Tukupedia.Components
{
    public class CartComponent:Card
    {
        private StackPanel spMain;
        private StackPanel spContent;
        private StackPanel spItem;
        private StackPanel spDesc;
        private StackPanel spCondition;

        private CheckBox checkBox;
        private Image itemImage;
        private TextBlock tbnamaItem;
        private TextBlock tbHarga;

        private Button btnDelete;
        private Button btnMin;
        private TextBlock tbQty;
        private Button btnPlus;
        private DataRow item;
        private int qty;
        private int subtotal;
        private ShopCartComponent parent;
        public CartComponent(ShopCartComponent parent)
        {
            this.parent = parent;
            spMain = new StackPanel();
            spContent = new StackPanel();
            spItem = new StackPanel();
            spDesc = new StackPanel();
            spCondition = new StackPanel();

            checkBox = new CheckBox();
            itemImage = new Image();
            tbnamaItem = new TextBlock();
            tbHarga = new TextBlock();
            btnDelete = new Button();
            btnMin = new Button();
            tbQty = new TextBlock();
            btnPlus = new Button();
            subtotal = 0;
            
            spDesc.Children.Add(tbnamaItem);
            spDesc.Children.Add(tbHarga);

            spItem.Orientation = Orientation.Horizontal;
            spItem.Children.Add(itemImage);
            spItem.Children.Add(spDesc);
            
            spCondition.Children.Add(btnDelete);
            spCondition.Children.Add(btnMin);
            spCondition.Children.Add(tbQty);
            spCondition.Children.Add(btnPlus);
            spCondition.Orientation = Orientation.Horizontal;
            
            spContent.Children.Add(spItem);
            spContent.Children.Add(spCondition);

            spMain.Orientation = Orientation.Horizontal;
            spMain.Children.Add(checkBox);
            spMain.Children.Add(spContent);
            
            //Event 
            btnDelete.Click+=BtnDeleteOnClick;
            btnMin.Click+=BtnMinOnClick;
            btnPlus.Click+=BtnPlusOnClick;
            
            //Styles
            itemImage.Source =
                new BitmapImage(new Uri(
                    AppDomain.CurrentDomain.BaseDirectory + Utility.defaultPicture));
            itemImage.Width = 75;
            itemImage.Height = 75;
            tbnamaItem.Style = Application.Current.TryFindResource("textblockblock-md") as Style;
            tbHarga.Style = Application.Current.TryFindResource("textblockblock-md") as Style;
            tbHarga.Foreground = new SolidColorBrush(Color.FromRgb(232,112,89));
            tbHarga.FontWeight = FontWeights.Bold;
            btnDelete.Content = "Remove";
            btnDelete.Style = Application.Current.TryFindResource("btn-danger") as Style;
            btnDelete.Margin = new Thickness(0, 0, 10, 0);
            btnMin.Content = "-";
            btnMin.Style = Application.Current.TryFindResource("btn-primary") as Style;
            btnMin.Margin = new Thickness(10, 0, 10, 0);
            btnPlus.Content = "+";
            btnPlus.Style = Application.Current.TryFindResource("btn-primary") as Style;
            btnPlus.Margin = new Thickness(10, 0, 10, 0);
            tbQty.VerticalAlignment = VerticalAlignment.Center;
            tbQty.Style = Application.Current.TryFindResource("textblockblock-md") as Style;
            spContent.Margin = new Thickness(10, 0, 0, 0);

            spCondition.Margin = new Thickness(0,10, 0, 10);
            this.Padding = new Thickness(5, 7, 5, 7);
            this.Margin = new Thickness(0, 0, 0, 10);
            this.AddChild(spMain);
        }

        private void BtnDeleteOnClick(object sender, RoutedEventArgs e)
        {
            CartViewModel.deleteItemFromCart(Convert.ToInt32(item["ID"]));
        }

        private void BtnPlusOnClick(object sender, RoutedEventArgs e)
        {
            //TODO SETELAH DI PLUS HARUS UPDATE HARGA DI CART :)
            // BUAT AJA 1 FUNCTION STATIC DI CARTVIEWMODEL TRUS DIPANGGIL
            qty++;
            qty = Math.Min(Convert.ToInt32(item["STOK"]), qty);
            tbQty.Text = qty.ToString();
            updateQty();
            CartViewModel.countSubTotal();
        }

        private void BtnMinOnClick(object sender, RoutedEventArgs e)
        {
            qty--;
            qty = Math.Max(0, qty);
            tbQty.Text = qty.ToString();
            updateQty();
            CartViewModel.countSubTotal();
        }

        public void initComponent(DataRow item, int qty)
        {
            this.item = item;
            this.qty = qty;
            tbnamaItem.Text = item["NAMA"].ToString();
            tbHarga.Text = Utility.formatMoney(Convert.ToInt32(item["HARGA"]));
            updateQty();

        }
        
        public void updateQty()
        {
            tbQty.Text = qty.ToString();
            CartViewModel.updateJumlah(Convert.ToInt32(item["ID"]),qty);
            subtotal = qty * Convert.ToInt32(item["HARGA"]);
            parent.updateSubTotal();;
        }

        public int getHarga()
        {
            return subtotal;
        }
    }
}
