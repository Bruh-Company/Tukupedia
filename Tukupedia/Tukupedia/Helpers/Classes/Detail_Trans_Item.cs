using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tukupedia.Helpers.Classes
{
    public class Detail_Trans_Item
    {
        public string id { get; set; }
        public string namaItem { get; set; }
        public string jumlah { get; set; }
        public string namaKurir { get; set; }
        public string status { get; set; }

        public Detail_Trans_Item(string id, string namaItem, string jumlah, string namaKurir, string status)
        {
            this.id = id;
            this.namaItem = namaItem;
            this.jumlah = jumlah;
            this.namaKurir = namaKurir;
            if(status == "W")
            {
                this.status = "WAITING FOR CONFIRMATION";
            }
            else if(status == "S")
            {
                this.status = "SHIPPING";
            }
            else if (status == "D")
            {
                this.status = "FINISHED";
            }
            else if (status == "C")
            {
                this.status = "CANCELED";
            }
        }
    }
}
