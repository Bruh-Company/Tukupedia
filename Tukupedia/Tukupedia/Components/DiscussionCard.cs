using System.Data;
using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using Tukupedia.Helpers.DatabaseHelpers;
using Tukupedia.Helpers.Utils;
using Tukupedia.Models;

namespace Tukupedia.Components
{
    public class DiscussionCard : Card
    {
        private StackPanel spMain;
        private StackPanel spComments;
        private StackPanel spReply;
        private Comment mainComment;
        bool exist = false;

        public DiscussionCard(double fullwidth)
        {
            spMain = new StackPanel();
            spComments = new StackPanel();
            spReply = new StackPanel();
            mainComment = new Comment(false);
            spComments.Margin = new Thickness(20, 0, 0, 0);

            spMain.Children.Add(mainComment);
            spMain.Children.Add(spComments);
            spMain.Children.Add(spReply);
            spMain.Width = fullwidth;
            this.AddChild(spMain);
            this.Margin = new Thickness(5, 5, 5, 5);

        }

        public void initMainComment(string message, string commenterName, string date, string url)
        {
            mainComment.init(message,commenterName,date,url);
            exist = true;
        }
        public void initComments(int id_h_diskusi)
        {
            D_DiskusiModel ddm = new D_DiskusiModel();
            ddm.addWhere("ID_H_DISKUSI",id_h_diskusi.ToString());
            // Dapatkan semua comment dengan id header diskusi yang diminta
            
            foreach (DataRow row in ddm.get())
            {
                exist = true;
                bool isPenjual;
                isPenjual = row["SENDER"].ToString() == "C" ? false : true;
                Comment com = new Comment(isPenjual);
                DataRow user;
                if (isPenjual) {
                    user = new DB("SELLER").select().@where("ID", row["ID_SELLER"].ToString()).getFirst();
                    com.init(
                    message: row["MESSAGE"].ToString(),
                    commenterName: user["NAMA_SELLER"].ToString(),
                    date: Utility.formatDate(row["CREATED_AT"].ToString()),
                    url: user["IMAGE"].ToString()
                    );
                }
                else {
                    user = new DB("CUSTOMER").select().@where("ID", row["ID_CUSTOMER"].ToString()).getFirst();
                    com.init(
                        message: row["MESSAGE"].ToString(),
                        commenterName: user["NAMA"].ToString(),
                        date: Utility.formatDate(row["CREATED_AT"].ToString()),
                        url: user["IMAGE"].ToString()
                        );
                }
                spComments.Children.Add(com);
            }

            if (exist)
            {
                ReplyCard rc = new ReplyCard();
                rc.initCard(id_h_diskusi);
                spComments.Children.Add(rc);
            }
            
        }
    }
}