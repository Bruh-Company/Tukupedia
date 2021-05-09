using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tukupedia.Helpers.Utils
{
    public static class Session
    {
        public static DataRow User;
        public static bool isLogin = false;
        public static string role = "";

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
    }
}
