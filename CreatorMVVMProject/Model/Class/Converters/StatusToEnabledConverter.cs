using CreatorMVVMProject.Model.Class.StatusReportService;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

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
            if (value is Status status && dictionary.ContainsKey(status))
            {
                return dictionary[status];
            }
            else
            {
                return false;
            }
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
