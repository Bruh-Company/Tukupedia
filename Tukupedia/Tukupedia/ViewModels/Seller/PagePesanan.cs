using System.Data;
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

        private D_Trans_ItemModel ditem;
        private D_Trans_ItemModel forid;
        private D_Trans_ItemModel forselectdata;
        int selected = -1;

        public PagePesanan(SellerView viewComponent, DataRow seller)
        {
            ViewComponent = viewComponent;
            this.seller = seller;
        }

        public void initPagePesanan()
        {
            seller = Session.User;

        }
        public void viewSemuaTransaksi()
        {
            ditem.initAdapter($"select d.JUMLAH as \"Jumlah\", i.NAMA as \"Nama Barang\", i.HARGA as \"Harga\", d.JUMLAH * i.HARGA as \"Total\" from D_TRANS_ITEM d, ITEM i, SELLER s, H_TRANS_ITEM h where d.ID_ITEM = i.ID and s.ID = i.ID_SELLER and h.ID = d.ID_H_TRANS and i.ID_SELLER = {seller["ID"]} order by h.TANGGAL_TRANSAKSI desc");
            forid.initAdapter($"select d.ID from D_TRANS_ITEM d, ITEM i, SELLER s, H_TRANS_ITEM h where d.ID_ITEM = i.ID and s.ID = i.ID_SELLER and h.ID = d.ID_H_TRANS and i.ID_SELLER = {seller["ID"]} order by h.TANGGAL_TRANSAKSI desc");
            ViewComponent.datagridPesanan.ItemsSource = ditem.Table.DefaultView;
        }
        public void viewPesananBaru()
        {
            ditem.initAdapter($"select d.JUMLAH as \"Jumlah\", i.NAMA as \"Nama Barang\", i.HARGA as \"Harga\", d.JUMLAH * i.HARGA as \"Total\" from D_TRANS_ITEM d, ITEM i, SELLER s, H_TRANS_ITEM h where d.ID_ITEM = i.ID and s.ID = i.ID_SELLER and h.ID = d.ID_H_TRANS and d.STATUS = 'W' and i.ID_SELLER = {seller["ID"]} order by h.TANGGAL_TRANSAKSI desc");
            forid.initAdapter($"select d.ID from D_TRANS_ITEM d, ITEM i, SELLER s, H_TRANS_ITEM h where d.ID_ITEM = i.ID and s.ID = i.ID_SELLER and h.ID = d.ID_H_TRANS and d.STATUS = 'W' and i.ID_SELLER = {seller["ID"]} order by h.TANGGAL_TRANSAKSI desc");
            ViewComponent.datagridPesanan.ItemsSource = ditem.Table.DefaultView;

        }
        public void viewSiapKirim()
        {
            ditem.initAdapter($"select d.JUMLAH as \"Jumlah\", i.NAMA as \"Nama Barang\", i.HARGA as \"Harga\", d.JUMLAH * i.HARGA as \"Total\" from D_TRANS_ITEM d, ITEM i, SELLER s, H_TRANS_ITEM h where d.ID_ITEM = i.ID and s.ID = i.ID_SELLER and h.ID = d.ID_H_TRANS and d.STATUS = 'P' and i.ID_SELLER = {seller["ID"]} order by h.TANGGAL_TRANSAKSI desc");
            forid.initAdapter($"select d.ID from D_TRANS_ITEM d, ITEM i, SELLER s, H_TRANS_ITEM h where d.ID_ITEM = i.ID and s.ID = i.ID_SELLER and h.ID = d.ID_H_TRANS and d.STATUS = 'P and i.ID_SELLER = {seller["ID"]}' order by h.TANGGAL_TRANSAKSI desc");
            ViewComponent.datagridPesanan.ItemsSource = ditem.Table.DefaultView;

        }

        public void viewDalamPengiriman()
        {
            ditem.initAdapter($"select d.JUMLAH as \"Jumlah\", i.NAMA as \"Nama Barang\", i.HARGA as \"Harga\", d.JUMLAH * i.HARGA as \"Total\" from D_TRANS_ITEM d, ITEM i, SELLER s, H_TRANS_ITEM h where d.ID_ITEM = i.ID and s.ID = i.ID_SELLER and h.ID = d.ID_H_TRANS and d.STATUS = 'S' and i.ID_SELLER = {seller["ID"]} order by h.TANGGAL_TRANSAKSI desc");
            forid.initAdapter($"select d.ID from D_TRANS_ITEM d, ITEM i, SELLER s, H_TRANS_ITEM h where d.ID_ITEM = i.ID and s.ID = i.ID_SELLER and h.ID = d.ID_H_TRANS and d.STATUS = 'S' and i.ID_SELLER = {seller["ID"]} order by h.TANGGAL_TRANSAKSI desc");
            ViewComponent.datagridPesanan.ItemsSource = ditem.Table.DefaultView;

        }

        public void viewPesananSelesai()
        {
            ditem.initAdapter($"select d.JUMLAH as \"Jumlah\", i.NAMA as \"Nama Barang\", i.HARGA as \"Harga\", d.JUMLAH * i.HARGA as \"Total\" from D_TRANS_ITEM d, ITEM i, SELLER s, H_TRANS_ITEM h where d.ID_ITEM = i.ID and s.ID = i.ID_SELLER and h.ID = d.ID_H_TRANS and d.STATUS = 'D' and i.ID_SELLER = {seller["ID"]} order by h.TANGGAL_TRANSAKSI desc");
            forid.initAdapter($"select d.ID from D_TRANS_ITEM d, ITEM i, SELLER s, H_TRANS_ITEM h where d.ID_ITEM = i.ID and s.ID = i.ID_SELLER and h.ID = d.ID_H_TRANS and d.STATUS = 'D' and i.ID_SELLER = {seller["ID"]} order by h.TANGGAL_TRANSAKSI desc");
            ViewComponent.datagridPesanan.ItemsSource = ditem.Table.DefaultView;

        }

        public void viewPesananBatal()
        {
            ditem.initAdapter($"select d.JUMLAH as \"Jumlah\", i.NAMA as \"Nama Barang\", i.HARGA as \"Harga\", d.JUMLAH * i.HARGA as \"Total\" from D_TRANS_ITEM d, ITEM i, SELLER s, H_TRANS_ITEM h where d.ID_ITEM = i.ID and s.ID = i.ID_SELLER and h.ID = d.ID_H_TRANS and d.STATUS = 'C' and i.ID_SELLER = {seller["ID"]} order by h.TANGGAL_TRANSAKSI desc");
            forid.initAdapter($"select d.ID from D_TRANS_ITEM d, ITEM i, SELLER s, H_TRANS_ITEM h where d.ID_ITEM = i.ID and s.ID = i.ID_SELLER and h.ID = d.ID_H_TRANS and d.STATUS = 'C' and i.ID_SELLER = {seller["ID"]} order by h.TANGGAL_TRANSAKSI desc");
            ViewComponent.datagridPesanan.ItemsSource = ditem.Table.DefaultView;

        }


        public void selectData(int selected)
        {
            this.selected = selected;
            forselectdata = new D_Trans_ItemModel();
            DataRow dr = ditem.Table.Rows[selected];


        }
        public void updateStatus(string status)
        {
            string id = forid.Table.Rows[0][0].ToString();
            //new DB("D_TRANS_ITEM")
        }
    }
}
