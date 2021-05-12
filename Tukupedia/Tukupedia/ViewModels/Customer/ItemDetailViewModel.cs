using System.Data;
using System.Windows;
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

        public static void loadReviews(FrameworkElement elem, int ItemID)
        {
            DataTable table = new DB("ULASAN").@select()
                .@join("D_TRANS_ITEM", "ULASAN.ID_D_TRANS_ITEM", "=", "D_TRANS_ITEM.ID")
                .@join("ITEM", "ID_ITEM", "=", "ID")
                .@where("ITEM.ID",ItemID.ToString()).get();
            foreach (DataRow row in table.Rows)
            {
                
                ReviewCard rc = new ReviewCard();
            }
        }
    }
}