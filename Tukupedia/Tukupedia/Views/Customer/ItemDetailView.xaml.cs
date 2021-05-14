using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for ItemDetailView.xaml
    /// </summary>
    public partial class ItemDetailView : Window
    {
        private int qty;
        private int maxQty;
        private DataRow item;
        private bool reviewLoaded = false;
        private bool discussionLoaded = false;
        public ItemDetailView()
        {
            InitializeComponent();
            qty = 0;
            maxQty = 10;
            
        }

        public void initDetail(string urlImage, DataRow item)
        {
            //Load Image
            ImageItem.Source = new BitmapImage(new Uri(
                AppDomain.CurrentDomain.BaseDirectory + "Resource\\Logo\\TukupediaLogo.png"));
            this.item = item;
            loadDetails();
            
        }

        public void loadDetails()
        {
            // Load Description
            tbDescription.Text = item["DESKRIPSI"].ToString();
            //Load Item Name
            tbNamaItem.Text = item["NAMA"].ToString();
            //Load Rating TODO Ganti setelah sudah ganti DB
            // RatingBar.Value = Convert.ToInt32(item["RATING"]);

        }

        private void TabDescription_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void TabReview_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!reviewLoaded)
            {
                ItemDetailViewModel.loadReviews(spanelReview, Convert.ToInt32(item["ID"]),cdContent.ActualWidth);
                reviewLoaded = true;
            }
        }

        private void TabDiscussion_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!discussionLoaded)
            {
                //Load Discussion
                ItemDetailViewModel.loadDiscussions(spanelDiscussion,Convert.ToInt32(item["ID"]));
                discussionLoaded = true;
            }
        }
        
        private void BtnMin_OnClick(object sender, RoutedEventArgs e)
        {
            qty--;
            qty = Math.Max(0, qty);
            tbQuantity.Text = qty.ToString();
        }

        private void BtnPlus_OnClick(object sender, RoutedEventArgs e)
        {
            qty++;
            qty = Math.Min(maxQty, qty);
            tbQuantity.Text = qty.ToString();
        }

        //TODO Add Cart
        private void BtnAddCart_OnClick(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
