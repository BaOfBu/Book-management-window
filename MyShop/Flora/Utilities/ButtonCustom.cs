using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Telerik.Windows.Controls;

namespace Flora.Utilities
{
    public class ButtonCustom : RadRadioButton
    {
        static ButtonCustom()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ButtonCustom), new FrameworkPropertyMetadata(typeof(ButtonCustom)));
        }
    }
}
