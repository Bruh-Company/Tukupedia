using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tukupedia.Helpers.DatabaseHelpers;
using Tukupedia.Models;

namespace Tukupedia.ViewModels.Admin
{
    class OfficialStoreViewModel
    {
        Trans_OSModel Htom;
        Trans_OSModel Htomhelper;
        Trans_OSModel Dtom;
        int selected = -1;
        public OfficialStoreViewModel()
        {
            Htom = new Trans_OSModel();
            Htomhelper = new Trans_OSModel();
            reloadHtom();
        }

        void reloadHtom()
        {
            Htom.initAdapter("select s.NAMA_TOKO as \"Nama Toko\", s.NAMA_SELLER as \"Pemilik\", case h.STATUS when 'R' then 'Request' when 'D' then 'Declined' when 'A' then 'Accepted' end as \"Status\", h.TANGGAL_TRANSAKSI as \"Tanggal Request\" from SELLER s, TRANS_OS h where h.ID_SELLER = s.ID order by h.TANGGAL_TRANSAKSI desc, h.STATUS desc");
            Htomhelper.initAdapter("select h.ID, h.STATUS, h.ID_SELLER from SELLER s, TRANS_OS h where h.ID_SELLER = s.ID order by h.TANGGAL_TRANSAKSI desc, h.STATUS desc");
        }

        public DataTable getHtom()
        {
            //MessageBox.Show(cm.statement);
            return Htom.Table;
        }
        public DataRow getDetailToko(int pos)
        {
            try
            {
                selected = pos;
                // 0 = id transos
                // 1 = status R / D / A
                // 2 = id seller
                DataRow dr = Htomhelper.Table.Rows[pos];
                Dtom = new Trans_OSModel();

                Dtom.initAdapter($"select s.NAMA_TOKO as \"toko\", s.EMAIL as \"email\", s.ALAMAT as \"alamat\", s.NO_TELP as \"notelp\", s.NAMA_SELLER as \"nama\", s.CREATED_AT as \"createdat\" from SELLER s where s.ID = {dr[2].ToString()}");

                return Dtom.Table.Rows[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataRow getHtomHelper()
        {
            return Htomhelper.Table.Rows[selected];
        }
        public bool getOS_Status()
        {
            DataRow dr = Htomhelper.Table.Rows[selected];
            DataRow result = new DB("SELLER").select("IS_OFFICIAL").where("ID", dr[2].ToString()).getFirst();
            if (result[0].ToString() == "1") return true;
            return false;

        }
        public void ChangeStatus(bool stats)
        {
            DB osmodel = new DB(), seller = new DB();
            DataRow dr = Htomhelper.Table.Rows[selected];
            // 0 = id transos
            // 1 = status R / D / A
            // 2 = id seller
            if (stats)
            {
                osmodel.statement = $"update TRANS_OS set STATUS = 'A' where ID = {dr[0].ToString()}";
                seller.statement = $"update SELLER set IS_OFFICIAL = '1' where ID = {dr[2].ToString()}";
            }
            else
            {
                osmodel.statement = $"update TRANS_OS set STATUS = 'D' where ID = {dr[0].ToString()}";
                seller.statement = $"update SELLER set IS_OFFICIAL = '0' where ID = {dr[2].ToString()}";
            }
            osmodel.execute();
            seller.execute();

        }
    }
}
