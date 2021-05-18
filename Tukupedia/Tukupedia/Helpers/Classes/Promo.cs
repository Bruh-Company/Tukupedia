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
        public string ID { get; set; }
        public string KODE { get; set; }
        public int POTONGAN { get; set; }
        public int POTONGAN_MAX { get; set; }
        public int HARGA_MIN { get; set; }
        public string JENIS_POTONGAN { get; set; }
        public int ID_JENIS_PROMO { get; set; }
        public DateTime TANGGAL_AWAL { get; set; }
        public DateTime TANGGAL_AKHIR { get; set; }
        public string STATUS { get; set; }
        public DataRow JenisPromo { get; set; }
        public Dictionary<string, bool> jenis { get; set; }

        

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
            jenis = new Dictionary<string, bool>();
            jenis.Add("category", false);
            jenis.Add("kurir", false);
            jenis.Add("seller", false);
            jenis.Add("metode_pembayaran", false);
        }

        // cat dri item
        // kurir dri scc
        // seller dri item
        // payment dr scc
        public bool checkPromo(string id_category, string id_kurir, string id_seller, string id_payment)
        {
            bool valid = true;
            //Check tanggal
            valid &= Utility.betweenDate(TANGGAL_AWAL, TANGGAL_AKHIR);

            //Check Dari jenis Promo
            if (JenisPromo != null)
            {
                //Harus 1 kategory aja foreach semua item yang checked apakah category sama
                // MessageBox.Show((JenisPromo["ID_CATEGORY"].ToString() == "").ToString());

                if (JenisPromo["ID_CATEGORY"].ToString() !=  "")
                {
                    valid &= id_category == JenisPromo["ID_CATEGORY"].ToString();
                    // MessageBox.Show("CATEGORY : " + (id_category == JenisPromo["ID_CATEGORY"].ToString()).ToString());
                    jenis["category"] = true;

                }
                //Kalau tokonya banyak, harus boleh 1kurir saja foreach shopcart, cek kalau selectednya adalah kurir yang sama ato tidk
                if (JenisPromo["ID_KURIR"].ToString()  !=  "")
                {
                    valid &= id_kurir == JenisPromo["ID_KURIR"].ToString();
                    // MessageBox.Show("ID_KURIR : " + (id_kurir == JenisPromo["ID_KURIR"].ToString()).ToString());
                    jenis["kurir"] = true;
                }
                //Kalau ada id seller brarti harus boleh 1 toko aja check apakah shopcartcomponent hnya 1 yaitu id seller yang bner ato tidak
                if (JenisPromo["ID_SELLER"].ToString() !=  "")
                {
                    valid &= id_seller == JenisPromo["ID_SELLER"].ToString();
                    // MessageBox.Show("ID_SELLER : " + (id_seller == JenisPromo["ID_SELLER"].ToString()).ToString());
                    jenis["seller"] = true;
                }
                //Metode pembayaran juga hanya boleh 1 Cek Metode pembyaran yang dipakai
                if (JenisPromo["ID_METODE_PEMBAYARAN"].ToString()  !=  "")
                {
                    valid &= id_payment == JenisPromo["ID_METODE_PEMBAYARAN"].ToString();
                    // MessageBox.Show("ID_METODE_PEMBAYARAN : " + (id_payment == JenisPromo["ID_METODE_PEMBAYARAN"].ToString()).ToString());
                    jenis["metode_pembayaran"] = true;
                }
            }
            
            return valid;
        }

        //harusnya 
        public int getDiscount()
        {
            int discount = 0;
            
            
            return discount;
        }

        public string getDescription()
        {
            if (JenisPromo["ID_CATEGORY"].ToString() !=  "") jenis["category"] = true;
            if (JenisPromo["ID_KURIR"].ToString()  !=  "")jenis["kurir"] = true;
            if (JenisPromo["ID_SELLER"].ToString() !=  "")jenis["seller"] = true;
            if (JenisPromo["ID_METODE_PEMBAYARAN"].ToString()  !=  "")jenis["metode_pembayaran"] = true;
            string str = "";
            str += $"Promo berlaku selama periode {Utility.formatDate(TANGGAL_AWAL)} - {Utility.formatDate(TANGGAL_AKHIR)}\n";
            str += $"Promo berlaku saat minimal belanja sebesar {Utility.formatMoney(HARGA_MIN)} \n";
            if(JENIS_POTONGAN=="P")
                str += $"Promo berupa diskon sebesar {POTONGAN} dengan potongan maksimal sebesar {Utility.formatMoney(POTONGAN_MAX)}\n";
            else if(JENIS_POTONGAN=="F") 
                str += $"Promo berupa diskon sebesar {Utility.formatMoney(POTONGAN)} \n"; 
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