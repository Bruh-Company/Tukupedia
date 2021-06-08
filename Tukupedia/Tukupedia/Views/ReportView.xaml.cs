using CrystalDecisions.CrystalReports.Engine;
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

namespace Tukupedia.Views {
    /// <summary>
    /// Interaction logic for ReportView.xaml
    /// </summary>
    /// CARA PAKAI
    /// init reportView dulu -> setParam -> show reportView
    public partial class ReportView : Window {
        private ReportClass report;
        public ReportView(ReportClass report) {
            InitializeComponent();
            this.report = report;
            this.report.SetDatabaseLogon(App.username, App.password, App.datasource, "");
            reportViewer.Owner = Window.GetWindow(this);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            reportViewer.ViewerCore.ReportSource = report;
        }

        public void setParam<T>(string paramName, T paramValue) {
            report.SetParameterValue(paramName, paramValue);
        }
    }
}
