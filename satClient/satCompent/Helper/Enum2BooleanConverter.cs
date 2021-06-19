using System;
using System.Windows.Data;

namespace TSFCS.DMDS.Client.Converter
{
    public class Enum2BooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;
            string checkvalue = value.ToString();
            string targetvalue = parameter.ToString();
            return checkvalue.Equals(targetvalue, StringComparison.InvariantCultureIgnoreCase);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || parameter == null)
                return null;
            bool usevalue = (bool)value;
            if (usevalue)
                return parameter.ToString();
            return null;
        }
    }
}
