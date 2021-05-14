using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using MaterialDesignThemes.Wpf;
using Tukupedia.Helpers.Utils;
using Tukupedia.Models;

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
            profilePic = new Image();
            richTextBox = new RichTextBox();
            btnKirim = new Button();
            

            spMain.Children.Add(profilePic);
            spMain.Children.Add(richTextBox);
            spMain.Children.Add(btnKirim);
            
            //Add event
            btnKirim.Click+=BtnKirimOnClick;
            
            //Init Style
            profilePic.Width = 50;
            profilePic.Height = 50;
            profilePic.Source =
                new BitmapImage(new Uri(
                    AppDomain.CurrentDomain.BaseDirectory + Utility.defaultPicture));
            btnKirim.Content = "Send";
            btnKirim.Style = Application.Current.TryFindResource("btn-primary") as Style;


        }

        public void initCard(int id_h_diskusi, string url="")
        {
            this.id_h_diskusi = id_h_diskusi;
            profilePic.Source =
                new BitmapImage(new Uri(
                    AppDomain.CurrentDomain.BaseDirectory + url));
        }
        private void BtnKirimOnClick(object sender, RoutedEventArgs e)
        {
            // Kirim Diskusi
            D_Trans_ItemModel dti = new D_Trans_ItemModel();
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
                "ID_H_DISKUSI",id_h_diskusi
            );
        }
    }
}