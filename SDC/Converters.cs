using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

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

    public class FontFamilyToStrConverter : IValueConverter
    {
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (null == value)
            {
                return String.Empty;
            }

            if(value is System.Windows.Media.FontFamily)
            {
                return (string)(value as System.Windows.Media.FontFamily).ToString();
            }

            return String.Empty;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (null == value)
            {
                return new System.Windows.Media.FontFamily("Arial");
            }

            if (value is String)
            {
                return new System.Windows.Media.FontFamily(value as string);
            }

            return null;
        }
    }
    public class ColorToSolidColorBrushValueConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (null == value)
            {
                return null;
            }
            // For a more sophisticated converter, check also the targetType and react accordingly..
            if (value is Color)
            {
                Color c = (Color)value;
                //Color color = Color.FromArgb(c.A, c.R, c.G, c.B);
                return new SolidColorBrush(c);
            }
            // You can support here more source types if you wish
            // For the example I throw an exception

            Type type = value.GetType();
            throw new InvalidOperationException("Unsupported type [" + type.Name + "]");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // If necessary, here you can convert back. Check if which brush it is (if its one),
            // get its Color-value and return it.

            throw new NotImplementedException();
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
