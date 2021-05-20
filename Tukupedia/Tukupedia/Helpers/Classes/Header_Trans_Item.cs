using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tukupedia.Helpers.Classes
{
    public class Header_Trans_Item
    {
        public string ID { get; set; }
        public string KODE { get; set; }
        public string TANGGAL_TRANSAKSI { get; set; }
        public string STATUS { get; set; }

        public Header_Trans_Item(string iD, string kODE, string tANGGAL_TRANSAKSI, string sTATUS)
        {
            ID = iD;
            KODE = kODE;
            TANGGAL_TRANSAKSI = tANGGAL_TRANSAKSI;
            STATUS = sTATUS;
            if (sTATUS == "C")
            {
                STATUS = "CANCELED";
            }
            else if (sTATUS == "W")
            {
                STATUS = "WAITING FOR PAYMENT";
            }
            else if (sTATUS == "P")
            {
                STATUS = "PAID";
            }
        }
    }

}
