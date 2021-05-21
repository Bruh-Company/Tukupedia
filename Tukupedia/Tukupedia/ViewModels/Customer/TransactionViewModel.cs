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
        private static int idxH_Trans;
        private static int idxItem;
        public static void initTransaction(CustomerView cv)
        {
            ViewComponent = cv;
        }
        public static void initH_Trans()
        {
            resetItem();
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
                idxH_Trans = idx;
                H_Trans_ItemModel hti = new H_Trans_ItemModel();
                DataRow row = hti.Table.Select($"ID ='{list_htrans[idx].ID}'").FirstOrDefault();
                if (row["STATUS"].ToString() == "W")
                {
                    //Bayar
                    row["STATUS"] = "P";
                    hti.update();
                    initH_Trans();
                    MessageBox.Show("Transaksi berhasil di bayar !");
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
                idxH_Trans = idx;
                H_Trans_ItemModel hti = new H_Trans_ItemModel();
                DataRow row = hti.Table.Select($"ID ='{list_htrans[idx].ID}'").FirstOrDefault();
                if (row["STATUS"].ToString() == "W")
                {
                    //Check kalau sudah ada yang S 
                    D_Trans_ItemModel dti = new D_Trans_ItemModel();
                    bool valid = true;
                    foreach (DataRow rowDTI in dti.Table.Select($"ID_H_TRANS_ITEM = '{row["ID"]}'"))
                    {
                        if (rowDTI["STATUS"].ToString() != "W")
                        {
                            valid = false;
                        }
                    }
                    if (valid)
                    {
                        //Cancel
                        row["STATUS"] = "C";
                        foreach (DataRow rowDTI in dti.Table.Select($"ID_H_TRANS_ITEM = '{row["ID"]}'"))
                        {
                            rowDTI["STATUS"] = "C";
                        }
                        dti.update();
                        hti.update();
                        initH_Trans();
                        loadD_Trans(idx);
                        MessageBox.Show("Transaksi berhasil di cancel !");
                    }
                    else
                    {
                        MessageBox.Show("Tidak bisa dicancel !");
                    }
                }
                else
                {
                    MessageBox.Show("Gagal Cancel !");
                }
            }
        }
        public static void resetItem()
        {
            ViewComponent.tbKurirItem.Text = "-";
            ViewComponent.tbNamaItem.Text = "-";
            ViewComponent.tbJumlahItem.Text = "-";
            ViewComponent.tbStatusItem.Text = "-";
        }
        public static void loadItem(int idx)
        {
            if (idx >= 0)
            {
                idxItem = idx;
                Detail_Trans_Item dti = list_dtrans[idx];
                ViewComponent.tbKurirItem.Text = dti.namaKurir;
                ViewComponent.tbNamaItem.Text = dti.namaItem;
                ViewComponent.tbJumlahItem.Text = dti.jumlah;
                ViewComponent.tbStatusItem.Text = dti.status;
            }
        }
        public static void beriUlasan(int idx)
        {
            if (idx >= 0)
            {
                idxItem = idx;
                Detail_Trans_Item dti = list_dtrans[idx];
                //Check kalau DTRANS HARUS SUDAH SELESAI
                if (dti.status == "FINISHED")
                {
                    UlasanModel um = new UlasanModel();
                    if (um.Table.Select($"ID_D_TRANS_ITEM = '{dti.id}' AND ID_CUSTOMER = '{Session.User["ID"]}'").Length > 0)
                    {
                        MessageBox.Show("Barang sudah di ulas!");
                    }
                    else
                    {
                        //TODO KENAPA KOK PAKAI UM TIDAK BISAAAA
                        //DataRow row = um.Table.NewRow();
                        //row["ID"] = 0;
                        //row["ID_CUSTOMER"] = Session.User["ID"].ToString();
                        //row["MESSAGE"] = Utility.StringFromRichTextBox(ViewComponent.rtbUlasan);
                        //row["RATING"] = ViewComponent.ratingUlasan.Value;
                        //row["ID_D_TRANS_ITEM"] = dti.id;
                        //um.Table.Rows.Add(row);
                        //um.update();
                        //um.insert(
                        //    "ID",0,
                        //    "ID_CUSTOMER", Session.User["ID"].ToString(),
                        //    "MESSAGE", Utility.StringFromRichTextBox(ViewComponent.rtbUlasan),
                        //    "RATING", ViewComponent.ratingUlasan.Value,
                        //    "ID_D_TRANS_ITEM", dti.id
                        //    );
                        new DB("ULASAN").insert(
                            "ID", 0,
                            "ID_CUSTOMER", Session.User["ID"].ToString(),
                            "MESSAGE", Utility.StringFromRichTextBox(ViewComponent.rtbUlasan),
                            "RATING", ViewComponent.ratingUlasan.Value,
                            "ID_D_TRANS_ITEM", dti.id
                            ).execute();
                        //Kalau pake DB, harus reset ulang Data Table

                        //reset input ulasan
                        resetItem();
                        MessageBox.Show("Berhasil Memberi Ulasan!");

                    }
                }
                else
                {
                    // Selain selesai
                    MessageBox.Show("ERROR");
                }
            }
        }
    }
}
