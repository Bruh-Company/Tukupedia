using System.Data;
using Tukupedia.Models;

namespace Tukupedia.ViewModels.Customer
{
    public class CartViewModel
    {
        private static DataTable cart = new D_Trans_ItemModel().Table.Clone();

        public static void clearCart()
        {
            cart = new D_Trans_ItemModel().Table.Clone();
        }
        public static void addtoCart(DataRow item, int jumlah)
        {
            DataRow newCart = cart.NewRow();
            //Harus ganti ga boleh 0?
            newCart["ID"] = 0;
            // Cari ID_H_TRANS_ITEM Terbaru
            newCart["ID_H_TRANS_ITEM"] = 0;
            newCart["ID_ITEM"] = item["ID"];
            newCart["JUMLAH"] = jumlah;
            cart.Rows.Add(newCart);
        }
    }
}