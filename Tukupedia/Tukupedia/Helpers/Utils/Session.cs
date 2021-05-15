using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tukupedia.Models;

namespace Tukupedia.Helpers.Utils
{
    public static class Session
    {
        public static DataRow User;
        public static bool isLogin = false;
        public static string role = "";
        public static Dictionary<int, DataTable> list_carts = new Dictionary<int,DataTable>();

        public static void Logout()
        {
            User = null;
            isLogin = false;
        }
        public static void Login(DataRow user, string Role)
        {
            User = user;
            isLogin = true;
            role = Role;
        }

        public static void setCart(int id_cust, DataTable cart)
        {
            if (list_carts.ContainsKey(id_cust))
            {
                list_carts[id_cust] = cart;
            }
            else
            {
                list_carts.Add(id_cust,cart);
            }
        }

        public static DataTable getCart(int id_cust)
        {
            if (list_carts.ContainsKey(id_cust))
            {
                return list_carts[id_cust];
            }
            else
            {
                return new D_Trans_ItemModel().Table.Clone();
            }
        }
    }
}
