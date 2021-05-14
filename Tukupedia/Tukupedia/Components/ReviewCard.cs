using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;

namespace Tukupedia.Components
{
    public class ReviewCard :Card
    {
        private StackPanel stackPanelMain;
        private StackPanel stackPanelCust;
        private StackPanel stackPanelContent;
        private StackPanel stackPanelSeller;
        private StackPanel stackPanelSellerKeterangan;
        private StackPanel stackPanelCustProfile;
        private StackPanel stackPanelSellerProfile;
        private TextBlock tbNamaCust;
        private TextBlock tbDateCust;
        private Image imgCust;
        
        private TextBlock tbNamaToko;
        private TextBlock tbKeteranganPenjual;
        private TextBlock tbReplyUlasan;
        private TextBlock tbDateSeller;
        private Image imgSeller;
        
        private TextBlock tbUlasan;
        private RatingBar ratingBar;

        private CustomCard sellerCard;
        private CustomCard contentCard;

        public ReviewCard(double fullWidth)
        {
            //Kalau bingung tnya chen aja thanks. Maaf Kodingannya pusing, saya juga pusing
            RectangleGeometry geometry = new RectangleGeometry();
            geometry.RadiusX = 5;
            geometry.RadiusY = 5;
            geometry.Rect = new Rect(0,0,150,113);

            stackPanelMain = new StackPanel();
            stackPanelMain.Width = Double.NaN;
            stackPanelMain.Orientation = Orientation.Horizontal;
            
            stackPanelContent = new StackPanel();
            ratingBar = new RatingBar();
            ratingBar.Foreground = new SolidColorBrush(Colors.Yellow);
            ratingBar.IsReadOnly = true;
            ratingBar.Min = 1;
            ratingBar.Max = 5;
            tbUlasan = new TextBlock();
            stackPanelContent.Children.Add(ratingBar);
            stackPanelContent.Children.Add(tbUlasan);
            
            stackPanelSeller = new StackPanel();
            stackPanelSellerKeterangan = new StackPanel();
            stackPanelSellerKeterangan.Orientation = Orientation.Horizontal;
            tbNamaToko = new TextBlock();
            tbKeteranganPenjual = new TextBlock();
            stackPanelSellerKeterangan.Children.Add(tbNamaToko);
            stackPanelSellerKeterangan.Children.Add(tbKeteranganPenjual);
            tbReplyUlasan = new TextBlock();
            tbDateSeller = new TextBlock();

            stackPanelSellerProfile = new StackPanel();
            stackPanelSellerProfile.Orientation = Orientation.Horizontal;
            stackPanelSeller.Children.Add(stackPanelSellerKeterangan);
            stackPanelSeller.Children.Add(tbDateSeller);
            stackPanelSeller.Children.Add(tbReplyUlasan);

            imgSeller = new Image();
            imgSeller.Width = 50;
            imgSeller.Height = 50;
            imgSeller.Clip = geometry;
            imgSeller.HorizontalAlignment = HorizontalAlignment.Center;
            imgSeller.VerticalAlignment = VerticalAlignment.Top;
            imgSeller.Source =
                new BitmapImage(new Uri(
                    AppDomain.CurrentDomain.BaseDirectory + "Resource\\Logo\\TukupediaLogo.png"));
            stackPanelSellerProfile.Children.Add(imgSeller);
            stackPanelSellerProfile.Children.Add(stackPanelSeller);

            sellerCard = new CustomCard(stackPanelSellerProfile);
            //Add Seller to Conent
            stackPanelContent.Children.Add(sellerCard);//End

            stackPanelCustProfile = new StackPanel();
            stackPanelCustProfile.Orientation = Orientation.Horizontal;
            stackPanelCust = new StackPanel();
            tbNamaCust = new TextBlock();
            tbDateCust = new TextBlock();
            stackPanelCust.Children.Add(tbNamaCust);
            stackPanelCust.Children.Add(tbDateCust);

            
            
            imgCust = new Image();
            imgCust.Width = 50;
            imgCust.Height = 50;
            imgCust.Clip = geometry;
            imgCust.HorizontalAlignment = HorizontalAlignment.Center;
            imgCust.VerticalAlignment = VerticalAlignment.Top;
            imgCust.Source =
                new BitmapImage(new Uri(
                    AppDomain.CurrentDomain.BaseDirectory + "Resource\\Logo\\TukupediaLogo.png"));
            stackPanelCustProfile.Children.Add(imgCust);
            stackPanelCustProfile.Children.Add(stackPanelCust);//End

            contentCard = new CustomCard(stackPanelContent);
            stackPanelMain.Children.Add(stackPanelCustProfile);
            stackPanelMain.Children.Add(contentCard);
            this.AddChild(stackPanelMain);
            // this.Background = new SolidColorBrush(Colors.Bisque);
            this.Padding = new Thickness(4, 4, 4, 4);
            this.Margin = new Thickness(4, 4, 4, 4);
            
            //Init Styles
            tbNamaCust.Style = Application.Current.TryFindResource("textblockblock-md-success") as Style;
            tbDateCust.FontSize = 14;
            tbUlasan.Style = Application.Current.TryFindResource("textblockblock-sm") as Style;
            tbUlasan.Margin = new Thickness(0, 5, 0, 2);
            sellerCard.Margin = new Thickness(0,4,0,7);
            contentCard.Margin = new Thickness(7, 2, 0, 2);
            contentCard.Padding = new Thickness(6, 4, 6, 4);
            contentCard.Width = fullWidth-200;
            tbNamaToko.Style = Application.Current.TryFindResource("textblockblock-md-success") as Style;
            tbDateSeller.FontSize = 14;
            tbReplyUlasan.Style = Application.Current.TryFindResource("textblockblock-sm") as Style;
            tbReplyUlasan.Margin = new Thickness(0, 5, 0, 5);
            tbKeteranganPenjual.Padding = new Thickness(4, 3, 4, 3);
            tbKeteranganPenjual.Margin = new Thickness(3, 0, 3, 0);
            tbKeteranganPenjual.Background = new SolidColorBrush(Color.FromRgb(214, 255, 222));
            tbKeteranganPenjual.Foreground = new SolidColorBrush(Color.FromRgb(42, 187, 52));
            sellerCard.Visibility = Visibility.Hidden;
            //TODO GANTI BORDER SHADOW JADI NONE
            sellerCard.BorderBrush = null;
            stackPanelMain.Width = fullWidth-25;


        }
        public void setCust(string cust)
        {
            tbNamaCust.Text = cust;
        }

        public void setDateCust(string date)
        {
            tbDateCust.Text = date;
        }

        public void setRating(int val)
        {
            ratingBar.Value = val;
        }

        public void setDescription(string desc)
        {
            tbUlasan.Text = desc;
        }

        public void setSeller(string seller)
        {
            tbNamaToko.Text = seller;
            sellerCard.Visibility = Visibility.Visible;
            tbKeteranganPenjual.Text = "Penjual";
        }

        public void setSellerDate(string date)
        {
            tbDateSeller.Text = date;
        }

        public void setSellerReply(string reply)
        {
            tbReplyUlasan.Text = reply;
        }

        public void setSellerImage(string url)
        {
            imgSeller.Source =
                new BitmapImage(new Uri(
                    AppDomain.CurrentDomain.BaseDirectory + url));
        }
        public void setCustImage(string url)
        {
            imgCust.Source =
                new BitmapImage(new Uri(
                    AppDomain.CurrentDomain.BaseDirectory + url));
        }
    }
}