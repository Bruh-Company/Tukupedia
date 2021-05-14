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

        public DiscussionCard()
        {
            spMain = new StackPanel();
            spComments = new StackPanel();
            spReply = new StackPanel();
            mainComment = new Comment(false);

            spMain.Children.Add(mainComment);
            spMain.Children.Add(spComments);
            spMain.Children.Add(spReply);
            this.AddChild(spMain);
        }

        public void initMainComment(string message, string commenterName, string date, string url)
        {
            mainComment.init(message,commenterName,date,url);
        }
        public void initComments(int id_h_diskusi)
        {
            D_DiskusiModel ddm = new D_DiskusiModel();
            ddm.addWhere("ID_H_DISKUSI",id_h_diskusi.ToString());
            // Dapatkan semua comment dengan id header diskusi yang diminta
            foreach (DataRow row in ddm.get())
            {
                bool isPenjual;
                MessageBox.Show(row["ID_SELLER"].ToString());
                isPenjual = row["SENDER"].ToString() == "C" ? false : true;
                Comment com = new Comment(isPenjual);
                string date;
                DataRow user;
                if (isPenjual) user = new DB("SELLER").@where("ID", row["ID_SELLER"].ToString()).getFirst();
                else user = new DB("CUSTOMER").@where("ID", row["ID_CUSTOMER"].ToString()).getFirst();
                com.init(
                    message:row["MESSAGE"].ToString(),
                    commenterName:user["NAMA"].ToString(),
                    date:Utility.formatDate(row["CREATED_AT"].ToString()),
                    url:user["IMAGE"].ToString()
                    );
                spComments.Children.Add(com);
            }
        }
    }
}