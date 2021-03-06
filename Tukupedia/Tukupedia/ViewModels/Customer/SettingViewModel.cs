using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tukupedia.Helpers.DatabaseHelpers;
using Tukupedia.Helpers.Utils;
using Tukupedia.Models;
using Tukupedia.Views.Customer;

namespace Tukupedia.ViewModels.Customer
{
    public class SettingViewModel
    {
        static CustomerModel cm = new CustomerModel();
        static CustomerView ViewComponent;

        private static string imagePath;
        public static void init(CustomerView cv)
        {
            ViewComponent = cv;

        }
        public static void reload()
        {
            cm.initAdapter($"select ID, NAMA, EMAIL, NO_TELP, ALAMAT, TANGGAL_LAHIR, IMAGE, PASSWORD from CUSTOMER where id = {Session.User["ID"].ToString()}");
            DataRow dr = cm.Table.Rows[0];
            ViewComponent.textboxNamaCustomer.Text = dr["NAMA"].ToString();
            ViewComponent.textboxEmailInfo.Text = dr["EMAIL"].ToString();
            ViewComponent.textboxAlamatInfo.Text = dr["ALAMAT"].ToString();
            ViewComponent.textboxNoTelpInfo.Text = dr["NO_TELP"].ToString();
            ViewComponent.dpickerlahir.SelectedDate = DateTime.Parse(dr["TANGGAL_LAHIR"].ToString());
            if (dr["IMAGE"].ToString() == "") ImageHelper.loadImageCheems(ViewComponent.imageInfo);
            else ImageHelper.loadImage(ViewComponent.imageInfo, dr["IMAGE"].ToString(),ImageHelper.target.customer);
            editing();
        }
        public static void update()
        {
            Admin.CustomerViewModel acvm = new Admin.CustomerViewModel();
            if (Session.User["EMAIL"].ToString() == ViewComponent.textboxEmailInfo.Text) ;
            else if (!acvm.checkEmail(ViewComponent.textboxEmailInfo.Text))
            {
                return;
            }
            DataRow dr = cm.Table.Rows[0];
            dr["NAMA"] = ViewComponent.textboxNamaCustomer.Text;
            dr["EMAIL"] = ViewComponent.textboxEmailInfo.Text;
            dr["ALAMAT"] = ViewComponent.textboxAlamatInfo.Text;
            dr["NO_TELP"] = ViewComponent.textboxNoTelpInfo.Text;
            dr["IMAGE"] = ImageHelper.saveImage(imagePath, Session.User["KODE"].ToString(), ImageHelper.target.customer);
            DateTime lahir = ViewComponent.dpickerlahir.SelectedDate.Value;
            new DB("customer").update("TANGGAL_LAHIR", lahir).where("ID",dr[0].ToString()).execute();
            cm.update();
            reload();

        }
        public static void changeImage()
        {
            imagePath = ImageHelper.openFileDialog(ViewComponent.imageInfo);
        }
        public static void editing(bool isEditing = false)
        {
            ViewComponent.textboxNamaCustomer.IsReadOnly = !isEditing;
            ViewComponent.textboxEmailInfo.IsReadOnly = !isEditing;
            ViewComponent.textboxAlamatInfo.IsReadOnly = !isEditing;
            ViewComponent.textboxNoTelpInfo.IsReadOnly = !isEditing;
            ViewComponent.btnCancel.Visibility = isEditing ? Visibility.Visible : Visibility.Hidden;
            ViewComponent.btnSave.Visibility = isEditing ? Visibility.Visible : Visibility.Hidden;
            ViewComponent.btnUbahInfoCustomer.Visibility = isEditing ? Visibility.Hidden : Visibility.Visible;
            ViewComponent.btnChangeImage.Visibility = isEditing ? Visibility.Visible : Visibility.Hidden;
            ViewComponent.btnChangePassword.Visibility = isEditing ? Visibility.Visible : Visibility.Hidden;
            ViewComponent.dpickerlahir.IsEnabled = isEditing;
            imagePath = "";
        }
    }
}
