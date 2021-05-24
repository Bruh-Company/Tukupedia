using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MaterialDesignThemes.Wpf;
using Tukupedia.Helpers.DatabaseHelpers;
using Tukupedia.Helpers.Utils;
using Tukupedia.Models;
using Tukupedia.ViewModels.Customer;

namespace Tukupedia.Components
{
    public class ShopCartComponent:Card
    {
        private StackPanel spMain;
        private StackPanel spDesc;
        private StackPanel spCart;
        private StackPanel spContent;
        private StackPanel spSubtotal;
        private StackPanel spKurir;
        private Image imgToko;
        private TextBlock namaToko;
        private TextBlock tbisOfficial;
        private TextBlock pilihDurasi;
        private TextBlock subTotal;
        private TextBlock labelsubTotal;
        private ComboBox cbKurir;
        private bool isOfficial;
        private DataTable kurir_seller;
        private int hargaTotal;
        private List<CartComponent> list_carts;
        private int Quantity,hargaAwal,ongkosKirim;


        public ShopCartComponent()
        {
            list_carts = new List<CartComponent>();
            isOfficial = false;
            spMain = new StackPanel();
            spDesc = new StackPanel();
            spContent = new StackPanel();
            spCart = new StackPanel();
            spSubtotal = new StackPanel();
            spKurir = new StackPanel();
            namaToko = new TextBlock();
            imgToko = new Image();
            tbisOfficial = new TextBlock();
            pilihDurasi = new TextBlock();
            subTotal = new TextBlock();
            labelsubTotal = new TextBlock();
            cbKurir = new ComboBox();
            kurir_seller = new DataTable();
            hargaTotal = 0;

            spDesc.Orientation = Orientation.Horizontal;
            spDesc.Children.Add(imgToko);
            spDesc.Children.Add(namaToko);
            //Check if Official
            spDesc.Children.Add(tbisOfficial);

            spKurir.Children.Add(pilihDurasi);
            spKurir.Children.Add(cbKurir);
            
            spContent.Orientation = Orientation.Horizontal;
            spContent.Children.Add(spCart);
            spContent.Children.Add(spKurir);

            spSubtotal.Orientation = Orientation.Horizontal;
            spSubtotal.Children.Add(labelsubTotal);
            spSubtotal.Children.Add(subTotal);
            
            spMain.Children.Add(spDesc);
            spMain.Children.Add(spContent);
            spMain.Children.Add(spSubtotal);
            
            this.AddChild(spMain);
            
            //Events
            cbKurir.SelectionChanged+=CbKurirOnSelectionChanged;
            
            //init Styles
            imgToko.Width = 50;
            imgToko.Height = 50;
            Random r = new Random();
            if (r.Next(2)==0) ImageHelper.loadImageCheems(imgToko);
            else ImageHelper.loadImageSwole(imgToko);
            
            namaToko.Style = Application.Current.TryFindResource("textblockblock-md-success") as Style;
            namaToko.VerticalAlignment = VerticalAlignment.Center;
            tbisOfficial.Text = "Official";
            tbisOfficial.Background = new SolidColorBrush(Color.FromRgb(127, 55, 215));
            tbisOfficial.Foreground = new SolidColorBrush(Color.FromRgb(255, 166, 23));
            tbisOfficial.Margin = new Thickness(10, 0, 0, 0);
            tbisOfficial.Padding = new Thickness(4, 4, 4, 4);
            tbisOfficial.VerticalAlignment = VerticalAlignment.Center;
            tbisOfficial.Style = Application.Current.TryFindResource("textblockblock-sm") as Style;
            pilihDurasi.FontWeight = FontWeights.Bold;
            pilihDurasi.Text = "Pilih Durasi ";
            pilihDurasi.Style = Application.Current.TryFindResource("textblockblock-sm") as Style;
            labelsubTotal.Text = "Subtotal : ";
            labelsubTotal.Style = Application.Current.TryFindResource("textblockblock-sm") as Style;
            subTotal.Style = Application.Current.TryFindResource("textblockblock-sm") as Style;
            subTotal.Text = Utility.formatMoney(0);
            cbKurir.Width = 200;
            spCart.Margin = new Thickness(50, 10, 20, 10);
            spCart.Width = 500;
            labelsubTotal.Style = Application.Current.TryFindResource("textblockblock-md") as Style;
            labelsubTotal.VerticalAlignment = VerticalAlignment.Center;
            subTotal.VerticalAlignment = VerticalAlignment.Center;
            subTotal.HorizontalAlignment = HorizontalAlignment.Right;
            subTotal.Style = Application.Current.TryFindResource("textblockblock-md") as Style;
            
            this.Padding = new Thickness(5, 7, 5, 7);
            this.Margin = new Thickness(0, 0, 0, 10);
        }

        private void CbKurirOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Harga Kurir itu berdasarkan barang,
            // Kalau biasa itu, anggap di db itu harga/500 Gram? Tetapi kalau kurang dri 500 gram itu minimal bayar harga
            // jangan lupa untuk liat diskon dri promo
            updateSubTotal();
            CartViewModel.updateGrandTotal();
            CartViewModel.checkPromotion(CartViewModel.promo);
        }

        public void initToko(DataRow toko)
        {
            namaToko.Text = toko["NAMA_TOKO"].ToString();
            isOfficial = toko["IS_OFFICIAL"].ToString() == "1" ? true: false;
            if (!isOfficial) tbisOfficial.Visibility = Visibility.Hidden;
            if (toko["IMAGE"].ToString() != "")
            {
                ImageHelper.loadImage(imgToko, toko["IMAGE"].ToString());
            }
            Kurir_SellerModel ksm = new Kurir_SellerModel();
            DataRow[] rows = ksm.Table.Select($"ID_SELLER ='{toko["ID"]}'");
            if (rows.Length > 0)
            {
                kurir_seller = rows.CopyToDataTable();
                foreach (DataRow row in kurir_seller.Rows)
                {
                    ComboBoxItem cbi = new ComboBoxItem();
                    DataRow kurir = new DB("KURIR").@select().@where("ID", row["ID_KURIR"].ToString()).getFirst();
                    cbi.Content = kurir["NAMA"].ToString();
                    cbi.Tag = kurir["ID"].ToString();
                    cbKurir.Items.Add(cbi);
                }
            }
            
        }

        public void updateSubTotal()
        {
            hargaAwal = 0;
            ongkosKirim = 0;
            int hargaKurir = 0, berat = 0;
            DataRow kurir=null;
            int idxKurir = cbKurir.SelectedIndex;
            if (idxKurir >= 0)
            {
                string selectedKurir = ((ComboBoxItem) cbKurir.SelectedItem).Tag.ToString();
                kurir = new DB("KURIR").@select().@where("ID", selectedKurir).getFirst();
                    if (kurir != null) hargaKurir = Convert.ToInt32(kurir["HARGA"]);
            }
            Quantity = 0;
            bool checkedCart = false;
            foreach (CartComponent cart in list_carts)
            {
                if (cart.isChecked())
                {
                    checkedCart = true;
                    Quantity += cart.getQuantity();
                    hargaAwal += cart.getHarga();
                    if (kurir != null)
                    {
                        berat += cart.getBerat();
                    }
                }
            }
            //Check kalau ada barang yang di check ada atau tidak, kalau gaada gaada ongkir
            if(checkedCart) ongkosKirim = berat < 1000 ? hargaKurir : hargaKurir * Convert.ToInt32((double)(berat / 1000));
            hargaTotal = hargaAwal + ongkosKirim;
            hargaTotal=berat > 0 ? hargaTotal : 0;
            //Untuk harga Kurir belum
            subTotal.Text = Utility.formatMoney(hargaTotal);
            CartViewModel.checkPromotion(CartViewModel.promo);
        }

        public int getOngkir()
        {
            return ongkosKirim;
        }
        public int getQuantity()
        {
            return Quantity;
        }

        public int getHargaSebelumOngkir()
        {
            return hargaAwal;
        }
        public int getSubtotal()
        {
            return hargaTotal;
        }
        public void addItemCart(List<DataRow> list_item)
        {
            foreach (var row in list_item)
            {
                CartComponent cc = new CartComponent(this);
                cc.initComponent(row,CartViewModel.getJumlah(Convert.ToInt32(row["ID"])));
                list_carts.Add(cc);
                spCart.Children.Add(cc);
            }
            updateSubTotal();
        }

        public Dictionary<string, bool> getCheckedItems()
        {
            Dictionary<string, bool> checked_items = new Dictionary<string, bool>();
            foreach (var cart in list_carts)
            {
                checked_items.Add(cart.getItemID(),cart.isChecked());
            }
            return checked_items;
        }

        public void setCheckedItems(Dictionary<string, bool> list_checked)
        {
            foreach (var cart in list_carts)
            {
                string item_id = cart.getItemID();
                if (list_checked.ContainsKey(item_id))
                {
                    cart.setChecked(list_checked[item_id]);
                }
                else
                {
                    //kalau gaada di list checked maka defaultnya adlah false
                    cart.setChecked(false);
                }
            }
        }

        public List<DataRow> getItems()
        {
            List<DataRow> list_items = new List<DataRow>();
            foreach (var cart in list_carts)
            {
                if (cart.isChecked())
                {
                    list_items.Add(cart.getItem());   
                }
            }
            return list_items;
        }

        public string getIDKurir()
        {
            if(cbKurir.SelectedIndex>=0) return ((ComboBoxItem) cbKurir.SelectedItem).Tag.ToString();
            else
            {
                return "";
            }
        }

        public List<CartComponent> getCarts()
        {
            List<CartComponent> checked_carts = new List<CartComponent>();
            foreach (CartComponent cc in list_carts)
            {
                if (cc.isChecked())
                {
                    checked_carts.Add(cc);
                }
            }

            return checked_carts;
        }


    }
}