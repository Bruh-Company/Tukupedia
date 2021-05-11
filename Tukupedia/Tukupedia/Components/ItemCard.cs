using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MaterialDesignThemes.Wpf;
using Tukupedia.Helpers.Utils;

namespace Tukupedia.Components
{
    public class ItemCard : MaterialDesignThemes.Wpf.Card
    {
        private Image _image;
        private StackPanel _stackPanel;
        private TextBlock namaBarang;
        private TextBlock harga;
        private RatingBar rating;
        private TextBlock terjual;
        private Button lihatItem;
        public ItemCard()
        {
            this.UniformCornerRadius = 5;
            this.Height = 300;
            this.Width = 195;
            this.Margin = new Thickness(3, 2, 3, 2);
            this.Padding = new Thickness(10, 4, 10, 4);
            _stackPanel = new StackPanel();
            _image = new Image();
            _image.Source =
                new BitmapImage(new Uri(
                    AppDomain.CurrentDomain.BaseDirectory + "Resource\\Logo\\TukupediaLogo.png"));
            _image.Width = 150;
            _image.Height = 150;
            // Label Nama Barang
            namaBarang = new TextBlock();
            namaBarang.Style = Application.Current.TryFindResource("textblockblock-lg") as Style;
            
            // Label Harga
            harga = new TextBlock();
            harga.Style = Application.Current.TryFindResource("textblockblock-md-danger") as Style;
            harga.FontSize = 16;
            harga.Foreground = new SolidColorBrush(Color.FromRgb(232,112,89));
            // Label Rating
            rating = new RatingBar();
            rating.Foreground = new SolidColorBrush(Colors.Yellow);
            rating.IsReadOnly = true;
            rating.Min = 1;
            rating.Max = 5;
            // Label Tejual 
            terjual = new TextBlock();
            terjual.Style = Application.Current.TryFindResource("textblockblock-md-danger") as Style;
            terjual.FontSize = 16;
            terjual.Foreground = new SolidColorBrush(Color.FromRgb(200,200,200));
            // Button Lihat Item
            lihatItem = new Button();
            lihatItem.Style = Application.Current.TryFindResource("btn-primary") as Style;
            lihatItem.Content = "Lihat Item";
            lihatItem.Margin = new Thickness(0, 10, 0, 0);
            //TODO Kasih Handler untuk buat form detail page (Boleh 1 form ato pakai window baru)
            

        }

        public void setImage(string url)
        {
            _image.Source =
                new BitmapImage(new Uri(
                    AppDomain.CurrentDomain.BaseDirectory + url));
        }
        public void setNamaBarang(string nama)
        {
            namaBarang.Text = nama;
        }
        public void setHarga(int harga)
        {
            this.harga.Text = Utility.formatMoney(harga);
        }
        public void setRating(int rating)
        {
            this.rating.Value = rating;
        }
        public void setJual(string jual)
        {
            terjual.Text = jual;
        }

        public void deploy()
        {
            _stackPanel.Children.Add(_image);
            _stackPanel.Children.Add(namaBarang);
            _stackPanel.Children.Add(harga);
            _stackPanel.Children.Add(rating);
            _stackPanel.Children.Add(terjual);
            _stackPanel.Children.Add(lihatItem);
            this.AddChild(_stackPanel);
        }
        
    }
}