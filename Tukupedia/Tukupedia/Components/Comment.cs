using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MaterialDesignThemes.Wpf;
using Tukupedia.Helpers.Utils;

namespace Tukupedia.Components
{
    public class Comment : Card
    {
        private StackPanel spMain;
        private StackPanel spContent;
        private StackPanel spDesc;
        private TextBlock message;
        private TextBlock commenterName;
        private TextBlock date;
        private TextBlock keteranganPenjual;
        private Image profilePic;
        private bool isPenjual;

        public Comment(bool isPenjual, bool transparent=false)
        {
            this.isPenjual = isPenjual;
            this.spMain = new StackPanel();
            this.spMain.Orientation = Orientation.Horizontal;
            this.spContent = new StackPanel();
            this.spDesc = new StackPanel();
            this.spDesc.Orientation = Orientation.Horizontal;
            this.message = new TextBlock();
            this.commenterName = new TextBlock();
            this.date = new TextBlock();
            this.keteranganPenjual = new TextBlock();
            this.profilePic = new Image();

            this.spDesc.Children.Add(commenterName);
            if(isPenjual)
                this.spDesc.Children.Add(keteranganPenjual);
            this.spDesc.Children.Add(date);
            
            this.spContent.Children.Add(spDesc);
            this.spContent.Children.Add(message);
            
            this.spMain.Children.Add(profilePic);
            this.spMain.Children.Add(spContent);
            
            this.AddChild(spMain);
            ImageHelper.loadImageCheems(profilePic);
            //Init Style
            this.profilePic.Width = 50;
            this.profilePic.Height = 50;
            this.message.Style = Application.Current.TryFindResource("textblock-md") as Style;
            this.message.Margin = new Thickness(0, 5, 0, 2);
            this.commenterName.Style = Application.Current.TryFindResource("textblock-md-success") as Style;
            this.commenterName.VerticalAlignment = VerticalAlignment.Center;
            this.date.Style = Application.Current.TryFindResource("textblock-sm") as Style;
            this.date.Foreground = new SolidColorBrush(Colors.DarkGray);
            this.date.FontSize = 12;
            this.date.VerticalAlignment = VerticalAlignment.Center;
            this.date.Margin = new Thickness(4, 2, 0, 0);


        }

        public void init(string message, string commenterName, string date, string url ="")
        {
            this.message.Text = message;
            this.commenterName.Text = commenterName;
            this.date.Text = date;
            if (url != "")
            {
                if (isPenjual)
                {
                    ImageHelper.loadImage(profilePic, url, ImageHelper.target.seller);
                }
                else
                {
                    ImageHelper.loadImage(profilePic, url, ImageHelper.target.customer);
                }
                
            }
        }

    }
}