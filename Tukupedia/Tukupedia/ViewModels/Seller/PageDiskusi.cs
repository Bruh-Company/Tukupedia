using Tukupedia.Models;
using Tukupedia.Views.Seller;
using Tukupedia.Helpers.DatabaseHelpers;
using System.Data;
using System.Windows.Media;
using System.Windows.Controls;
using System;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Tukupedia.Helpers.Utils;
using System.Windows;
using Oracle.DataAccess.Client;
using Tukupedia.Components;

namespace Tukupedia.ViewModels.Seller {
    public class PageDiskusi {
        private SellerView ViewComponent;
        private DataRow seller;

        public PageDiskusi(SellerView viewComponent, DataRow seller, string itemId) {
            ViewComponent = viewComponent;
            this.seller = seller;
            
            initPageDiskusi(itemId);
        }

        public void initPageDiskusi(string id) {
            H_DiskusiModel model = new H_DiskusiModel();
            Canvas elem = ViewComponent.canvasDiskusi;
            model.addWhere("ID_ITEM", id.ToString());
            model.addOrderBy("CREATED_AT ASC");
            foreach (DataRow row in model.get()) {
                DiscussionCard dc = new DiscussionCard(elem.ActualWidth);
                DataRow customer = new DB("CUSTOMER").select().@where("ID", row["ID_CUSTOMER"].ToString()).getFirst();
                dc.initMainComment(
                    message: row["MESSAGE"].ToString(),
                    commenterName: customer["NAMA"].ToString(),
                    date: Utility.formatDate(row["CREATED_AT"].ToString()),
                    url: customer["IMAGE"].ToString()
                    );
                dc.initComments(Convert.ToInt32(row["ID"]));
                elem.Children.Add(dc);
            }
        }
    }
}
