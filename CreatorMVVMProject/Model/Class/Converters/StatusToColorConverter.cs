﻿using CreatorMVVMProject.Model.Class.StatusReportService;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace CreatorMVVMProject.Model.Class.Converters
{
    [ValueConversion(typeof(Status), typeof(string))]
    public class StatusToColorConverter : IValueConverter
    {
        private const string Red = "#D35D6E";
        private const string Green = "#116530";
        private const string LightGreen = "#5AA469";
        private const string Gray = "#A9A9A9";
        private readonly Dictionary<Status, string> dictionary = new()
        {
            { Status.NotStarted, Gray },
            { Status.InProgress, LightGreen },
            { Status.Success, Green },
            { Status.Disabled, Red },
            { Status.Failed, Red },
            { Status.Obsolete, Red }
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Status status && dictionary.ContainsKey(status) ? dictionary[status] : (object)Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
