using System;
using System.Data;
using System.Net.Configuration;
using System.Windows;
using System.Windows.Controls;
using Tukupedia.Components;
using Tukupedia.Helpers.DatabaseHelpers;
using Tukupedia.Helpers.Utils;
using Tukupedia.Models;

namespace Tukupedia.ViewModels.Customer
{
    public class ItemDetailViewModel
    {
        public static void loadDiscussions(StackPanel elem, int ItemID)
        {
            H_DiskusiModel hdm = new H_DiskusiModel();
            hdm.addWhere("ID_ITEM",ItemID.ToString());
            hdm.addOrderBy("CREATED_AT ASC");
            foreach (DataRow row in hdm.get())
            {
                DiscussionCard dc = new DiscussionCard(elem.ActualWidth);
                DataRow customer = new DB("CUSTOMER").select().@where("ID", row["ID_CUSTOMER"].ToString()).getFirst();
                dc.initMainComment(
                    message:row["MESSAGE"].ToString(),
                    commenterName:customer["NAMA"].ToString(),
                    date: Utility.formatDate(row["CREATED_AT"].ToString()),
                    url:customer["IMAGE"].ToString()
                    );
                dc.initComments(Convert.ToInt32(row["ID"]));
                elem.Children.Add(dc);
            }
        }

        public static void loadReviews(StackPanel elem, int ItemID, double width)
        {
            DataTable table = new DB("ULASAN")
                .@select("CUSTOMER.NAMA as namaCust","ULASAN.MESSAGE as MESSAGE", "ULASAN.RATING as RATING", 
                    "TO_CHAR(ULASAN.CREATED_AT,\'DD-MM-YYYY\') as tglUlasan","ULASAN.ID_SELLER as ID_SELLER",
                    "ULASAN.REPLY_AT as REPLY_AT","ULASAN.REPLY as REPLY","CUSTOMER.IMAGE as IMAGE_CUST")
                .@join("D_TRANS_ITEM", "ULASAN","ID_D_TRANS_ITEM", "=", "ID")
                .@join("ITEM", "D_TRANS_ITEM","ID_ITEM", "=", "ID")
                .@join("H_TRANS_ITEM","D_TRANS_ITEM","ID_H_TRANS_ITEM","=","ID")
                .@join("CUSTOMER","H_TRANS_ITEM","ID_CUSTOMER","=","ID")
                .@where("ITEM.ID",ItemID.ToString()).get(true);
            
            foreach (DataRow row in table.Rows)
            {
                ReviewCard rc = new ReviewCard(width);
                rc.setCust(row["namaCust"].ToString());
                rc.setDateCust(row["tglUlasan"].ToString());
                rc.setRating(Convert.ToInt32(row["RATING"]));
                rc.setDescription(row["MESSAGE"].ToString());
                //SET CUSTOMER PROFILE PICTURE
                if (row["IMAGE_CUST"].ToString() != "")
                {
                    rc.setCustImage(row["IMAGE_CUST"].ToString());
                }
                // IF ADA
                if (row["REPLY"].ToString() != "")
                {
                    DataRow seller = new DB("SELLER").@select().@where("ID", row["ID_SELLER"].ToString()).getFirst();
                    rc.setSeller(seller["NAMA"].ToString());
                    rc.setSellerDate(row["REPLY_AT"].ToString());
                    rc.setSellerReply(row["REPLY"].ToString());
                    //Set Profile Foto Seller
                    if (seller["IMAGE"].ToString() != "")
                    {
                        rc.setSellerImage(seller["IMAGE"].ToString());
                    }
                }
                elem.Children.Add(rc);
            }
        }

        public static void kirimDiskusi(string message,int id_item)
        {
            H_DiskusiModel hd = new H_DiskusiModel();
            MessageBox.Show(message);
            hd.insert(
                "ID",0,
                "ID_CUSTOMER",Session.User["ID"].ToString(),
                "MESSAGE", message,
                "ID_ITEM",id_item,
                "STATUS",1,
                "CREATED_AT",DateTime.Now.ToString()
                );
        }
    }
}