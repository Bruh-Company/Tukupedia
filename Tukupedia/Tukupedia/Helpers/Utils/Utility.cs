using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tukupedia.Helpers.Utils
{
    public static class Utility
    {
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
    }
}
