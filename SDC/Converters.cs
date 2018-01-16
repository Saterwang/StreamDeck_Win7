using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SDC
{
    public class MarginConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue)
            {
                return null;
            }
            ButtonPosMode PosMode = (ButtonPosMode)values[0];
            int CustomTopMargin = (int)values[1];

            Thickness ret = new Thickness(0);
            switch(PosMode)
            {
                case ButtonPosMode.Center:
                    break;
                case ButtonPosMode.Upper:
                    ret.Top = 10;
                    break;
                case ButtonPosMode.Lower:
                    ret.Top = 52;
                    break;
                case ButtonPosMode.Custom:
                    ret.Top = CustomTopMargin;
                    break;

                default:
                    break;
            }

            return ret;
        }
        public object[] ConvertBack(object values, Type[] targetType, object parameter,System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    public class AlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ButtonPosMode PosMode = (ButtonPosMode)value;
            if(PosMode == ButtonPosMode.Center)
            {
                return VerticalAlignment.Center;
            }

            return VerticalAlignment.Top;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class StringToIntConverter:IValueConverter
    {
        public int EmptyStringValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;
            else if (value is string)
                return value;
            else if (value is int && (int)value == EmptyStringValue)
                return string.Empty;
            else
                return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is String)
            {
                String s = (String)value;
                int i = 0;

                if (int.TryParse(s, out i))
                    return i;
                else
                    return EmptyStringValue;
            }
            return value;
        }

    }
}
