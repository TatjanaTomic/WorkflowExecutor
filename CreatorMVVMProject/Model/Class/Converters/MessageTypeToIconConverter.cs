using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace CreatorMVVMProject.Model.Class.Converters
{
    [ValueConversion(typeof(bool), typeof(string))]
    public class MessageTypeToIconConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isErrorMessage && !isErrorMessage)
            {
                return Application.Current.FindResource("InformationIcon") as BitmapImage;
            }
            return Application.Current.FindResource("ErrorIcon") as BitmapImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is string icon && icon.Equals("ErrorIcon");
        }
    }
}
