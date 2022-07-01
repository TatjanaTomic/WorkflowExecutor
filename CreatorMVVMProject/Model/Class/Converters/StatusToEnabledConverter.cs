using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using CreatorMVVMProject.Model.Class.StatusReportService;

namespace CreatorMVVMProject.Model.Class.Converters
{
    [ValueConversion(typeof(Status), typeof(bool))]
    public class StatusToEnabledConverter : IValueConverter
    {
        private readonly Dictionary<Status, bool> dictionary = new()
        {
            { Status.Disabled, false },
            { Status.NotStarted, true },
            { Status.InProgress, false },
            { Status.Success, true },
            { Status.Failed, true },
            { Status.Obsolete, true }
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Status status && dictionary.ContainsKey(status) ? dictionary[status] : (object)false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
