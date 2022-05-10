using ExecutionEngine.Step;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace CreatorMVVMProject.Model.Class.Converters
{
    [ValueConversion(typeof(Status), typeof(string))]
    public class StatusToColorConverter : IValueConverter
    {
        private readonly Dictionary<Status, string> dictionary = new()
        {
            { Status.InProgress, "LightGreen" },
            { Status.Disabled, "Red" },
            { Status.Success, "Green" },
            { Status.Failed, "Red" },
            { Status.NotStarted, "Red" }
        };
        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            
            if (value is Status status && dictionary.ContainsKey(status))
            {
                return dictionary[status];
            }
            else
                return "Red";

        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
