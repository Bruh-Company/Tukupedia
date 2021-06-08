using System;
using System.Data;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using Tukupedia.Helpers.DatabaseHelpers;
using Tukupedia.Helpers.Utils;
using Tukupedia.Models;
using Tukupedia.Report;
using Tukupedia.Views;
using Tukupedia.Views.Seller;

namespace Tukupedia.ViewModels.Seller
{
    public class PagePesanan
    {
        //duek ditambah, stok dikurangi
        private SellerView ViewComponent;
        private DataRow seller;

        private D_Trans_ItemModel htrans;
        private D_Trans_ItemModel forselectdata;
        private D_Trans_ItemModel dtrans_helper;
        private D_Trans_ItemModel dtrans;
        //private KurirModel km;
        int h_trans_selected = -1, d_trans_selected;
        string status = "";
        int lastclick = -1;
        string orderby = "order by h.TANGGAL_TRANSAKSI desc";
        string query = "";

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
            viewSemuaPesanan();
            ViewComponent.comboboxSortPesanan.Items.Add("Urutkan dari Transaksi Terbaru");
            ViewComponent.comboboxSortPesanan.Items.Add("Urutkan dari Transaksi Terlama");
            ViewComponent.comboboxSortPesanan.Items.Add("Urutkan dari Nama Customer A-Z");
            ViewComponent.comboboxSortPesanan.Items.Add("Urutkan dari Nama Customer Z-A");
            ViewComponent.comboboxSortPesanan.Items.Add("Urutkan dari Kode Pesanan A-Z");
            ViewComponent.comboboxSortPesanan.Items.Add("Urutkan dari Kode Pesanan Z-A");
            ViewComponent.comboboxSortPesanan.SelectedIndex = 0;


        }
        public void orderBy()
        {
            int choose = ViewComponent.comboboxSortPesanan.SelectedIndex;
            if(choose == 0)
            {
                orderby = "order by h.TANGGAL_TRANSAKSI desc";
            }
            else if (choose == 1)
            {
                orderby = "order by h.TANGGAL_TRANSAKSI asc";
            }
            else if (choose == 2)
            {
                orderby = "order by c.NAMA asc";
            }
            else if (choose == 3)
            {
                orderby = "order by c.NAMA desc";
            }
            else if (choose == 4)
            {
                orderby = "order by h.KODE asc";
            }
            else if (choose == 5)
            {
                orderby = "order by h.KODE desc";
            }
            reloadInternal();
        }

        void reloadInternal()
        {
            if (lastclick == 1) viewSemuaPesanan(query);
            if (lastclick == 2) viewPesananBaru(query);
            if (lastclick == 3) viewDalamPengiriman(query);
            if (lastclick == 4) viewPesananSelesai(query);
            if (lastclick == 5) viewPesananBatal(query);
        }
        public void cariPesanan()
        {
            string keyword = ViewComponent.textboxCariPesanan.Text;
            if (keyword == "")query = "";
            else query = $"AND lower(c.NAMA) like lower('%{keyword}%') OR lower(h.KODE) like lower('%{keyword}%')";
            reloadInternal();
        }
        private void toCurrencyHtrans()
        {
            foreach(DataRow dr in htrans.Table.Rows){
                dr[2] = Utility.formatMoney(Convert.ToInt32(dr[2].ToString()));
            }
            
        }

        public void viewSemuaPesanan(string keyword = "")
        {
            lastclick = 1;
            ViewComponent.textboxCariPesanan.Text = "";
            htrans.initAdapter($"select distinct h.KODE as \"Kode Pesanan\", c.NAMA as \"Nama Customer\", to_char(sum(i.HARGA * d.JUMLAH)) as \"Total\", h.TANGGAL_TRANSAKSI as \"Tanggal Transaksi\", k.NAMA as \"Kurir\", case h.STATUS when 'C' then 'Canceled' when 'W' then 'Waiting Payment' when 'P' then 'Paid' end as \"Status\" from H_TRANS_ITEM h, D_TRANS_ITEM d, CUSTOMER c, ITEM i, KURIR k where i.ID = d.ID_ITEM and h.ID = d.ID_H_TRANS_ITEM and c.ID = h.ID_CUSTOMER and i.ID_SELLER = '{seller["ID"]}' and k.ID = d.ID_KURIR {keyword} group by h.KODE, c.NAMA, h.TANGGAL_TRANSAKSI, k.NAMA, h.STATUS {orderby}");
            toCurrencyHtrans();
            status = "";
            ViewComponent.datagridPesanan.ItemsSource = htrans.Table.DefaultView;
            ViewComponent.canvasDetailPesanan.Visibility = System.Windows.Visibility.Hidden;

        }
        public void viewPesananBaru(string keyword = "")
        {
            lastclick = 2;
            ViewComponent.textboxCariPesanan.Text = "";

            htrans.initAdapter($"select distinct h.KODE as \"Kode Pesanan\", c.NAMA as \"Nama Customer\", to_char(sum(i.HARGA * d.JUMLAH)) as \"Total\", h.TANGGAL_TRANSAKSI as \"Tanggal Transaksi\", k.NAMA as \"Kurir\", case h.STATUS when 'C' then 'Canceled' when 'W' then 'Waiting Payment' when 'P' then 'Paid' end as \"Status\" from H_TRANS_ITEM h, D_TRANS_ITEM d, CUSTOMER c, ITEM i, KURIR k where i.ID = d.ID_ITEM and h.ID = d.ID_H_TRANS_ITEM and c.ID = h.ID_CUSTOMER and i.ID_SELLER = '{seller["ID"]}' and k.ID = d.ID_KURIR and d.STATUS = 'W' {keyword} group by h.KODE, c.NAMA, h.TANGGAL_TRANSAKSI, k.NAMA, h.STATUS {orderby}");
            toCurrencyHtrans();

            ViewComponent.datagridPesanan.ItemsSource = htrans.Table.DefaultView;
            ViewComponent.canvasDetailPesanan.Visibility = System.Windows.Visibility.Hidden;

            status = "and d.STATUS = 'W'";

        }
        public void viewSiapKirim(string keyword = "")
        {
            lastclick = 3;
            ViewComponent.textboxCariPesanan.Text = "";

            htrans.initAdapter($"select distinct h.KODE as \"Kode Pesanan\", c.NAMA as \"Nama Customer\", to_char(sum(i.HARGA * d.JUMLAH)) as \"Total\", h.TANGGAL_TRANSAKSI as \"Tanggal Transaksi\", k.NAMA as \"Kurir\", case h.STATUS when 'C' then 'Canceled' when 'W' then 'Waiting Payment' when 'P' then 'Paid' end as \"Status\" from H_TRANS_ITEM h, D_TRANS_ITEM d, CUSTOMER c, ITEM i, KURIR k where i.ID = d.ID_ITEM and h.ID = d.ID_H_TRANS_ITEM and c.ID = h.ID_CUSTOMER and i.ID_SELLER = '{seller["ID"]}' and k.ID = d.ID_KURIR and d.STATUS = 'P' {keyword} group by h.KODE, c.NAMA, h.TANGGAL_TRANSAKSI, k.NAMA, h.STATUS {orderby}");
            toCurrencyHtrans();

            ViewComponent.datagridPesanan.ItemsSource = htrans.Table.DefaultView;
            ViewComponent.canvasDetailPesanan.Visibility = System.Windows.Visibility.Hidden;

            status = "and d.STATUS = 'P'";

        }

        public void viewDalamPengiriman(string keyword = "")
        {
            lastclick = 3;
            ViewComponent.textboxCariPesanan.Text = "";

            htrans.initAdapter($"select distinct h.KODE as \"Kode Pesanan\", c.NAMA as \"Nama Customer\", to_char(sum(i.HARGA * d.JUMLAH)) as \"Total\", h.TANGGAL_TRANSAKSI as \"Tanggal Transaksi\", k.NAMA as \"Kurir\", case h.STATUS when 'C' then 'Canceled' when 'W' then 'Waiting Payment' when 'P' then 'Paid' end as \"Status\" from H_TRANS_ITEM h, D_TRANS_ITEM d, CUSTOMER c, ITEM i, KURIR k where i.ID = d.ID_ITEM and h.ID = d.ID_H_TRANS_ITEM and c.ID = h.ID_CUSTOMER and i.ID_SELLER = '{seller["ID"]}' and k.ID = d.ID_KURIR and d.STATUS = 'S' {keyword} group by h.KODE, c.NAMA, h.TANGGAL_TRANSAKSI, k.NAMA, h.STATUS {orderby}");
            toCurrencyHtrans();

            ViewComponent.datagridPesanan.ItemsSource = htrans.Table.DefaultView;
            ViewComponent.canvasDetailPesanan.Visibility = System.Windows.Visibility.Hidden;

            status = "and d.STATUS = 'S'";

        }

        public void viewPesananSelesai(string keyword = "")
        {
            lastclick = 4;
            ViewComponent.textboxCariPesanan.Text = "";

            htrans.initAdapter($"select distinct h.KODE as \"Kode Pesanan\", c.NAMA as \"Nama Customer\", to_char(sum(i.HARGA * d.JUMLAH)) as \"Total\", h.TANGGAL_TRANSAKSI as \"Tanggal Transaksi\", k.NAMA as \"Kurir\", case h.STATUS when 'C' then 'Canceled' when 'W' then 'Waiting Payment' when 'P' then 'Paid' end as \"Status\" from H_TRANS_ITEM h, D_TRANS_ITEM d, CUSTOMER c, ITEM i, KURIR k where i.ID = d.ID_ITEM and h.ID = d.ID_H_TRANS_ITEM and c.ID = h.ID_CUSTOMER and i.ID_SELLER = '{seller["ID"]}' and k.ID = d.ID_KURIR and d.STATUS = 'D' {keyword} group by h.KODE, c.NAMA, h.TANGGAL_TRANSAKSI, k.NAMA, h.STATUS {orderby}");
            toCurrencyHtrans();

            ViewComponent.datagridPesanan.ItemsSource = htrans.Table.DefaultView;
            ViewComponent.canvasDetailPesanan.Visibility = System.Windows.Visibility.Hidden;

            status = "and d.STATUS = 'D'";

        }

        public void viewPesananBatal(string keyword = "")
        {
            lastclick = 5;
            ViewComponent.textboxCariPesanan.Text = "";

            htrans.initAdapter($"select distinct h.KODE as \"Kode Pesanan\", c.NAMA as \"Nama Customer\", to_char(sum(i.HARGA * d.JUMLAH)) as \"Total\", h.TANGGAL_TRANSAKSI as \"Tanggal Transaksi\", k.NAMA as \"Kurir\", case h.STATUS when 'C' then 'Canceled' when 'W' then 'Waiting Payment' when 'P' then 'Paid' end as \"Status\" from H_TRANS_ITEM h, D_TRANS_ITEM d, CUSTOMER c, ITEM i, KURIR k where i.ID = d.ID_ITEM and h.ID = d.ID_H_TRANS_ITEM and c.ID = h.ID_CUSTOMER and i.ID_SELLER = '{seller["ID"]}' and k.ID = d.ID_KURIR and d.STATUS = 'C' {keyword} group by h.KODE, c.NAMA, h.TANGGAL_TRANSAKSI, k.NAMA, h.STATUS {orderby}");
            toCurrencyHtrans();

            ViewComponent.datagridPesanan.ItemsSource = htrans.Table.DefaultView;
            ViewComponent.canvasDetailPesanan.Visibility = System.Windows.Visibility.Hidden;

            status = "and d.STATUS = 'C'";

        }

        public void reloadHtrans()
        {
            selectHtrans(h_trans_selected);
        }
        public int hitungTotal()
        {
            int total = 0;
            for (int i = 0; i < dtrans_helper.Table.Rows.Count; i++)
            {
                DataRow dr = dtrans.Table.Rows[i], drHelper = dtrans_helper.Table.Rows[i];
                string statuses = drHelper[1].ToString();
                if (statuses == "SC" || statuses == "S" || statuses == "D")
                {
                    try
                    {
                        total += Convert.ToInt32(drHelper["Total"].ToString());
                    }catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            ViewComponent.textboxTotalPesanan.Text = Utility.formatMoney(total);
            return total;
        }
        public void selectHtrans(int selected)
        {
            if(selected != -1 && selected < htrans.Table.Rows.Count)
            {
                ViewComponent.checkboxTerimaSemua.IsChecked = false;
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
                dtrans.initAdapter($"select d.JUMLAH as \"Jumlah\", i.NAMA as \"Nama Barang\", to_char(i.HARGA) as \"Harga\", to_char(i.HARGA * d.JUMLAH) as \"Total\", case d.STATUS when 'W' then 'Pesanan Baru' when 'P' then 'Siap Kirim' when 'S' then 'Dalam Pengiriman' when 'D' then 'Pesanan Selesai' when 'C' then 'Pesanan Dibatalkan' end as \"Status\" from ITEM i, H_TRANS_ITEM h, D_TRANS_ITEM d where h.ID = d.ID_H_TRANS_ITEM and i.ID = d.ID_ITEM and h.KODE = '{dr[0].ToString()}' and i.ID_SELLER = '{seller["ID"]}' {status}");
                dtrans_helper = new D_Trans_ItemModel();
                dtrans_helper.initAdapter($"select d.ID , d.STATUS, i.STOK, d.JUMLAH, i.ID, to_char(i.HARGA * d.JUMLAH) as \"Total\" from ITEM i, H_TRANS_ITEM h, D_TRANS_ITEM d where h.ID = d.ID_H_TRANS_ITEM and i.ID = d.ID_ITEM and h.KODE = '{dr[0].ToString()}' and i.ID_SELLER = '{seller["ID"]}' {status}");
                ViewComponent.datagridProdukPesanan.ItemsSource = dtrans.Table.DefaultView;

                Utility.toCurrency(dtrans.Table, 2);
                Utility.toCurrency(dtrans.Table, 3);
                ViewComponent.canvasDetailPesanan.Visibility = System.Windows.Visibility.Visible;
                hitungTotal();
            }
        }

        public void confirm()
        {
            int tidakterima = 0;
            int trans = 0;
            //check stock
            for (int i = 0; i < dtrans_helper.Table.Rows.Count; i++)
            {
                DataRow dr = dtrans.Table.Rows[i], drHelper = dtrans_helper.Table.Rows[i];
                if (drHelper[1].ToString() == "SC")
                {
                    if (Convert.ToInt32(drHelper[2].ToString()) <= Convert.ToInt32(drHelper[3].ToString()))
                    {
                        MessageBox.Show($"Barang {dr[1].ToString()} tidak mencukupi, transaksi gagal");
                        return;
                    }
                }
            }
            foreach (DataRow dr in dtrans_helper.Table.Rows)
            {
                if (dr[1].ToString() == "W") tidakterima++;
                if (dr[1].ToString() == "SC") trans++;
            }
            if (tidakterima != 0)
            {
                DialogResult d = MessageBox.Show($"Ada {tidakterima} pesanan yang belum diterima, Yakin lanjut ?", "Pesanan Blom di terima", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (d == DialogResult.No)
                {
                    return;
                }
            }
            if (trans != 0)
            {
                foreach (DataRow dr in dtrans_helper.Table.Rows)
                {
                    if (dr[1].ToString() == "SC")
                        new DB("D_TRANS_ITEM").update("STATUS", "S").where("ID", dr[0].ToString()).execute();
                }
                reloadHtrans();
            }

        }

        public void toggleDtrans()
        {
            d_trans_selected = ViewComponent.datagridProdukPesanan.SelectedIndex;
            if (d_trans_selected == -1 && d_trans_selected < dtrans.Table.Rows.Count) return;
            DataRow dr = dtrans.Table.Rows[d_trans_selected];
            DataRow dr_trans = htrans.Table.Rows[h_trans_selected];
            if(dr_trans["Status"].ToString() == "Canceled")
            {
                MessageBox.Show("Tidak bisa terima pesanan karena pesanan di cancel");
                return;
            }
            else if(dr_trans["Status"].ToString() == "Waiting Payment")
            {
                MessageBox.Show("Tidak bisa terima pesanan karena pesanan belum dibayar");
                return;
            }
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
            ViewComponent.datagridProdukPesanan.ItemsSource = "";
            ViewComponent.datagridProdukPesanan.ItemsSource = dtrans.Table.DefaultView;
            hitungTotal();

        }
        public void terimasemua()
        {
            bool ischecked = (bool) ViewComponent.checkboxTerimaSemua.IsChecked;
            DataRow dr_trans = htrans.Table.Rows[h_trans_selected];

            if (dr_trans["Status"].ToString() == "Canceled")
            {
                MessageBox.Show("Tidak bisa terima pesanan karena pesanan di cancel");
                return;
            }
            else if (dr_trans["Status"].ToString() == "Waiting Payment")
            {
                MessageBox.Show("Tidak bisa terima pesanan karena pesanan belum dibayar");
                return;
            }
            for (int i = 0; i < dtrans_helper.Table.Rows.Count; i++)
            {
                DataRow drhelper = dtrans_helper.Table.Rows[i], dr = dtrans.Table.Rows[i];
                if (drhelper[1].ToString() == "W" || drhelper[1].ToString() == "SC")
                {
                    if (ischecked) drhelper[1] = "SC";
                    else drhelper[1] = "W";

                    if (ischecked) dr[4] = "Dalam Pengiriman *";
                    else dr[4] = "Pesanan Baru";
                }
            }
            ViewComponent.datagridProdukPesanan.ItemsSource = "";
            ViewComponent.datagridProdukPesanan.ItemsSource = dtrans.Table.DefaultView;
            hitungTotal();
        }
        

        public void checkStatus(DataGridRow dgRow)
        {
            DataRowView item = dgRow.Item as DataRowView;
            if (item != null)
            {
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

        public void showLaporan() {
            new LaporanView().ShowDialog();
        }
    }
}
