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
            reportView.setParam("ID_SELLER", Session.User["ID"].ToString());
            reportView.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private ParameterValues GetParameterValues(string idSeller) {
            ParameterValues pValues = new ParameterValues();
            Model model = new Model();
            model.initAdapter($"SELECT hti.ID FROM H_TRANS_ITEM hti join D_TRANS_ITEM DTI on hti.ID = DTI.ID_H_TRANS_ITEM join ITEM I on I.ID = DTI.ID_ITEM WHERE I.ID_SELLER='{idSeller}' AND hti.STATUS ='P' group by hti.ID");
            foreach (DataRow row in model.Table.Rows) {
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
