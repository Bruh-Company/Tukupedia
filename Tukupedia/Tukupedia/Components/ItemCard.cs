using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MaterialDesignThemes.Wpf;
using SAPBusinessObjects.WPF.Viewer;
using Tukupedia.Helpers.Utils;
using Tukupedia.Views.Customer;

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
        private StackPanel spRating;
        private TextBlock tbRating;
        private DataRow item;
        public ItemCard()
        {
            this.UniformCornerRadius = 5;
            this.Height = 300;
            this.Width = 195;
            this.Margin = new Thickness(3, 2, 3, 2);
            this.Padding = new Thickness(10, 4, 10, 4);
            _stackPanel = new StackPanel();
            _image = new Image();
            ImageHelper.loadImageSwole(_image);
            _image.Width = 150;
            _image.Height = 150;
            // Label Nama Barang
            namaBarang = new TextBlock();
            namaBarang.Style = Application.Current.TryFindResource("textblock-lg") as Style;
            
            // Label Harga
            harga = new TextBlock();
            harga.Style = Application.Current.TryFindResource("textblock-md-danger") as Style;
            harga.FontSize = 16;
            harga.Foreground = new SolidColorBrush(Color.FromRgb(232,112,89));
            harga.FontWeight = FontWeights.Bold;
            //Stack Panel untuk rating
            spRating = new StackPanel();
            // Label + Bintang Rating
            rating = new RatingBar();
            rating.Foreground = new SolidColorBrush(Colors.Yellow);
            rating.IsReadOnly = true;
            rating.Min = 1;
            rating.Max = 5;
            tbRating = new MyTextBlock();
            tbRating.Text = Utility.formatNumber(0);
            tbRating.Style = Application.Current.TryFindResource("textblock-sm") as Style;
            tbRating.VerticalAlignment = VerticalAlignment.Center;
            tbRating.Margin = new Thickness(3, 0, 0, 0);
            spRating.Children.Add(rating);
            spRating.Children.Add(tbRating);
            spRating.Orientation = Orientation.Horizontal;
            // Label Tejual 
            terjual = new TextBlock();
            terjual.Style = Application.Current.TryFindResource("textblock-md-danger") as Style;
            terjual.FontSize = 16;
            terjual.Foreground = new SolidColorBrush(Color.FromRgb(200,200,200));
            // Button Lihat Item
            lihatItem = new Button();
            lihatItem.Style = Application.Current.TryFindResource("btn-primary") as Style;
            lihatItem.Content = "Lihat Item";
            lihatItem.Margin = new Thickness(0, 10, 0, 0);
            lihatItem.Click+=LihatItemOnClick;
        }
        private void LihatItemOnClick(object sender, RoutedEventArgs e)
        {
            ItemDetailView itemDetailView = new ItemDetailView();
            //Berguna supaya tidak bisa di alt tab
            itemDetailView.Owner = Window.GetWindow((Window)PresentationSource.FromVisual(this).RootVisual);
            itemDetailView.initDetail(item:this.item);
            itemDetailView.ShowDialog();
        }

        public void setItem(DataRow item)
        {
            this.item = item;
        }
        
        public void setImage(string url)
        {
            ImageHelper.loadImage(_image, url,ImageHelper.target.item);
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
            tbRating.Text = Utility.formatNumber(rating);
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
            _stackPanel.Children.Add(spRating);
            _stackPanel.Children.Add(terjual);
            _stackPanel.Children.Add(lihatItem);
            this.AddChild(_stackPanel);
        }
        
    }
}