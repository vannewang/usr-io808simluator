// 文件路径：DiIndexConverter.cs
using System;
using System.Globalization;
using System.Windows.Data;



namespace WpfApp3.Converter
{
    public class DiIndexConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // values[0]: 当前DI状态值 (bool)
            // values[1]: AlternationIndex (int)
            if (values.Length == 2 && values[0] is bool && values[1] is int index)
            {
                return $"DI{index + 1}";
            }
            return "Unknown";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}