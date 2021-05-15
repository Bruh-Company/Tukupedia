using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Oracle.DataAccess.Client;
using Tukupedia.Helpers.Utils;
using Tukupedia.Models;
using Tukupedia.Components;
using Tukupedia.Helpers.DatabaseHelpers;

namespace Tukupedia.ViewModels.Customer
{
    public class CartViewModel
    {
        private static DataTable cart = new D_Trans_ItemModel().Table.Clone();
        private static int id_h_trans_item;
        public static StackPanel spCart;
        public static Label labelHarga;
        public static TextBlock tbSubTotal;
        
        

        public static void initCart()
        {
            int id_customer = Convert.ToInt32(Session.User["ID"]);
            cart = Session.getCart(id_customer);
            if (cart.Rows.Count == 0)
            {
                id_h_trans_item = generateID_H_Trans();
            }
            else
            {
                id_h_trans_item = Convert.ToInt32(cart.Rows[0]["ID_H_TRANS_ITEM"]);
            }
            
        }
        public static void addtoCart(DataRow item, int jumlah, bool add = false)
        {
            int id_item = Convert.ToInt32(item["ID"]);
            if (checkCart(id_item))
            {
                DataRow itemCart = searchCart(id_item).FirstOrDefault();
                if (itemCart != null)
                {
                    int prev = 0;
                    if (add)
                    {
                        prev = Convert.ToInt32(itemCart["JUMLAH"]);
                    }   
                    itemCart["JUMLAH"] = Math.Min(prev + jumlah,Convert.ToInt32(item["STOK"]));
                }
            }
            else
            {
                DataRow newCart = cart.NewRow();
                //Harus ganti ga boleh 0?
                newCart["ID"] = 0;
                // Cari ID_H_TRANS_ITEM Terbaru
                newCart["ID_H_TRANS_ITEM"] = id_h_trans_item;
                newCart["ID_ITEM"] = item["ID"];
                newCart["JUMLAH"] = jumlah;
                cart.Rows.Add(newCart);
            }
        }

        //Check apakah barang sudah di cart atau tidak
        // Kenapa harus dicek? karena barang yang sama tidak boleh double
        public static bool checkCart(int id_item)
        {
            int result = searchCart(id_item).Length;
            if (result > 0)
            {
                //Kalau Nemu
                return true;
            }
            //Kalau tidak nemu
            return false;
        }

        public static DataRow[] searchCart(int id_item)
        {
            return cart.Select($"ID_ITEM = '{id_item}'");
        }

        public static int generateID_H_Trans()
        {
            // MessageBox.Show(Utility.checkMax(new H_Trans_ItemModel().Table, "ID", 0, 5, "1=1").ToString());
            return Utility.checkMax(new H_Trans_ItemModel().Table,"ID",0,5,"1=1") + 1;
        }
        public static int generateID_D_Trans()
        {
            MessageBox.Show(Utility.checkMax(new D_Trans_ItemModel().Table, "ID", 0, 5, "1=1").ToString());
            return Utility.checkMax(new D_Trans_ItemModel().Table,"ID",0,5,"1=1") + 1;
        }

        public static void loadCartItem(StackPanel sp)
        {
            if(spCart==null)spCart = sp;
            sp.Children.Clear();
            foreach (DataRow row in cart.Rows)
            {
                CartComponent cc = new CartComponent();
                DataRow item = new DB("ITEM").@select().@where("ID", row["ID_ITEM"].ToString()).getFirst();
                cc.iniComponent(item,Convert.ToInt32(row["JUMLAH"]));
                sp.Children.Add(cc);
            }
        }

        public static void initHargaCart(Label total, TextBlock tbHarga)
        {
            labelHarga = total;
            tbSubTotal = tbHarga;
        }

        public static void countSubTotal()
        {
            int total = 0;
            int qty = 0;
            foreach (DataRow row in cart.Rows)
            {
                DataRow item = new DB("ITEM").@select().@where("ID", row["ID_ITEM"].ToString()).getFirst();
                total += (Convert.ToInt32(item["HARGA"]) * Convert.ToInt32(row["JUMLAH"]));
                qty += Convert.ToInt32(row["JUMLAH"]);
            }
            updateHarga(qty,total);
        }
        public static void updateHarga(int qty, int harga)
        {
            string desc = qty > 0 ? "Items" : "Item";
            labelHarga.Content = $"Total Price ({qty} {desc})";
            tbSubTotal.Text = Utility.formatMoney(harga);
        }

        public static void updateJumlah(int id_item, int jml)
        {
            DataRow row = cart.Select($"ID_ITEM='{id_item}'").FirstOrDefault();
            if (row != null)
            {
                row["JUMLAH"] = jml;
            }
        }

        public static void deleteItemFromCart(int id_item)
        {
            DataRow row = cart.Select($"ID_ITEM='{id_item}'").FirstOrDefault();
            cart.Rows.Remove(row);
            loadCartItem(spCart);
            countSubTotal();
        }
        public static void proceedToCheckout()
        {
            using(OracleTransaction Transaction = App.connection.BeginTransaction())
            {
                try
                {
                    H_Trans_ItemModel hti = new H_Trans_ItemModel();
                    DataRow row = hti.Table.NewRow();
                    row["ID"] = 0;
                    row["ID_CUSTOMER"] = Session.User["ID"].ToString();
                    
                    hti.insert();
                }
                catch (OracleException e)
                {
                    Transaction.Rollback();
                    MessageBox.Show(e.Message);
                }
            };
            
        }
    }
}