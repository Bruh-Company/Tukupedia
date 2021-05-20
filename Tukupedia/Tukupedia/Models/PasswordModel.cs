using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Tukupedia.Models
{
    class PasswordModel : Model
    {
        public PasswordModel(string tabel, string id)
        {
            this.initAdapter($"SELECT ID, PASSWORD from {tabel} where id = {id}");
        }
        public bool changePassword(string passwordLama, string passwordBaru, string confirmPassword)
        {
            DataRow dr = Table.Rows[0];
            if(dr["PASSWORD"].ToString() == passwordLama)
            {
                if (passwordBaru == confirmPassword)
                {
                    dr["PASSWORD"] = passwordBaru;
                    update();
                    MessageBox.Show("Password Berhasil diganti");
                    return true;
                }
                else
                {
                    MessageBox.Show("Password dan Confirm Password Tidak sama");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Password Lama Salah");
                return false;
            }
        }
    }
}
