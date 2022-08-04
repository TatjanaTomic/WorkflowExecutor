using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using CreatorMVVMProject.Model.Class.StatusReportService;

namespace CreatorMVVMProject.Model.Class.Converters
{
    [ValueConversion(typeof(Status), typeof(string))]
    public class StatusToColorConverter : IValueConverter
    {
        private const string Red = "#EE4E56";
        private const string LightRed = "#D35D6E";
        private const string Green = "#116530";
        private const string LightGreen = "#5AA469";
        private const string Gray = "#A9A9A9";
        private const string Orange = "#FFAE42";
        private readonly Dictionary<Status, string> dictionary = new()
        {
            { Status.NotStarted, Gray },
            { Status.InProgress, LightGreen },
            { Status.Success, Green },
            { Status.Disabled, Orange },
            { Status.Failed, Red },
            { Status.Obsolete, LightRed }
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Status status && dictionary.ContainsKey(status) ? dictionary[status] : (object)Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is string color && dictionary.ContainsValue(color) ? dictionary.FirstOrDefault(x => x.Value == color).Key : DependencyProperty.UnsetValue;
        }
    }
}
