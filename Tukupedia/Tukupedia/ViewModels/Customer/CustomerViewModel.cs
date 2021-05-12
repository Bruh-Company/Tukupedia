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

namespace Tukupedia.ViewModels.Customer
{
    public class CustomerViewModel
    {
        private static ItemModel items = new ItemModel();
        private static CategoryModel categories = new CategoryModel();
        private static DataRow[] filteredItems;
        private static bool isFiltered=false;
        
        public static void loadItems(WrapPanel wp)
        {
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
                    card.setItem(item);
                    card.deploy();
                    wp.Children.Add(card);
                }
            }
        }

        public static void searchItems()
        {
            filteredItems = items.Table.Select(items.where);
        }

        public static void loadCategory(ComboBox cb)
        {
            foreach (DataRow row in categories.Table.Select("STATUS='1'"))
            {
                CheckBox checkBox = new CheckBox();
                checkBox.Content = row["NAMA"].ToString();
                checkBox.Tag = row["ID"];
                checkBox.IsChecked = true;
                // TODO Beri handler untuk kasih label berapa category yang dipilih
                cb.Items.Add(checkBox);
            }
        }
    }
}
