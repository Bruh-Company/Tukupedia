using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using Tukupedia.Helpers.DatabaseHelpers;
using Tukupedia.Helpers.Utils;

namespace Tukupedia.Helpers.Classes
{
    public class Promo
    {
        private string ID;
        private string KODE;
        private int POTONGAN;
        private int POTONGAN_MAX;
        private int HARGA_MIN;
        private string JENIS_POTONGAN;
        private int ID_JENIS_PROMO;
        private DateTime TANGGAL_AWAL;
        private DateTime TANGGAL_AKHIR;
        private string STATUS;
        private DataRow JenisPromo;
        private Dictionary<string, bool> jenis;

        public Promo(string id, string kode, int potongan, int potonganMax, int hargaMin, string jenisPotongan, int idJenisPromo, DateTime tanggalAwal, DateTime tanggalAkhir, string status)
        {
            ID = id;
            KODE = kode;
            POTONGAN = potongan;
            POTONGAN_MAX = potonganMax;
            HARGA_MIN = hargaMin;
            JENIS_POTONGAN = jenisPotongan;
            ID_JENIS_PROMO = idJenisPromo;
            TANGGAL_AWAL = tanggalAwal;
            TANGGAL_AKHIR = tanggalAkhir;
            STATUS = status;
            // JenisPromo = new DB("JENIS_PROMO").@select().@where("ID", ID_JENIS_PROMO.ToString()).getFirst();
            jenis = new Dictionary<string, bool>();
            jenis.Add("category", false);
            jenis.Add("kurir", false);
            jenis.Add("seller", false);
            jenis.Add("metode_pembayaran", false);
        }

        //TODO PROMO :)
        public bool checkPromo(string id_category, string id_kurir, string id_seller, string id_payment,string tglAwal, string tglAkhir)
        {
            bool valid = true;
            //Check tanggal
            DateTime awal = DateTime.Parse(tglAwal);
            DateTime akhir = DateTime.Parse(tglAkhir);
            valid &= Utility.betweenDate(awal, akhir);
            
            //Check Dari jenis Promo
            if (JenisPromo != null)
            {
                MessageBox.Show(JenisPromo["ID_CATEGORY"].ToString());
                //Harus 1 kategory aja
                if (JenisPromo["ID_CATEGORY"] == null)
                {
                    valid &= id_category == JenisPromo["ID_CATEGORY"].ToString();
                    jenis["category"] = true;

                }
                //Kalau tokonya banyak, harus boleh 1kurir saja
                if (JenisPromo["ID_KURIR"] == null)
                {
                    valid &= id_kurir == JenisPromo["ID_KURIR"].ToString();
                    jenis["kurir"] = true;
                }
                //Kalau ada id seller brarti harus boleh 1 toko aja
                if (JenisPromo["ID_SELLER"] == null)
                {
                    valid &= id_seller == JenisPromo["ID_SELLER"].ToString();
                    jenis["seller"] = true;
                }
                //Metode pembayaran juga hanya boleh 1
                if (JenisPromo["ID_METODE_PEMBAYARAN"] == null)
                {
                    valid &= id_payment == JenisPromo["ID_METODE_PEMBAYARAN"].ToString();
                    jenis["metode_pembayaran"] = true;
                }
            }
            
            return valid;
        }

        //harusnya 
        public int getDiscount()
        {
            int discount = 0;
            // bool valid = checkPromo();
            
            return discount;
        }

        public string getDescription()
        {
            string str = "";
            str += $"Promo berlaku selama periode {Utility.formatDate(TANGGAL_AWAL)} - {Utility.formatDate(TANGGAL_AKHIR)}\n";
            str += $"Promo berlaku saat minimal belanja sebesar {HARGA_MIN} \n";
            str += $"Promo berupa diskon sebesar {POTONGAN} dengan potongan maksimal sebesar {POTONGAN_MAX}\n";
            if (jenis["category"])
            {
                DataRow row = new DB("CATEGORY").@select().@where("ID", JenisPromo["ID_CATEGORY"].ToString())
                    .getFirst();
                str += $"Promo ini hanya berlaku pada barang dengan category {row["NAMA"]}\n";
            }
            if (jenis["kurir"])
            {
                DataRow row = new DB("KURIR").@select().@where("ID", JenisPromo["ID_KURIR"].ToString()).getFirst();
                str += $"Promo berlaku apabila menggunakan {row["NAMA"]} sebagai kurir\n";

            }
            if (jenis["seller"])
            {
                DataRow row = new DB("SELLER").@select().@where("ID", JenisPromo["ID_SELLER"].ToString()).getFirst();
                str += $"Promo berlaku apabila barang berasal dari toko {row["NAMA_TOKO"]} \n";

            }
            if (jenis["metode_pembayaran"])
            {
                DataRow row = new DB("METODE_PEMBAYARAN").@select().@where("ID", JenisPromo["ID_METODE_PEMBAYARAN"].ToString())
                    .getFirst();
                str += $"Promo berlaku apabila menggunakan metode pembayaran {row["NAMA"]} \n";
            }
            return str;
        }
        public override string ToString()
        {
            return this.KODE;
        }
    }
}