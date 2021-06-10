using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using MaterialDesignThemes.Wpf;
using Tukupedia.Components;
using Tukupedia.Helpers.DatabaseHelpers;
using Tukupedia.Models;
using Tukupedia.Views.Customer;

namespace Tukupedia.ViewModels.Customer
{
    public class CustomerViewModel
    {
        private static ItemModel items;
        private static CategoryModel categories;
        private static DataRow[] filteredItems;
        private static CustomerView ViewComponent;
        private static bool isFiltered=false;
        

        public static void initCustomerViewModel(CustomerView view) {
            ViewComponent = view;
        }
        
        public static void loadItems()
        {
            WrapPanel wp = ViewComponent.PanelItems;
            wp.Children.Clear();
            if (isFiltered)
            {
                foreach(DataRow item in filteredItems)
                {
                    ItemCard card = new ItemCard();
                    DataRow jmlItem = new DB("D_TRANS_ITEM")
                        .select("SUM(JUMLAH) as JML")
                        .where("ID_ITEM", item["ID"].ToString())
                        .getFirst();
                    int jml = jmlItem["JML"].ToString()!=""?Convert.ToInt32(jmlItem["JML"]):0;
                    card.setHarga(Convert.ToInt32(item["HARGA"]));
                    card.setNamaBarang(item["NAMA"].ToString());
                    card.setItem(item);
                    if (item["RATING"].ToString() == "")
                    {
                        card.setRating(0);
                    }
                    else
                    {
                        card.setRating(Convert.ToInt32(item["RATING"]));
                    }
                    card.setJual($"Terjual : {jml}");
                    if (item["IMAGE"].ToString() != "") card.setImage(item["IMAGE"].ToString());
                    card.deploy();
                    wp.Children.Add(card);
                }
            }
            else
            {
                foreach(DataRow item in new ItemModel().Table.Select("STATUS = '1'"))
                {
                    ItemCard card = new ItemCard();
                    DataRow jmlItem = new DB("D_TRANS_ITEM")
                        .select("SUM(JUMLAH) as JML")
                        .where("ID_ITEM", item["ID"].ToString())
                        .getFirst();
                    int jml = jmlItem["JML"].ToString() != "" ? Convert.ToInt32(jmlItem["JML"]) : 0;
                    card.setItem(item);
                    card.setHarga(Convert.ToInt32(item["HARGA"]));
                    card.setNamaBarang(item["NAMA"].ToString());
                    if (item["RATING"].ToString() == "")
                    {
                        card.setRating(0);
                    }
                    else
                    {
                        card.setRating(Convert.ToInt32(item["RATING"]));
                    }
                    card.setJual($"Terjual : {jml}");
                    if (item["IMAGE"].ToString() != "") card.setImage(item["IMAGE"].ToString());
                    card.deploy();
                    wp.Children.Add(card);
                }
            }
        }

        public static void searchItems(string keyword, int minPrice, int maxPrice, List<int> categoryIDs)
        {
            string cmd =
                "SELECT " +
                "i.ID as ID, " +
                "i.KODE as KODE, " +
                "i.ID_CATEGORY as ID_CATEGORY, " +
                "i.HARGA as HARGA, " +
                "i.STATUS as STATUS, " +
                "i.DESKRIPSI as DESKRIPSI, " +
                "i.ID_SELLER as ID_SELLER, " +
                "i.BERAT as BERAT, " +
                "i.STOK as STOK, " +
                "i.RATING as RATING, " +
                "i.IMAGE as IMAGE, " +
                "i.CREATED_AT as CREATED_AT, " +
                "i.NAMA as NAMA " +
                "FROM ITEM i, SELLER s, CATEGORY c " +
                "WHERE i.ID_SELLER = s.ID " +
                "and i.ID_CATEGORY = c.ID ";

            string where = "";
            if (keyword != "") {
                where += $"and (i.NAMA like '%{keyword.ToUpper()}%' " +
                    $"or s.NAMA_TOKO like '%{keyword.ToUpper()}%' ";
            }
            if (minPrice < maxPrice) {
                if (minPrice > 0) {
                    if (where == "") where += $"and (i.HARGA > {minPrice} ";
                    else where += $" or i.HARGA > {minPrice} ";
                    
                }
                if (maxPrice > 0) {
                    if (where == "") where += $"and (i.HARGA > {maxPrice} ";
                    else where += $" or i.HARGA > {maxPrice} ";
                }
                if (categoryIDs != null && categoryIDs.Count > 0) {
                    foreach (int id in categoryIDs) {
                        if (where == "") where += $"and (c.ID = {id} ";
                        else where += $" or c.ID = {id} ";
                    }
                }
            }
            where += where == "" ? "" : ")";
            cmd += where;

            items = new ItemModel();
            items.initAdapter(cmd);
            filteredItems = items.Table.Select();
            if (keyword != "") {
                
            }
            loadItems();
            isFiltered = false;
        }
    
        public static void loadCategory(StackPanel sp)
        {
            categories = new CategoryModel();
            categories.addWhere("STATUS", "1");
            categories.init();

            foreach (DataRow row in categories.Table.Rows) {
                CheckBox checkBox = new CheckBox();
                checkBox.Name = "cb" + row["ID"].ToString();
                checkBox.Content = row["NAMA"].ToString();
                sp.Children.Add(checkBox);
            }
        }

        public static void filterItems() {
            string keyword = ViewComponent.tbSearch.Text;
            int minPrice = (int)ViewComponent.SliderMin.Value;
            int maxPrice = (int)ViewComponent.SliderMax.Value;

            List<int> listCategoryID = new List<int>();
            foreach (DataRow row in categories.Table.Rows) {
                int id = Convert.ToInt32(row["ID"].ToString()) - 1;
                if (ViewComponent.spCategory.Children[id] != null) {
                    CheckBox cb = (CheckBox)ViewComponent.spCategory.Children[id];
                    if (cb.IsChecked == true) listCategoryID.Add(id);
                }
            }
            
            if (keyword == "" && minPrice == 0 && maxPrice == 0 && listCategoryID == null && listCategoryID.Count <= 0) 
                isFiltered = false;
            else isFiltered = true;
            searchItems(keyword, minPrice, maxPrice, listCategoryID);
        }
    }
}
