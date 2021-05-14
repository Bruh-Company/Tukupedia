using System;
using System.Windows.Controls;
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

        public Comment(bool isPenjual, bool transparent=false)
        {
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

            this.spMain.Children.Add(profilePic);
            this.spMain.Children.Add(spContent);

            this.spContent.Children.Add(spDesc);
            this.spContent.Children.Add(message);

            this.spDesc.Children.Add(commenterName);
            if(isPenjual)
                this.spDesc.Children.Add(keteranganPenjual);
            this.spDesc.Children.Add(date);
            
            this.AddChild(spMain);
            profilePic.Source =
                new BitmapImage(new Uri(
                    AppDomain.CurrentDomain.BaseDirectory + Utility.defaultPicture));
            //Init Style
        }

        public void init(string message, string commenterName, string date, string url ="")
        {
            this.message.Text = message;
            this.commenterName.Text = commenterName;
            this.date.Text = date;
            if (url != "")
            {
                profilePic.Source =
                    new BitmapImage(new Uri(
                        AppDomain.CurrentDomain.BaseDirectory + url));
            }
        }

    }
}