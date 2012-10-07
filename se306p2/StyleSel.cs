using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Media;

namespace se306p2
{
    public class StyleSel: StyleSelector
    {
        public override Style SelectStyle(object item, DependencyObject container)
        {
            //Style st = new Style();
            //st.TargetType = typeof(SurfaceListBoxItem);
            //Setter backGroundSetter = new Setter();
            //backGroundSetter.Property = SurfaceListBoxItem.BackgroundProperty;
            //SurfaceListBox surfacelistbox =
            //    ItemsControl.ItemsControlFromItemContainer(container)
            //      as SurfaceListBox;

            FrameworkElement element = container as FrameworkElement;

            DataItem ob = item as DataItem;

            if (ob.Color == "#0b9246")
            {
                return element.FindResource("SurfaceListBoxItemStyle4") as Style; 
            }
            else if(ob.Color == "#1e9ad5")
            {
                return element.FindResource("SurfaceListBoxItemStyle3") as Style;
            }
            else if (ob.Color == "#f6a220")
            {
            return element.FindResource("SurfaceListBoxItemStyle2") as Style;
            }
            
            return null;
        }
    }
}
