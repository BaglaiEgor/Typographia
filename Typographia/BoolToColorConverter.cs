using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Brush = System.Windows.Media.Brush;

namespace Typographia.Pages
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string[] colors = (parameter as string)?.Split(';') ?? new[] { "Green", "Red", "Transparent" };
            Brush trueColor = (Brush)new BrushConverter().ConvertFromString(colors[0]);
            Brush falseColor = (Brush)new BrushConverter().ConvertFromString(colors[1]);
            Brush nullColor = (Brush)new BrushConverter().ConvertFromString(colors[2]);

            if (value is bool isRead)
            {
                return isRead ? trueColor : falseColor;
            }
            return nullColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
