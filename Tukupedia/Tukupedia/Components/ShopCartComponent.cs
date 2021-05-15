using System.Windows.Controls;
using MaterialDesignThemes.Wpf;

namespace Tukupedia.Components
{
    public class ShopCartComponent:Card
    {
        private StackPanel spMain;
        private StackPanel spDesc;
        private StackPanel spCart;
        private StackPanel spContent;
        private StackPanel spSubtotal;
        private StackPanel spKurir;
        private TextBlock namaToko;
        private TextBlock isOfficial;
        private TextBlock pilihDurasi;
        private TextBlock subTotal;
        private TextBlock labelsubTotal;
        private ComboBox cbKurir;
        

        public ShopCartComponent()
        {
            spMain = new StackPanel();
            spDesc = new StackPanel();
            spContent = new StackPanel();
            spCart = new StackPanel();
            spSubtotal = new StackPanel();
            spKurir = new StackPanel();
            namaToko = new TextBlock();
            isOfficial = new TextBlock();
            pilihDurasi = new TextBlock();
            subTotal = new TextBlock();
            labelsubTotal = new TextBlock();
            cbKurir = new ComboBox();

            spDesc.Children.Add(namaToko);
            //Check if Official
            spDesc.Children.Add(isOfficial);

            spKurir.Children.Add(pilihDurasi);
            spKurir.Children.Add(cbKurir);
            
            spContent.Orientation = Orientation.Horizontal;
            spContent.Children.Add(spCart);
            spContent.Children.Add(spKurir);

            spSubtotal.Children.Add(labelsubTotal);
            spSubtotal.Children.Add(subTotal);
            
            spMain.Children.Add(spDesc);
            spMain.Children.Add(spContent);
            spMain.Children.Add(spSubtotal);

        }
    }
}