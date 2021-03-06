using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MaterialDesignThemes.Wpf;
using Tukupedia.Helpers.Utils;
using Tukupedia.Models;
using Tukupedia.ViewModels.Customer;
using Tukupedia.ViewModels.Seller;

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
            ImageHelper.loadImageSwole(profilePic);
            btnKirim.Content = "Send";
            btnKirim.Style = Application.Current.TryFindResource("btn-primary") as Style;
            btnKirim.Width = 65;
            btnKirim.Height = 90;
            btnKirim.VerticalAlignment = VerticalAlignment.Center;
            btnKirim.HorizontalAlignment = HorizontalAlignment.Center;
            btnKirim.Margin = new Thickness(8,0,0,0);
            richTextBox.GotFocus += RichTextBox_GotFocus;
            richTextBox.LostFocus += RichTextBox_LostFocus;
            Utility.setRichTextBoxString(richTextBox, "Reply Here");
            richTextBox.Width = 450;
            richTextBox.Height = 100;
            richTextBox.Foreground = new SolidColorBrush(Colors.White);
            richTextBox.Margin = new Thickness(10, 0, 0, 0);


        }

        private void RichTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            string val = Utility.StringFromRichTextBox(richTextBox);
            if (val.Length <= 2)
            {
                Utility.setRichTextBoxString(richTextBox, "Reply Here");
            }
        }

        private void RichTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            string val = Utility.StringFromRichTextBox(richTextBox);
            if (val.Contains("Reply Here"))
            {
                Utility.setRichTextBoxString(richTextBox, "");
            }
        }

        public void initCard(int id_h_diskusi)
        {
            this.id_h_diskusi = id_h_diskusi;
            if (Session.User["IMAGE"].ToString() != "")
            {
                if (Session.role == "Seller")
                {

                    ImageHelper.loadImage(profilePic, Session.User["IMAGE"].ToString(), ImageHelper.target.seller);

                }
                else
                {
                    ImageHelper.loadImage(profilePic, Session.User["IMAGE"].ToString(), ImageHelper.target.customer);
                }
            }
            else
            {
                ImageHelper.loadImageSwole(profilePic);

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
            Utility.setRichTextBoxString(richTextBox, "");
            if (Session.role.ToUpper() == "SELLER") {
                SellerViewModel.pageProduk.resetDiskusi();
            }
            else {
                ItemDetailViewModel.resetDiscussion();
            }
        }
    }
}