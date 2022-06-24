using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Type = CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml.Type;

namespace CreatorMVVMProject.Model.Class.Converters
{
    [ValueConversion(typeof(Type), typeof(string))]
    public class StepTypeToIconConverter : IValueConverter
    {
        private readonly Dictionary<Type, string> dictionary = new()
        {
            { Type.Upload, "UploadIcon" },
            { Type.Download, "DownloadIcon" },
            { Type.Executable, "ExecuteIcon" }
        };

        public object? Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Type type && dictionary.ContainsKey(type))
            {
                return Application.Current.FindResource(dictionary[type]) as BitmapImage;
            }
            else 
            {
                return "";
            }
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
