using System;
using System.Data;
using System.Net.Configuration;
using System.Windows;
using System.Windows.Controls;
using Tukupedia.Components;
using Tukupedia.Helpers.DatabaseHelpers;
using Tukupedia.Models;

namespace Tukupedia.ViewModels.Customer
{
    public class ItemDetailViewModel
    {
        public static void loadDiscussions(FrameworkElement elem, int ItemID)
        {
            DiskusiModel diskusiModel = new DiskusiModel();
            diskusiModel.addWhere("ID_ITEM",ItemID.ToString());
            foreach (DataRow row in diskusiModel.get())
            {
                
            }
        }

        public static void loadReviews(StackPanel elem, int ItemID)
        {
            DataTable table = new DB("ULASAN")
                .@select("CUSTOMER.NAMA as namaCust","ULASAN.MESSAGE as MESSAGE", "ULASAN.RATING as RATING", 
                    "TO_CHAR(ULASAN.CREATED_AT,\'DD-MM-YYYY\') as tglUlasan")
                .@join("D_TRANS_ITEM", "ULASAN","ID_D_TRANS_ITEM", "=", "ID")
                .@join("ITEM", "D_TRANS_ITEM","ID_ITEM", "=", "ID")
                .@join("H_TRANS_ITEM","D_TRANS_ITEM","ID_H_TRANS_ITEM","=","ID")
                .@join("CUSTOMER","H_TRANS_ITEM","ID_CUSTOMER","=","ID")
                .@where("ITEM.ID",ItemID.ToString()).get(true);
            
            foreach (DataRow row in table.Rows)
            {
                MessageBox.Show(row["namaCust"].ToString());
                ReviewCard rc = new ReviewCard();
                rc.setCust(row["namaCust"].ToString());
                rc.setDateCust(row["tglUlasan"].ToString());
                rc.setRating(Convert.ToInt32(row["RATING"]));
                rc.setDescription(row["MESSAGE"].ToString());
                // IF ADA
                rc.setSeller("Boodie");
                rc.setSellerDate("10-10-2069");
                rc.setSellerReply("Oh MY God");
                // rc.setSellerImage();
                elem.Children.Add(rc);


            }
        }
    }
}