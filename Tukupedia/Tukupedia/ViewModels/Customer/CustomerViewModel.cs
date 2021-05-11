using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using MaterialDesignThemes.Wpf;
using Tukupedia.Components;

namespace Tukupedia.ViewModels.Customer
{
    public class CustomerViewModel
    {
        public static void loadItems(StackPanel sp)
        {
            ItemCard item = new ItemCard();
            item.setHarga(123);
            item.setNamaBarang("nama");
            item.setRating(2);
            item.setJual("Terjual : 10");
            item.deploy();
            sp.Children.Add(item);
        }
    }
}
