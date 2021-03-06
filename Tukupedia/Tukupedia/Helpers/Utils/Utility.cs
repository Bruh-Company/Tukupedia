using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Tukupedia.Helpers.Utils
{
    public static class Utility
    {
        private const string ErrCapt = "Utility fail";

        public static int checkMax(DataTable table, string column, int startIdx, int length, string like)
        {
            int maxNumber = 0;
            foreach (DataRow dr in table.Select(like))
            {
                string urutan;
                if (length == 0) urutan = dr[column].ToString().Substring(startIdx);
                else urutan = dr[column].ToString().Substring(startIdx, length);
                urutan = urutan.TrimStart(new char[] { '0' });
                int id = Convert.ToInt32(urutan);
                maxNumber = Math.Max(maxNumber, id);
            }
            return maxNumber;
        }
        public static string translate(int val, int len)
        {
            return string.Format("{0:D" + len + "}", val);
        }

        public static string formatMoney(int money)
        {
            System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("id-ID");
            return money.ToString("C2", cultureinfo);
        }
        public static string formatNumber(int val)
        {
            return val.ToString("N");
        }

        public static string kodeGenerator(string nama)
        {
            string[] kotak = nama.Split(' ');
            string kode = "";
            if (kotak.Length == 1)
            {
                kode += kotak[0].Substring(0, 2);
            }
            else
            {
                kode += kotak[0].Substring(0, 1) + kotak[1].Substring(0, 1);
            }
            return kode;
        }

        public static string Date2Str(DateTime date, string patt = "dd-MM-yyyy")
        {
            try
            {
                return date.ToString(patt);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error while convert date\n" +
                                $"Err:\n" +
                                $"{e}",
                                ErrCapt, MessageBoxButton.OK, MessageBoxImage.Error);
                return "";
            }
        }

        public static DateTime Str2Date(string date, string patt = "dd-MM-yyyy")
        {
            try
            {
                CultureInfo provider = CultureInfo.InvariantCulture;
                return DateTime.ParseExact(date, patt, provider);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error while convert date\n" +
                                $"Err:\n" +
                                $"{e}",
                                ErrCapt, MessageBoxButton.OK, MessageBoxImage.Error);
                return DateTime.Now;
            }
        }

        // ini buat input cuma angka + spasi (kalau ada yang mau hilangi spasinya silakan)
        public static void NumberValidationTextBox(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public static void TextBoxNumeric(TextBox tb)
        {
            string temp = tb.Text;
            tb.Text = temp.All(char.IsDigit) ? temp : temp.Remove(temp.Length - 1);
        }

        public static string defaultPicture = "Resource\\Logo\\TukupediaLogo.png";
        public static string tokopediaLogo = "Resource\\Logo\\TukupediaLogo.png";

        public static DateTime dateParse(string date)
        {
            return DateTime.Parse(date);
        }
        public static string formatDate(DateTime date)
        {
            return $"{date:dd-MM-yyyy}";
        }
        public static string formatDate(string date)
        {
            return dateParse(date).ToString("dd-MM-yyyy");
        }
        public static string StringFromRichTextBox(RichTextBox rtb)
        {
            TextRange textRange = new TextRange(
                // TextPointer to the start of content in the RichTextBox.
                rtb.Document.ContentStart,
                // TextPointer to the end of content in the RichTextBox.
                rtb.Document.ContentEnd
            );

            // The Text property on a TextRange object returns a string
            // representing the plain text content of the TextRange.
            return textRange.Text;
        }

        public static bool betweenDate(DateTime fromDate, DateTime toDate)
        {
            DateTime curent = DateTime.Now.Date;
            if (fromDate.CompareTo(toDate) >= 1)
            {
                MessageBox.Show("From Date shouldn't be grater than To Date", "DateRange",MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            }
            int cd_fd = curent.CompareTo(fromDate);
            int cd_td = curent.CompareTo(toDate);

            if (cd_fd == 0 || cd_td == 0)
            {
                return true;
            }

            if (cd_fd >= 1 && cd_td <= -1)
            {
                return true;
            }
            return false;
        }
        public static void setRichTextBoxString(RichTextBox rtb,string str)
        {
            rtb.Document.Blocks.Clear();
            rtb.Document.Blocks.Add(new Paragraph(new Run(str)));
        }

        public static void TraverseVisualTree(Visual myMainWindow)
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(myMainWindow);
            for (int i = 0; i < childrenCount; i++)
            {
                var visualChild = (Visual)VisualTreeHelper.GetChild(myMainWindow, i);
                if (visualChild is TextBox)
                {
                    TextBox tb = (TextBox)visualChild;
                    tb.Clear();
                }
                if (visualChild is PasswordBox)
                {
                    PasswordBox pb = (PasswordBox)visualChild;
                    pb.Clear();

                }
                TraverseVisualTree(visualChild);
            }
        }
        public static void toCurrency(DataTable dt, int row)
        {
            foreach (DataRow dr in dt.Rows)
            {
                dr[row] = Utility.formatMoney(Convert.ToInt32(dr[row].ToString()));
            }
        }
        public static string toNumber(string duek)
        {
            duek = duek.Substring(2);
            duek = duek.Replace(".", "");
            duek = duek.Replace(",00", "");

            return duek;
        }

    }
}
