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
using Tukupedia.Helpers.Utils;
using Tukupedia.Models;
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
            CustomerViewModel.loadCategory(cbCategory);
            debugMode();
            labelWelcome.Content = "Welcome "+ Session.User["NAMA"].ToString();

        }

        void debugMode()
        {
            Session.Login(new CustomerModel().Table.Rows[0],"Customer");
        }
        
        private void SliderMax_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tbMaxPrice.Text = Utility.formatNumber(Convert.ToInt32(SliderMax.Value));
        }

        private void SliderMin_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tbMinPrice.Text = Utility.formatNumber(Convert.ToInt32(SliderMin.Value));
        }

        private void BtnLogout_OnClick(object sender, RoutedEventArgs e)
        {
            Session.Logout();
            this.Close();
        }
    }
}
