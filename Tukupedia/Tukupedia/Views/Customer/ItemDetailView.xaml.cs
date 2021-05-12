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

namespace Tukupedia.Views.Customer
{
    /// <summary>
    /// Interaction logic for ItemDetailView.xaml
    /// </summary>
    public partial class ItemDetailView : Window
    {
        public ItemDetailView()
        {
            InitializeComponent();
        }

        public void initDetail(string urlImage)
        {
            //Load Image
            ImageItem.Source = new BitmapImage(new Uri(
                AppDomain.CurrentDomain.BaseDirectory + "Resource\\Logo\\TukupediaLogo.png"));
            
            
        }

        private void TabDescription_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void TabReview_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void TabDiscussion_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            
        }
    }
}
