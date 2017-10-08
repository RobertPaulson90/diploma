using System;
using System.Globalization;
using System.Windows.Data;

namespace Diploma.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    internal sealed class InvertBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var booleanValue = (bool)value;
            return !booleanValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var booleanValue = (bool)value;
            return !booleanValue;
        }
    }
}
