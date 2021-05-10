using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Tukupedia.Helpers.Utils
{
    static class ComponentHelper
    {


        public static void changeZIndexComponent(FrameworkElement component, Visibility vis)
        {
            if (vis == Visibility.Visible)
            {
                Panel.SetZIndex(component, 1);
                component.IsEnabled = true;
            }
            else if (vis == Visibility.Hidden)
            {
                Panel.SetZIndex(component, 0);
                component.IsEnabled = false;
            }
        }

        public static void changeVisibilityComponent(FrameworkElement component, Visibility vis)
        {
            if (vis == Visibility.Visible)
            {
                component.Opacity = 1;
            }
            else if (vis == Visibility.Hidden)
            {
                component.Opacity = 0;
            }
            changeZIndexComponent(component, vis);
        }
    }
}
