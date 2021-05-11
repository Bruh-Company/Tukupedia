using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Tukupedia.ViewModels.Customer;

namespace Tukupedia.Views.Customer
{
    /// <summary>
    /// Interaction logic for CustomerView.xaml
    /// </summary>
    public partial class CustomerView : Window
    {
        public CustomerView()
        {
            InitializeComponent();
            
        }

        private void flipper_MouseUp(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void CustomerView_OnLoaded(object sender, RoutedEventArgs e)
        {
            CustomerViewModel.loadItems(PanelItems);
            Label l = new Label();
            l.Content = "test";
            PanelItems.Children.Add(l);
        }
    }
}
