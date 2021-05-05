using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Tukupedia
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static bool gagal = false;
        public static string datasource, username, password;
        public static OracleConnection connection = new OracleConnection()
        {
            ConnectionString = $"Data Source=ORCL;User Id=projectpcs;Password=pcs;"
        };
        public static void initConnectionString(string datasource, string username, string password)
        {
            App.datasource = datasource;
            App.username = username;
            App.password = password;
            connection.ConnectionString = $"Data Source={datasource};User Id={username};Password={password};";
        }
        public static void openConnection()
        {
            try
            {
                if (App.connection.State == ConnectionState.Open)
                {
                    App.connection.Close();
                    App.connection.Open();
                }
                else
                {
                    App.connection.Open();
                }
                //MessageBox.Show("Koneksi Sukses");
                gagal = false;
            }
            catch (Exception ex)
            {
                gagal = true;
                MessageBox.Show("Gagal Login Oracle");
            }
        }

        public static void closeConnection()
        {
            try
            {
                if (App.connection.State == ConnectionState.Open)
                {
                    App.connection.Close();
                }
                //MessageBox.Show("Koneksi Tertutup");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
