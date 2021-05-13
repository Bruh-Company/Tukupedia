using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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

        public ReviewCard( )
        {
            //Kalau bingung tnya chen aja thanks.
            stackPanelMain = new StackPanel();
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
            stackPanelSeller.Children.Add(stackPanelSellerKeterangan);
            stackPanelSeller.Children.Add(tbDateSeller);
            stackPanelSeller.Children.Add(tbReplyUlasan);

            imgSeller = new Image();
            imgSeller.Source =
                new BitmapImage(new Uri(
                    AppDomain.CurrentDomain.BaseDirectory + "Resource\\Logo\\TukupediaLogo.png"));
            stackPanelSellerProfile.Children.Add(imgSeller);
            stackPanelSellerProfile.Children.Add(stackPanelSeller);
            
            //Add Seller to Conent
            stackPanelContent.Children.Add(stackPanelSellerProfile);//End

            stackPanelCustProfile = new StackPanel();
            stackPanelCustProfile.Orientation = Orientation.Horizontal;
            stackPanelCust = new StackPanel();
            tbNamaCust = new TextBlock();
            tbDateCust = new TextBlock();
            stackPanelCust.Children.Add(tbNamaCust);
            stackPanelCust.Children.Add(tbDateCust);
            imgCust = new Image();
            imgCust.Source =
                new BitmapImage(new Uri(
                    AppDomain.CurrentDomain.BaseDirectory + "Resource\\Logo\\TukupediaLogo.png"));
            stackPanelCustProfile.Children.Add(imgCust);
            stackPanelCustProfile.Children.Add(stackPanelCust);//End

            stackPanelMain.Children.Add(stackPanelCustProfile);
            stackPanelMain.Children.Add(stackPanelContent);
            this.AddChild(stackPanelMain);
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