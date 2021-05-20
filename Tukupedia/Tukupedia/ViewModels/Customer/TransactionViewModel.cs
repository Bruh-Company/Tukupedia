using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Tukupedia.Helpers.Classes;
using Tukupedia.Helpers.DatabaseHelpers;
using Tukupedia.Helpers.Utils;
using Tukupedia.Models;
using Tukupedia.Views.Customer;

namespace Tukupedia.ViewModels.Customer
{
    public class TransactionViewModel
    {
        public static List<Header_Trans_Item> list_htrans;
        public static List<Detail_Trans_Item> list_dtrans;
        public static CustomerView ViewComponent;
        public static void initTransaction(CustomerView cv)
        {
            ViewComponent = cv;
        }
        public static void initH_Trans()
        {
            list_htrans = new List<Header_Trans_Item>();
            H_Trans_ItemModel hti = new H_Trans_ItemModel();
            foreach (DataRow item in hti.Table.Select($"ID_CUSTOMER ='{Session.User["ID"]}'","TANGGAL_TRANSAKSI ASC"))
            {
                Header_Trans_Item row = new Header_Trans_Item(
                    iD:item["ID"].ToString(),
                    kODE:item["KODE"].ToString(),
                    tANGGAL_TRANSAKSI: Utility.formatDate(item["TANGGAL_TRANSAKSI"].ToString()),
                    sTATUS: item["STATUS"].ToString()
                    );
                list_htrans.Add(row);
            }
            ViewComponent.grid_H_Trans.ItemsSource = null;
            ViewComponent.grid_H_Trans.ItemsSource = list_htrans;
            ViewComponent.grid_H_Trans.Columns[0].Header = "ID";
            ViewComponent.grid_H_Trans.Columns[0].Visibility = Visibility.Hidden;
            ViewComponent.grid_H_Trans.Columns[1].Header = "Kode Transaksi";
            ViewComponent.grid_H_Trans.Columns[2].Header = "Tanggal Transaksi";
            ViewComponent.grid_H_Trans.Columns[3].Header = "Status";
        }
        public static void loadD_Trans(int idx)
        {
            if (idx >= 0)
            {
                //Init Labels
                H_Trans_ItemModel hti = new H_Trans_ItemModel();
                DataRow row = hti.Table.Select($"ID ='{list_htrans[idx].ID}'").FirstOrDefault();
                ViewComponent.tbKodeTrans.Text = row["KODE"].ToString();
                string idPromo = row["ID_PROMO"].ToString() == "" ? "none" : row["ID_PROMO"].ToString();
                if (idPromo != "none")
                {
                    DataRow promo = new DB("PROMO").select().where("ID", idPromo).getFirst();
                    ViewComponent.tbKodePromo.Text = promo["KODE"].ToString();
                    ViewComponent.tbDiskon.Text = Utility.formatNumber(Convert.ToInt32(row["DISKON"]));
                }
                else
                {
                    ViewComponent.tbKodePromo.Text = "-";
                    ViewComponent.tbDiskon.Text = Utility.formatNumber(0);
                }
                ViewComponent.tbStatusTrans.Text = list_htrans[idx].STATUS;
                ViewComponent.tbSubtotal.Text = Utility.formatNumber(Convert.ToInt32(row["SUBTOTAL"]));
                ViewComponent.tbOngkosKirim.Text = Utility.formatNumber(Convert.ToInt32(row["ONGKOS_KIRIM"]));
                ViewComponent.tbGrandtotal.Text = Utility.formatMoney(Convert.ToInt32(row["GRANDTOTAL"]));

                // Init Table D Trans

                list_dtrans = new List<Detail_Trans_Item>();
                D_Trans_ItemModel dti = new D_Trans_ItemModel();
                foreach (DataRow rowDTI in dti.Table.Select($"ID_H_TRANS_ITEM = '{row["ID"]}'"))
                {
                    string namaItem="",namaKurir="";

                    namaItem = new DB("ITEM")
                        .select()
                        .where("ID", rowDTI["ID_ITEM"].ToString())
                        .getFirst()
                        ["NAMA"].ToString();
                    namaKurir = new DB("KURIR")
                        .select()
                        .where("ID", rowDTI["ID_KURIR"].ToString())
                        .getFirst()
                        ["NAMA"].ToString();


                    Detail_Trans_Item dt = new Detail_Trans_Item(
                        id: rowDTI["ID"].ToString(),
                        namaItem: namaItem,
                        jumlah: rowDTI["JUMLAH"].ToString(),
                        namaKurir: namaKurir,
                        status: rowDTI["STATUS"].ToString()
                        );
                    list_dtrans.Add(dt);
                }
                ViewComponent.grid_D_Trans.ItemsSource = null;
                ViewComponent.grid_D_Trans.ItemsSource = list_dtrans;
                ViewComponent.grid_D_Trans.Columns[0].Header = "ID";
                ViewComponent.grid_D_Trans.Columns[1].Header = "Nama Item";
                ViewComponent.grid_D_Trans.Columns[2].Header = "Jumlah";
                ViewComponent.grid_D_Trans.Columns[3].Header = "Kurir";
                ViewComponent.grid_D_Trans.Columns[4].Header = "Status";
                ViewComponent.grid_D_Trans.Columns[0].Visibility = Visibility.Hidden;
                
            }
        }
        public static void bayarTrans(int idx)
        {
            //Check trans apakah sudah dibayar atau belum
            //Jika sudah,  buat btn enable false?
            if (idx >= 0)
            {
                H_Trans_ItemModel hti = new H_Trans_ItemModel();
                DataRow row = hti.Table.Select($"ID ='{list_htrans[idx].ID}'").FirstOrDefault();
                if (row["STATUS"].ToString() == "W")
                {
                    //Bayar
                    row["STATUS"] = "P";
                    hti.update();
                    initH_Trans();
                    MessageBox.Show("Berhasil membayar!");
                }
                else
                {
                    MessageBox.Show("Gagal membayar!");
                }
            }
        }
        public static void cancelTrans(int idx)
        {
            //Check trans apakah sudah dibayar atau belum
            // Kalau sudah bayar tpi mau cancel harus cek kalau d transnya sudah ada yang kirim
            // atau belum, Kemudian kalau cancel h trans, harus cancel di semua dtrans
            //Jika sudah,  tidak boleh di cancel
            if (idx >= 0)
            {
                H_Trans_ItemModel hti = new H_Trans_ItemModel();
                DataRow row = hti.Table.Select($"ID ='{list_htrans[idx].ID}'").FirstOrDefault();
                if (row["STATUS"].ToString() == "W")
                {
                    //Bayar
                    row["STATUS"] = "C";
                    hti.update();
                    initH_Trans();
                    MessageBox.Show("Berhasil membayar!");
                }
                else
                {
                    MessageBox.Show("Gagal Cancel!");
                }
            }
        }
    }
}
