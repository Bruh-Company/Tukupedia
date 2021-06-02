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
        private const string ErrCapt = "Oracle Connection Error";

        public static bool gagal = false;
        public static string datasource="ORCL", username="projectpcs", password="pcs";

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

        public static void testConnection(out bool status)
        {
            bool st;

            openConnection(out st);
            if (!st)
            {
                status = false;
                return;
            }
            closeConnection(out st);
            if (!st)
            {
                status = false;
                return;
            }

            status = true;
            return;
        }

        public static void openConnection(out bool status)
        {
            bool st;

            try
            {
                closeConnection(out st);
                if (st) connection.Open();

                status = true;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error opening connection\n" +
                                $"Err:\n" +
                                $"{e}",
                                ErrCapt, MessageBoxButton.OK, MessageBoxImage.Error);
                status = false;
            }

        }

        public static void closeConnection(out bool status)
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();

                status = true;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error opening connection\n" +
                                $"Err:\n" +
                                $"{e}",
                                ErrCapt, MessageBoxButton.OK, MessageBoxImage.Error);
                status = false;
            }
        }
    }
}
