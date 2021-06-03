using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Oracle.DataAccess.Client;
using Tukupedia.Helpers.Utils;
using Tukupedia.Models;
using Tukupedia.Components;
using Tukupedia.Helpers.Classes;
using Tukupedia.Helpers.DatabaseHelpers;
using Tukupedia.Views.Customer;

namespace Tukupedia.ViewModels.Customer
{
    public class CartViewModel
    {
        private static DataTable cart;
        private static int id_h_trans_item;
        public static StackPanel spCart;
        public static Label labelHarga;
        public static TextBlock tbSubTotal;
        public static List<ShopCartComponent> list_shopcart;
        public static int grandTotal,hargaSebelumOngkir,ongkos_kirim;
        public static List<Promo> list_promo;
        public static Dictionary<string, bool> list_checked;
        public static ComboBox cbPayment;
        public static Promo promo;
        public static TextBlock tbDiscount;
        public static TextBlock tbErrorPromotion;
        public static int diskon;
        public static CustomerView ViewComponent;
        

        public static void initCart(CustomerView view)
        {
            ViewComponent = view;
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

        public static int getJumlah(int id_item)
        {
            return Convert.ToInt32(cart.Select($"ID_ITEM ='{id_item}'").First()["JUMLAH"]);
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
            return Utility.checkMax(new H_Trans_ItemModel().Table,"ID",0,0,"1=1");
        }
        public static int generateID_D_Trans()
        {
            MessageBox.Show(Utility.checkMax(new D_Trans_ItemModel().Table, "ID", 0, 5, "1=1").ToString());
            return Utility.checkMax(new D_Trans_ItemModel().Table,"ID",0,0,"1=1");
        }

        public static void loadCartItem()
        {
            list_checked = new Dictionary<string, bool>();
            if (list_shopcart!=null&&list_shopcart.Count > 0)
            {
                foreach (ShopCartComponent scc in list_shopcart)
                {
                    list_checked = list_checked.Concat(scc.getCheckedItems().Where(kvp => !list_checked.ContainsKey(kvp.Key)))
                        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                }
            }
            
            ViewComponent.spCart.Children.Clear();
            Dictionary<string, List<DataRow>> toko = new Dictionary<string, List<DataRow>>();
            //Check per Toko
            foreach (DataRow row in cart.Rows)
            {
                DataRow item = new DB("ITEM").@select().@where("ID", row["ID_ITEM"].ToString()).getFirst();
                //MessageBox.Show(item["NAMA"].ToString());
                string id_toko = item["ID_SELLER"].ToString();
                if (toko.ContainsKey(id_toko))
                {
                    toko[id_toko].Add(item);
                    //MessageBox.Show("Item masuk ke toko yang sama");
                }
                else
                {
                    toko[id_toko] = new List<DataRow>();
                    toko[id_toko].Add(item);
                    //MessageBox.Show("Item masuk ke toko baru");
                }
                
            }

            list_shopcart = new List<ShopCartComponent>();
            foreach (var val in toko)
            {
                ShopCartComponent scc = new ShopCartComponent();
                scc.initToko(new DB("SELLER").@select().@where("ID",val.Key).getFirst());
                scc.addItemCart(val.Value);
                list_shopcart.Add(scc);
                ViewComponent.spCart.Children.Add(scc);
            }

            foreach (var shopCart in list_shopcart)
            {
                shopCart.setCheckedItems(list_checked);
            }
        }

        public static void initHargaCart(Label total, TextBlock tbHarga)
        {
            labelHarga = total;
            tbSubTotal = tbHarga;
        }

        public static void updateHarga(int qty, int harga, int discount)
        {
            string desc = qty > 1 ? "Items" : "Item";
            labelHarga.Content = $"Total Price ({qty} {desc})";
            tbSubTotal.Text = Utility.formatMoney(harga);
            tbDiscount.Text = Utility.formatMoney(discount);
        }

        public static void updateJumlah(int id_item, int jml)
        {
            DataRow row = cart.Select($"ID_ITEM='{id_item}'").FirstOrDefault();
            if (row != null)
            {
                row["JUMLAH"] = jml;
            }
        }

        public static void deleteItemFromCart(int id_item, bool update=true)
        {
            DataRow row = cart.Select($"ID_ITEM='{id_item}'").FirstOrDefault();
            cart.Rows.Remove(row);
            if (update)
            {
                loadCartItem();
                updateGrandTotal();
            }
        }

        public static void updateGrandTotal()
        {
            grandTotal = 0;
            int qty = 0;
            hargaSebelumOngkir = 0;
            ongkos_kirim = 0;
            foreach (ShopCartComponent scc in list_shopcart)
            {
                grandTotal += scc.getSubtotal();
                // MessageBox.Show(scc.getSubtotal().ToString());
                hargaSebelumOngkir += scc.getHargaSebelumOngkir();
                 ongkos_kirim += scc.getOngkir();
                qty += scc.getQuantity();
            }
            //Cari Diskon berapa
            diskon = 0;
            if (promo != null)
            {
                if (promo.JENIS_POTONGAN == "P")
                {
                    diskon = Convert.ToInt32((Convert.ToDouble(promo.POTONGAN) * Convert.ToDouble(hargaSebelumOngkir)) / 100.0);
                    diskon = Math.Min(promo.POTONGAN_MAX, diskon);
                }
                else
                {
                    diskon = promo.POTONGAN;
                }
            }
            
            updateHarga(qty,grandTotal,diskon);
        }
        public static void proceedToCheckout()
        {
            //Validasi Promo + Validasi Ada checked + kurir + payment
            //Validasi Ada barang, 
            string message = validateProceedCheckout();
            if (message == "Success")
            {
                App.openConnection(out _);
                using(OracleTransaction Transaction = App.connection.BeginTransaction())
                {
                    try
                    {
                        Dictionary<string, int> updateStok = new Dictionary<string, int>();
                        List<string> id_items = new List<string>();
                        H_Trans_ItemModel hti = new H_Trans_ItemModel();
                        DataRow rowHTI = hti.Table.NewRow();
                        ComboBoxItem cbiPembayaran = (ComboBoxItem)ViewComponent.cbPaymentMethod.SelectedItem;
                        Metode_PembayaranModel mpm = new Metode_PembayaranModel();
                        DataRow dr = mpm.Table.Select($"NAMA = '{cbiPembayaran.Content}'").First();
                        rowHTI["ID"] = 0;
                        rowHTI["ID_CUSTOMER"] = Session.User["ID"].ToString();
                        rowHTI["TANGGAL_TRANSAKSI"] = DateTime.Now;
                        if (checkPromotion(promo, false))
                        {
                            rowHTI["ID_PROMO"] = promo.ID;
                        }
                        //grandtotal = hargasebelumongkir + ongkir - diskon
                        rowHTI["GRANDTOTAL"] = grandTotal-diskon;
                        rowHTI["SUBTOTAL"] = hargaSebelumOngkir;
                        rowHTI["ONGKOS_KIRIM"] = ongkos_kirim;
                        rowHTI["DISKON"] = diskon;
                        rowHTI["ID_METODE_PEMBAYARAN"] = dr["ID"].ToString();
                        rowHTI["STATUS"] = "W";
                        hti.insert(rowHTI);
                        D_Trans_ItemModel dti = new D_Trans_ItemModel();
                        int id_hti = generateID_H_Trans(); 
                        foreach (ShopCartComponent scc in list_shopcart)
                        {
                            foreach (CartComponent cc in scc.getCarts())
                            {
                                DataRow rowDTI = dti.Table.NewRow();
                                rowDTI["ID"] = 0;
                                rowDTI["ID_H_TRANS_ITEM"] = id_hti;
                                rowDTI["ID_ITEM"] = cc.getItemID();
                                id_items.Add( cc.getItemID());
                                rowDTI["JUMLAH"] = cc.getQuantity();
                                rowDTI["ID_KURIR"] = scc.getIDKurir();
                                rowDTI["STATUS"] = "W";
                                if (updateStok.ContainsKey(cc.getItemID()))
                                {
                                    updateStok[cc.getItemID()] += Convert.ToInt32(cc.getQuantity());
                                }
                                else
                                {
                                    updateStok.Add(cc.getItemID(),Convert.ToInt32(cc.getQuantity()));
                                }
                                dti.insert(rowDTI);
                            }
                        }
                        Transaction.Commit();
                        //Update Stok
                        foreach (var val in updateStok)
                        {
                            string id_item = val.Key;
                            int qty = val.Value;
                            // MessageBox.Show(id_item + " - " + qty);
                            ItemModel im = new ItemModel();
                            DataRow item = im.Table.Select($"ID ='{id_item}'").FirstOrDefault();
                            if (item != null)
                            {
                                item["STOK"] = Convert.ToInt32(item["STOK"]) - qty;
                            }
                            im.update();
                        }
                        //TODO tambah ke history trans :) 
                        foreach (string id_item in id_items)
                        {
                            deleteItemFromCart(Convert.ToInt32(id_item),false);
                        }
                        loadCartItem();
                        updateGrandTotal();
                        TransactionViewModel.initH_Trans();
                        
                    }
                    catch (OracleException e)
                    {
                        Transaction.Rollback();
                        MessageBox.Show(e.Message);
                    }
                };
                App.closeConnection(out _);
            }
            else
            {
                MessageBox.Show(message);
            }
            
        }

        public static string validateProceedCheckout()
        {
            int jmlBrg = 0;
            int jmlKurir = 0;
            string message = "Success";
            foreach (ShopCartComponent scc in list_shopcart)
            {
                foreach (CartComponent cc in scc.getCarts())
                {
                    if (scc.getIDKurir() != "") jmlKurir++;
                    jmlBrg++;
                }
            }

            if (jmlBrg <= 0)
            {
                message="Pilihlah minimal 1 barang untuk dibeli!";
            }
            else if (jmlKurir != jmlBrg)
            {
                message = "Pilihlah kurir terlebih dahulu!";
            }
            else if (cbPayment.SelectedIndex<0)
            {
                message = "Pilihlah payment method terlebih dahulu!";
            }
            
            return message;
        }

        public static void initPaymentMethod(ComboBox comboBox)
        {
            Metode_PembayaranModel mpm = new Metode_PembayaranModel();
            comboBox.Items.Clear();
            foreach (DataRow row in mpm.Table.Rows)
            {
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Content = row["NAMA"].ToString();
                cbi.Tag = row["ID"].ToString();
                comboBox.Items.Add(cbi);
            }
            cbPayment = comboBox;
        }

        public static void initPromotion(ComboBox comboBox, TextBlock discount, TextBlock tbError)
        {
            tbDiscount = discount;
            tbErrorPromotion = tbError;
            PromoModel pm = new PromoModel();

            list_promo = new List<Promo>();
            foreach (DataRow row in pm.Table.Rows)
            {
                Promo p = new Promo(
                    id:row["ID"].ToString(),
                    kode: row["KODE"].ToString(),
                    potongan: Convert.ToInt32(row["POTONGAN"]),
                    potonganMax:Convert.ToInt32(row["POTONGAN_MAKS"]),
                    hargaMin:Convert.ToInt32(row["HARGA_MIN"]),
                    jenisPotongan:row["JENIS_POTONGAN"].ToString(),
                    idJenisPromo: Convert.ToInt32(row["ID_JENIS_PROMO"]),
                    tanggalAwal:Utility.dateParse(row["TANGGAL_AWAL"].ToString()),
                    tanggalAkhir:Utility.dateParse(row["TANGGAL_AKHIR"].ToString()),
                    status:row["STATUS"].ToString()
                    );
                list_promo.Add(p);
            }

            comboBox.ItemsSource = list_promo;
            comboBox.SelectedValuePath = "ID";
        }

        public static bool checkPromotion(Promo p, bool toggle=true)
        {
            bool valid = false;
            if (promo != null)
            {
                valid = true;
                foreach (ShopCartComponent scc in list_shopcart)
                {
                    string id_kurir = scc.getIDKurir();
                    string id_payment = cbPayment.SelectedIndex >= 0
                        ? ((ComboBoxItem)cbPayment.SelectedItem).Tag.ToString()
                        : "";
                    foreach (DataRow item in scc.getItems())
                    {
                        valid &= p.checkPromo(item["ID_CATEGORY"].ToString(), id_kurir, item["ID_SELLER"].ToString(), id_payment);
                    }

                    valid &= hargaSebelumOngkir >= p.HARGA_MIN;
                }
                if(toggle)togglePromotionError(valid);
            }
            return valid;
        }
        public static void togglePromotionError(bool valid)
        {
            if (promo != null)
            {
                if (valid) tbErrorPromotion.Visibility = Visibility.Hidden;
                else tbErrorPromotion.Visibility = Visibility.Visible;
            }
        }
        public static void saveCart()
        {
            Session.setCart(Convert.ToInt32(Session.User["ID"]), cart);
        }
        
    }
}