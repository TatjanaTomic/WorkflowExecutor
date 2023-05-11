using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using CreatorMVVMProject.Model.Class.StatusReportService;

namespace CreatorMVVMProject.Model.Class.Converters;

[ValueConversion(typeof(Status), typeof(string))]
public class StatusToIconConverter : IValueConverter
{
    private readonly Dictionary<Status, string> dictionary = new()
    {
        { Status.Ready, "GrayIcon" },
        { Status.Running, "LightGreenIcon" },
        { Status.Success, "GreenIcon" },
        { Status.Blocked, "OrangeIcon" },
        { Status.Failed, "DarkRedIcon" },
        { Status.Obsolete, "RedIcon" }
    };

    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is Status status && dictionary.ContainsKey(status) ? Application.Current.FindResource(dictionary[status]) as BitmapImage : (object)"";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is string icon && dictionary.ContainsValue(icon) ? dictionary.FirstOrDefault(x => x.Value == icon).Key : DependencyProperty.UnsetValue;
    }
}
