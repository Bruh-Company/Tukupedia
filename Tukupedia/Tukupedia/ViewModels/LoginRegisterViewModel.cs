using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tukupedia.Helpers.DatabaseHelpers;

namespace Tukupedia.ViewModels
{
    class LoginRegisterViewModel
    {
        public static bool login(string username, string password)
        {
            
            if (username == "admin" && password == "admin")
            {
                MessageBox.Show("Berhasil Login Admin");
                return true;
            }
            DB db = new DB("customer");
            DataRow customer = db.select()
                .where("email", username)
                .where("password", password)
                .getFirst();
            if (customer == null)
            {
                MessageBox.Show("Gagal Customer");

                return false;
            }
            else
            {
                MessageBox.Show("Berhasil Login Customer"+customer[0].ToString());
                return true;
            }

            db = new DB("seller");
            DataRow seller = db.select()
                .where("email", username)
                .where("password", password)
                .getFirst();
            if (seller == null)
            {
                MessageBox.Show("Gagal Seller");
                return false;
            }
            else
            {
                MessageBox.Show("Berhasil Login Seller" + seller[0].ToString());
                return true;

            }

            return false;
        }
        
    }
}
