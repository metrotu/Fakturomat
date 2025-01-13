using System.Globalization;
using System.Windows.Data;

namespace ArchOp
{
    public class VatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "Exempt";
            if (value is double vat)
                return vat == 0 ? "0%" : $"{vat * 100}%";
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                if (str.Equals("Exempt", StringComparison.OrdinalIgnoreCase))
                    return null;
                if (double.TryParse(str.TrimEnd('%'), out var percentage))
                    return percentage / 100;
            }
            return null;
        }
    }
}