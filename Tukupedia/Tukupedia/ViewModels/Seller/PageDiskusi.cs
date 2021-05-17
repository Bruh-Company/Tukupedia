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

namespace Tukupedia.ViewModels.Seller {
    public class PageDiskusi {
        private SellerView ViewComponent;
        private DataRow seller;

        public PageDiskusi(SellerView viewComponent, DataRow seller) {
            ViewComponent = viewComponent;
            this.seller = seller;
        }

        public void initPageDiskusi() {

        }
    }
}
