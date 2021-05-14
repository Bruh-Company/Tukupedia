using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using MaterialDesignThemes.Wpf;
using Tukupedia.Helpers.Utils;
using Tukupedia.Models;
using Tukupedia.ViewModels.Customer;

namespace Tukupedia.Components
{
    public class ReplyCard:Card
    {
        private StackPanel spMain;
        private Image profilePic;
        private RichTextBox richTextBox;
        private Button btnKirim;
        private int id_h_diskusi;
        public ReplyCard()
        {
            spMain = new StackPanel();
            spMain.Orientation = Orientation.Horizontal;
            profilePic = new Image();
            richTextBox = new RichTextBox();
            btnKirim = new Button();
            

            spMain.Children.Add(profilePic);
            spMain.Children.Add(richTextBox);
            spMain.Children.Add(btnKirim);
            
            //Add event
            btnKirim.Click+=BtnKirimOnClick;
            
            //Add to Cart
            this.AddChild(spMain);
            
            //Init Style
            profilePic.Width = 50;
            profilePic.Height = 50;
            profilePic.VerticalAlignment = VerticalAlignment.Top;
            profilePic.HorizontalAlignment = HorizontalAlignment.Center;
            profilePic.Source =
                new BitmapImage(new Uri(
                    AppDomain.CurrentDomain.BaseDirectory + Utility.defaultPicture));
            btnKirim.Content = "Send";
            btnKirim.Style = Application.Current.TryFindResource("btn-primary") as Style;
            btnKirim.Width = 75;
            btnKirim.VerticalAlignment = VerticalAlignment.Center;
            btnKirim.HorizontalAlignment = HorizontalAlignment.Center;
            btnKirim.Margin = new Thickness(15,0,0,0);
            richTextBox.Width = 450;
            richTextBox.Height = 100;


        }

        public void initCard(int id_h_diskusi, string url="")
        {
            this.id_h_diskusi = id_h_diskusi;
            if (url != "")
            {
                profilePic.Source =
                    new BitmapImage(new Uri(
                        AppDomain.CurrentDomain.BaseDirectory + url));
            }
            
        }
        private void BtnKirimOnClick(object sender, RoutedEventArgs e)
        {
            // Kirim Diskusi
            D_DiskusiModel dti = new D_DiskusiModel();
            string col, id,Sender;
            if (Session.role.ToUpper() == "SELLER")
            {
                col = "ID_SELLER";
                Sender = "S";
            }
            else
            {
                col = "ID_CUSTOMER";
                Sender = "C";
            }
            id = Session.User["ID"].ToString();
            string message = Utility.StringFromRichTextBox(richTextBox);
            dti.insert(
                "ID",0,
                col,id,
                "MESSAGE",message,
                "SENDER",Sender,
                "ID_H_DISKUSI",id_h_diskusi,
                "CREATED_AT",DateTime.Now.ToString()
            );
            ItemDetailViewModel.resetDiscussion();
        }
    }
}