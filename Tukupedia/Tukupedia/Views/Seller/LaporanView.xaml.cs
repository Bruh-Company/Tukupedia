using CrystalDecisions.Shared;
using System;
using System.Data;
using System.Windows;
using Tukupedia.Helpers.Utils;
using Tukupedia.Models;
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
            ParameterValues pValues = GetParameterValues(Session.User["ID"].ToString());
            SellerPenjualanReport report = new SellerPenjualanReport();
            ReportView reportView = new ReportView(report);
            reportView.setParam("minDate", minDate);
            reportView.setParam("maxDate", maxDate);
            reportView.setParam("idHtrans", pValues);
            reportView.setParam("imagePath", ImageHelper.getDebugPath() + "\\Resource\\Items\\");
            reportView.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private ParameterValues GetParameterValues(string idSeller) {
            ParameterValues pValues = new ParameterValues();
            H_Trans_ItemModel model = new H_Trans_ItemModel();
            model.initAdapter($"select h.ID as ID from H_TRANS_ITEM h, D_TRANS_ITEM d, ITEM i where h.ID = d.ID_H_TRANS_ITEM and d.ID_ITEM = i.ID and i.ID_SELLER = {idSeller}");
            model.init();
            foreach (DataRow row in model.get()) {
                pValues.Add(getParamVal(row["ID"].ToString()));
            }
            return pValues;
        }

        public ParameterDiscreteValue getParamVal(Object val) {
            ParameterDiscreteValue param = new ParameterDiscreteValue();
            param.Value = val;
            return param;
        }
    }
}
