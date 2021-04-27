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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tukupedia.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            IDseq id = new IDseq(9959076001234567890, 3138428376859, 1771566314683, 16);

            string a = "";
            for(int i = 0; i < 10; i++)
            {
                a += id.nextId() + "\n";
            }
            MessageBox.Show(a);
        }
    }
}
