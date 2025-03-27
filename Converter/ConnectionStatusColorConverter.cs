// ConnectionStatusColorConverter.cs
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfApp3.Converter
{
    public class ConnectionStatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool connected
                ? (connected ? Brushes.Green : Brushes.Red)
                : Brushes.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}