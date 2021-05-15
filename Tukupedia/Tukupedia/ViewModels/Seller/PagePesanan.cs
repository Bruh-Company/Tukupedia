using System;
using System.Data;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using Tukupedia.Helpers.DatabaseHelpers;
using Tukupedia.Helpers.Utils;
using Tukupedia.Models;
using Tukupedia.Views.Seller;

namespace Tukupedia.ViewModels.Seller
{
    public class PagePesanan
    {
        private SellerView ViewComponent;
        private ItemModel itemModel;
        private bool toggleBtnInsertProduk = true;
        private DataRow seller;

        private D_Trans_ItemModel htrans;
        private D_Trans_ItemModel forselectdata;
        private D_Trans_ItemModel dtrans_helper;
        private D_Trans_ItemModel dtrans;
        int h_trans_selected = -1, d_trans_selected;
        string status = "";

        public PagePesanan(SellerView viewComponent, DataRow seller)
        {
            ViewComponent = viewComponent;
            this.seller = seller;
            htrans = new D_Trans_ItemModel();
            forselectdata = new D_Trans_ItemModel();
            dtrans = new D_Trans_ItemModel();
            dtrans_helper = new D_Trans_ItemModel();
        }

        public void initPagePesanan()
        {
            seller = Session.User;

        }
        public void viewSemuaPesanan()
        {
            htrans.initAdapter($"select distinct h.KODE as \"Kode Pesanan\", c.NAMA as \"Nama Customer\", sum(i.HARGA * d.JUMLAH) as \"Total\", h.TANGGAL_TRANSAKSI as \"Tanggal Transaksi\" from H_TRANS_ITEM h, D_TRANS_ITEM d, CUSTOMER c, ITEM i where i.ID = d.ID_ITEM and h.ID = d.ID_H_TRANS_ITEM and c.ID = h.ID_CUSTOMER and i.ID_SELLER = '{seller["ID"]}' group by h.KODE, c.NAMA, h.TANGGAL_TRANSAKSI order by h.TANGGAL_TRANSAKSI desc");
            status = "";
            ViewComponent.datagridPesanan.ItemsSource = htrans.Table.DefaultView;
            ViewComponent.canvasDetailPesanan.Visibility = System.Windows.Visibility.Hidden;

        }
        public void viewPesananBaru()
        {
            htrans.initAdapter($"select distinct h.KODE as \"Kode Pesanan\", c.NAMA as \"Nama Customer\", sum(i.HARGA * d.JUMLAH) as \"Total\", h.TANGGAL_TRANSAKSI as \"Tanggal Transaksi\" from H_TRANS_ITEM h, D_TRANS_ITEM d, CUSTOMER c, ITEM i where i.ID = d.ID_ITEM and h.ID = d.ID_H_TRANS_ITEM and c.ID = h.ID_CUSTOMER and i.ID_SELLER = '{seller["ID"]}' and d.STATUS = 'W' group by h.KODE, c.NAMA, h.TANGGAL_TRANSAKSI order by h.TANGGAL_TRANSAKSI desc");
            ViewComponent.datagridPesanan.ItemsSource = htrans.Table.DefaultView;
            ViewComponent.canvasDetailPesanan.Visibility = System.Windows.Visibility.Hidden;

            status = "and d.STATUS = 'W'";

        }
        public void viewSiapKirim()
        {
            htrans.initAdapter($"select distinct h.KODE as \"Kode Pesanan\", c.NAMA as \"Nama Customer\", sum(i.HARGA * d.JUMLAH) as \"Total\", h.TANGGAL_TRANSAKSI as \"Tanggal Transaksi\" from H_TRANS_ITEM h, D_TRANS_ITEM d, CUSTOMER c, ITEM i where i.ID = d.ID_ITEM and h.ID = d.ID_H_TRANS_ITEM and c.ID = h.ID_CUSTOMER and i.ID_SELLER = '{seller["ID"]}' and d.STATUS = 'W' group by h.KODE, c.NAMA, h.TANGGAL_TRANSAKSI order by h.TANGGAL_TRANSAKSI desc");
            ViewComponent.datagridPesanan.ItemsSource = htrans.Table.DefaultView;
            ViewComponent.canvasDetailPesanan.Visibility = System.Windows.Visibility.Hidden;

            status = "and d.STATUS = 'P'";

        }

        public void viewDalamPengiriman()
        {
            htrans.initAdapter($"select distinct h.KODE as \"Kode Pesanan\", c.NAMA as \"Nama Customer\", sum(i.HARGA * d.JUMLAH) as \"Total\", h.TANGGAL_TRANSAKSI as \"Tanggal Transaksi\" from H_TRANS_ITEM h, D_TRANS_ITEM d, CUSTOMER c, ITEM i where i.ID = d.ID_ITEM and h.ID = d.ID_H_TRANS_ITEM and c.ID = h.ID_CUSTOMER and i.ID_SELLER = '{seller["ID"]}' and d.STATUS = 'W' group by h.KODE, c.NAMA, h.TANGGAL_TRANSAKSI order by h.TANGGAL_TRANSAKSI desc");
            ViewComponent.datagridPesanan.ItemsSource = htrans.Table.DefaultView;
            ViewComponent.canvasDetailPesanan.Visibility = System.Windows.Visibility.Hidden;

            status = "and d.STATUS = 'S'";

        }

        public void viewPesananSelesai()
        {
            htrans.initAdapter($"select distinct h.KODE as \"Kode Pesanan\", c.NAMA as \"Nama Customer\", sum(i.HARGA * d.JUMLAH) as \"Total\", h.TANGGAL_TRANSAKSI as \"Tanggal Transaksi\" from H_TRANS_ITEM h, D_TRANS_ITEM d, CUSTOMER c, ITEM i where i.ID = d.ID_ITEM and h.ID = d.ID_H_TRANS_ITEM and c.ID = h.ID_CUSTOMER and i.ID_SELLER = '{seller["ID"]}' and d.STATUS = 'W' group by h.KODE, c.NAMA, h.TANGGAL_TRANSAKSI order by h.TANGGAL_TRANSAKSI desc");
            ViewComponent.datagridPesanan.ItemsSource = htrans.Table.DefaultView;
            ViewComponent.canvasDetailPesanan.Visibility = System.Windows.Visibility.Hidden;

            status = "and d.STATUS = 'D'";

        }

        public void viewPesananBatal()
        {
            htrans.initAdapter($"select distinct h.KODE as \"Kode Pesanan\", c.NAMA as \"Nama Customer\", sum(i.HARGA * d.JUMLAH) as \"Total\", h.TANGGAL_TRANSAKSI as \"Tanggal Transaksi\"  from H_TRANS_ITEM h, D_TRANS_ITEM d, CUSTOMER c, ITEM i where i.ID = d.ID_ITEM and h.ID = d.ID_H_TRANS_ITEM and c.ID = h.ID_CUSTOMER and i.ID_SELLER = '{seller["ID"]}' and d.STATUS = 'W' group by h.KODE, c.NAMA, h.TANGGAL_TRANSAKSI order by h.TANGGAL_TRANSAKSI desc");
            ViewComponent.datagridPesanan.ItemsSource = htrans.Table.DefaultView;
            ViewComponent.canvasDetailPesanan.Visibility = System.Windows.Visibility.Hidden;

            status = "and d.STATUS = 'C'";

        }

        public void reloadHtrans()
        {
            selectHtrans(h_trans_selected);
        }
        public void selectHtrans(int selected)
        {
            if(selected != -1)
            {
                this.h_trans_selected = selected;
                DataRow dr = htrans.Table.Rows[selected];
                forselectdata = new D_Trans_ItemModel();
                forselectdata.initAdapter($"select h.KODE, to_char(h.TANGGAL_TRANSAKSI,'dd-mm-yyyy'), k.NAMA, c.NAMA, c.ALAMAT from H_TRANS_ITEM h, D_TRANS_ITEM d, KURIR k, CUSTOMER c, ITEM i where d.ID_ITEM = i.ID and h.ID_CUSTOMER = c.ID and d.ID_H_TRANS_ITEM = h.ID and h.KODE = '{dr[0].ToString()}' {status}");
                dr = forselectdata.Table.Rows[0];
                ViewComponent.labelKodePesanan.Content = dr[0].ToString();
                ViewComponent.labelTanggalTransaksi.Content = dr[1].ToString();
                ViewComponent.labelKurir.Content = dr[2].ToString();
                ViewComponent.labelNamaPembeli.Content = dr[3].ToString();
                ViewComponent.textboxAlamatPesanan.Text = dr[4].ToString();
                ViewComponent.textboxAlamatPesanan.IsReadOnly = true;
                dtrans = new D_Trans_ItemModel();
                dtrans.initAdapter($"select d.JUMLAH as \"Jumlah\", i.NAMA as \"Nama Barang\", i.HARGA as \"Harga\", i.HARGA * d.JUMLAH as \"Total\", case d.STATUS when 'W' then 'Pesanan Baru' when 'P' then 'Siap Kirim' when 'S' then 'Dalam Pengiriman' when 'D' then 'Pesanan Selesai' when 'C' then 'Pesanan Dibatalkan' end as \"Status\" from ITEM i, H_TRANS_ITEM h, D_TRANS_ITEM d where h.ID = d.ID_H_TRANS_ITEM and i.ID = d.ID_ITEM and h.KODE = '{dr[0].ToString()}' {status}");
                dtrans_helper = new D_Trans_ItemModel();
                dtrans_helper.initAdapter($"select d.ID , d.STATUS, i.STOK, d.JUMLAH from ITEM i, H_TRANS_ITEM h, D_TRANS_ITEM d where h.ID = d.ID_H_TRANS_ITEM and i.ID = d.ID_ITEM and h.KODE = '{dr[0].ToString()}' {status}");
                ViewComponent.datagridProdukPesanan.ItemsSource = dtrans.Table.DefaultView;
                ViewComponent.canvasDetailPesanan.Visibility = System.Windows.Visibility.Visible;
            }
        }

        public void selectDtrans(int selected)
        {
            if (selected != -1)
            {
                this.d_trans_selected = selected;
                DataRow dr = dtrans.Table.Rows[selected];
                ViewComponent.labelStatusPesanan.Content = dr[4].ToString();
                dr = dtrans_helper.Table.Rows[selected];
                string statuses = dr[1].ToString();
                //- W = Pesanan Baru
                //- S = Dalam Pengiriman
                //- SC = Dalam Pengiriman blom confirm

                //- D = Pesanan Selesai
                //- [//baru](//baru) untuk admin
                //-C = Pesanan Dibatalkan
                //-CC = Pesanan Dibatalkan blom confirm
                //if (statuses == "W" || statuses == "SC" || statuses == "CC")
                //{
                //    ViewComponent.btnSuccessPesanan.Visibility = System.Windows.Visibility.Visible;
                //    ViewComponent.btnBatalPesanan.Visibility = System.Windows.Visibility.Visible;
                //}
                //else
                //{
                //    ViewComponent.btnSuccessPesanan.Visibility = System.Windows.Visibility.Hidden;
                //    ViewComponent.btnBatalPesanan.Visibility = System.Windows.Visibility.Hidden;
                //}
            }
        }
        public void toggleDtrans()
        {
            d_trans_selected = ViewComponent.datagridProdukPesanan.SelectedIndex;
            if (d_trans_selected == -1) return;
            DataRow dr = dtrans.Table.Rows[d_trans_selected];
            ViewComponent.labelStatusPesanan.Content = dr[4].ToString();
            DataRow drHelper = dtrans_helper.Table.Rows[d_trans_selected];
            string statuses = drHelper[1].ToString();
            if (statuses == "W")
            {
                drHelper[1] = "SC";
                dr[4] = "Dalam Pengiriman *";
            }
            else if (statuses == "C" || statuses == "D" || statuses == "S") ;
            else
            {
                drHelper[1] = "W";
                dr[4] = "Pesanan Baru";
            }
            //- W = Pesanan Baru
            //- S = Dalam Pengiriman
            //- SC = Dalam Pengiriman blom confirm

            //- D = Pesanan Selesai
            //- [//baru](//baru) untuk admin
            //-C = Pesanan Dibatalkan
            ViewComponent.datagridProdukPesanan.ItemsSource = "";
            ViewComponent.datagridProdukPesanan.ItemsSource = dtrans.Table.DefaultView;

        }
        public void terimasemua(bool ya)
        {
            foreach (DataRow dr in dtrans_helper.Table.Rows)
            {
                if(ya)dr[1] = "SC";
                else dr[1] = "W";
            }
            foreach (DataRow dr in dtrans.Table.Rows)
            {
                if (ya) dr[4] = "Dalam Pengiriman *";
                else dr[4] = "Pesanan Baru";
            }
            ViewComponent.datagridProdukPesanan.ItemsSource = "";
            ViewComponent.datagridProdukPesanan.ItemsSource = dtrans.Table.DefaultView;
        }
        public void confirm()
        {
            int tidakterima = 0;
            //check stock
            for (int i = 0; i < dtrans_helper.Table.Rows.Count; i++)
            {
                DataRow dr = dtrans.Table.Rows[i], drHelper = dtrans_helper.Table.Rows[i];
                //2 = Stock
                //3 = Barang Yang dibeli
                if (Convert.ToInt32(drHelper[2].ToString()) < Convert.ToInt32(dr[3].ToString()))
                {
                    MessageBox.Show($"Barang {dr[1].ToString()} tidak mencukupi, transaksi gagal");
                    return;
                }
            }
            
            foreach (DataRow dr in dtrans_helper.Table.Rows)
            {
                if (dr[1].ToString() == "W") tidakterima++;
            }
            if (tidakterima != 0)
            {
                DialogResult d = MessageBox.Show($"Ada {tidakterima} pesanan yang belum diterima, Yakin lanjut ?", "Pesanan Blom di terima", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (d == DialogResult.No)
                {
                    return;
                }
            }
            foreach (DataRow dr in dtrans_helper.Table.Rows)
            {
                new DB("D_TRANS_ITEM").update("STATUS","S").where("ID",dr[0].ToString()).execute();
                
            }
        }

        public void checkStatus(DataGridRow dgRow)
        {
            DataRowView item = dgRow.Item as DataRowView;
            if (item != null)
            {
                //dtrans.initAdapter($"select d.JUMLAH as \"Jumlah\", i.NAMA as \"Nama Barang\", i.HARGA as \"Harga\", i.HARGA * d.JUMLAH as \"Total\", case d.STATUS when
                //'W' then 'Pesanan Baru' when
                //'P' then 'Siap Kirim' when
                //'S' then 'Dalam Pengiriman' when
                //'D' then 'Pesanan Selesai' when
                //'C' then 'Pesanan Dibatalkan' end as \"Status\" from ITEM i, H_TRANS_ITEM h, D_TRANS_ITEM d where h.ID = d.ID_H_TRANS_ITEM and i.ID = d.ID_ITEM and h.KODE = '{dr[0].ToString()}' {status}");
                DataRow row = item.Row;
                string status = row["Status"].ToString();
                if (status == "Pesanan Baru" || status == "Pesanan Dibatalkan")
                {
                    Color color = (Color)ColorConverter.ConvertFromString("#E23434");
                    dgRow.Background = new SolidColorBrush(color);
                }
                else
                {
                    Color color = (Color)ColorConverter.ConvertFromString("#65C65B");
                }
            }
        }
        public void updateStatus(string status)
        {
            //string id = forid.Table.Rows[0][0].ToString();
            //new DB("D_TRANS_ITEM")
        }
    }
}
