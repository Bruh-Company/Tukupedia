using System;
using System.Windows;
using Tukupedia.Helpers.Utils;
using Tukupedia.Report;

namespace Tukupedia.Views.Seller {
    /// <summary>
    /// Interaction logic for LaporanView.xaml
    /// </summary>
    public partial class LaporanView : Window {
        public LaporanView() {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e) {
            datePickerMulai.SelectedDate = DateTime.Now.AddMonths(-1);
            datePickerAkhir.SelectedDate = DateTime.Now;
        }

        private void btnShow_Click(object sender, RoutedEventArgs e) {
            DateTime minDate = datePickerMulai.SelectedDate.Value;
            DateTime maxDate = datePickerAkhir.SelectedDate.Value;
            int id = Convert.ToInt32(Session.User["ID"].ToString());
            SellerPenjualanReport report = new SellerPenjualanReport();
            ReportView reportView = new ReportView(report);
            reportView.setParam("minDate", minDate);
            reportView.setParam("maxDate", maxDate);
            reportView.setParam("idSeller", id);
            reportView.Show();
            this.Close();
        }

    }
}
