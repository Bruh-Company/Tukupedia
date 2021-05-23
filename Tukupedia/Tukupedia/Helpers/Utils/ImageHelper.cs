using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace Tukupedia.Helpers.Utils {
    public static class ImageHelper {

        // IMAGE PROCESS
        public enum target { item, seller, customer };

        public static string getDebugPath() {
            return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6);
        }

        public static string getItemImagePath(string filename) {
            return new Uri(getDebugPath() + "\\Resource\\Items\\" + filename).LocalPath;
        }

        public static string getSellerImagePath(string filename) {
            return new Uri(getDebugPath() + "\\Resource\\Sellers\\" + filename).LocalPath;
        }
        public static string getCustomerImagePath(string filename) {
            return new Uri(getDebugPath() + "\\Resource\\Customers\\" + filename).LocalPath;
        }

        public static string openFileDialog(Image elem) {
            // return image path
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                if (openFileDialog.FileName == "") return null;
                string path = new Uri(openFileDialog.FileName).LocalPath;
                using (var stream = new FileStream(path, FileMode.Open)) {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();
                    elem.Source = bitmapImage;
                    return path;
                }
            }
            return null;
        }


        public static string saveImage(string originPath, string saveAs, target t, bool debug = false) {
            // return image path
            // cara pakai
            // string path = ImageHelper.saveImage(originPath, row["IMAGE"].ToString(), ImageHelper.target.item)
            if (originPath == "" || originPath == null) {
                Console.WriteLine("originPath is null");
                return null;
            }
            Uri oldUri = new Uri(originPath);
            if (!oldUri.IsFile) return null;

            string oldPath = new Uri(originPath).LocalPath;
            string oldName = Path.GetFileName(oldPath);
            string newPath = "", movePath = "";
            string newName = saveAs + ".png";

            if (t == target.item) {
                newPath = getItemImagePath(oldName);
                movePath = getItemImagePath(newName);
            }
            if (t == target.seller) {
                newPath = getSellerImagePath(oldName);
                movePath = getSellerImagePath(newName);
            }
            if (t == target.customer) {
                newPath = getCustomerImagePath(oldName);
                movePath = getCustomerImagePath(newName);
            }

            if (newPath == "" || movePath == "") return null;

            if (debug) Console.WriteLine("\noldPath = " + oldPath + "\n" + "newPath = " + newPath + "\n" + "movePath = " + movePath + "\n");

            try {
                if (oldPath != newPath) {
                    File.Copy(oldPath, newPath);
                    if (File.Exists(movePath)) File.Delete(movePath);
                    if (oldName != newName) File.Move(newPath, movePath);
                }
                return movePath;
            }
            catch (IOException copyError) {
                MessageBox.Show(copyError.Message);
            }
            return null;
        }

        public static void loadImage(Image elem, string path) {
            // CARA PAKAI
            // loadImage(<component image>, path)
            if (path == "") {
                MessageBox.Show("ERR! Can't load image :)", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            path = new Uri(path).LocalPath;
            using (var stream = new FileStream(path, FileMode.Open)) {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                elem.Source = bitmapImage;
            }
        }

        public static void loadImageCheems(Image elem) {
            string path = new Uri(getDebugPath() + "\\Resource\\cheems.png").LocalPath;
            using (var stream = new FileStream(path, FileMode.Open)) {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                elem.Source = bitmapImage;
            }
        }

        public static void loadImageSwole(Image elem) {
            string path = new Uri(getDebugPath() + "\\Resource\\swole.png").LocalPath;
            using (var stream = new FileStream(path, FileMode.Open)) {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                elem.Source = bitmapImage;
            }
        }
    }
}
