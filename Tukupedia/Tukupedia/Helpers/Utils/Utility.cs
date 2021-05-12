using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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
            return money.ToString("C2", CultureInfo.CurrentCulture);
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

        public static DateTime Str2Date(string date, string patt = "dd-MM-yyyy", string cultureInfo = "en-US")
        {
            try
            {
                CultureInfo provider = new CultureInfo(cultureInfo);
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

        private static void NumberValidationTextBox(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

    }
}
