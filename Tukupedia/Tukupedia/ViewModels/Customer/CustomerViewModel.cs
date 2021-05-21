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
        
        public static void loadItems(WrapPanel wp)
        {
            wp.Children.Clear();
            if (isFiltered)
            {
                foreach(DataRow item in filteredItems)
                {
                    ItemCard card = new ItemCard();
                    int jml = new DB("D_TRANS_ITEM").select()
                        .where("ID_ITEM", item["ID"].ToString()).count();
                    card.setHarga(Convert.ToInt32(item["HARGA"]));
                    card.setNamaBarang(item["NAMA"].ToString());
                    card.setRating(5);
                    card.setJual($"Terjual : {jml}");
                    //if (item["IMAGE"].ToString() != "") card.setImage(item["IMAGE"].ToString());
                    card.deploy();
                    wp.Children.Add(card);
                }
            }
            else
            {
                foreach(DataRow item in new ItemModel().Table.Rows)
                {
                    ItemCard card = new ItemCard();
                    int jml = new DB("D_TRANS_ITEM").select()
                        .where("ID_ITEM", item["ID"].ToString()).count();
                    card.setHarga(Convert.ToInt32(item["HARGA"]));
                    card.setNamaBarang(item["NAMA"].ToString());
                    card.setRating(5);
                    card.setJual($"Terjual : {jml}");
                    //if (item["IMAGE"].ToString() != "") card.setImage(item["IMAGE"].ToString());
                    card.setItem(item);
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
                "i.HARGA as HARGA, " +
                "i.NAMA as NAMA " +
                "FROM ITEM i, SELLER s, CATEGORY c " +
                "WHERE i.ID_SELLER = s.ID " +
                "and i.ID_CATEGORY = c.ID ";

            string where = "";
            if (keyword != "") {
                where += $"and (i.NAMA like '%{keyword}%' " +
                    $"or s.NAMA_TOKO like '%{keyword}%' ";
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

            MessageBox.Show(cmd);
            items = new ItemModel();
            items.initAdapter(cmd);
            filteredItems = items.Table.Select();
            if (keyword != "") {
                
            }
            loadItems(ViewComponent.PanelItems);
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
