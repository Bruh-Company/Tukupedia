using System.Windows;
using MaterialDesignThemes.Wpf;

namespace Tukupedia.Components
{
    public class CustomCard : Card
    {
        public CustomCard(FrameworkElement elem)
        {
            this.AddChild(elem);
        }
    }
}