using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using CreatorMVVMProject.Model.Class.StatusReportService;

namespace CreatorMVVMProject.Model.Class.Converters;

[ValueConversion(typeof(Status), typeof(string))]
public class StatusToForecolorConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Status status && Status.Success == status)
        {
            return Application.Current.FindResource("TextLightColorBrush") as SolidColorBrush;
        }

        return Application.Current.FindResource("TextDarkColorBrush") as SolidColorBrush;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
