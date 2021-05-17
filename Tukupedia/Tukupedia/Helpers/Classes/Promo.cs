using System;
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
            JenisPromo = new DB("JENIS_PROMO").@select().@where("ID", ID_JENIS_PROMO.ToString()).getFirst();
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
                }
                //Kalau tokonya banyak, harus boleh 1kurir saja
                if (JenisPromo["ID_KURIR"] == null)
                {
                    valid &= id_kurir == JenisPromo["ID_KURIR"].ToString();
                }
                //Kalau ada id seller brarti harus boleh 1 toko aja
                if (JenisPromo["ID_SELLER"] == null)
                {
                    valid &= id_seller == JenisPromo["ID_SELLER"].ToString();
                }
                //Metode pembayaran juga hanya boleh 1
                if (JenisPromo["ID_METODE_PEMBAYARAN"] == null)
                {
                    valid &= id_payment == JenisPromo["ID_METODE_PEMBAYARAN"].ToString();
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
    }
}