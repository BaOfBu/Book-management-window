using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

namespace Flora.Utilities
{
    public class DefaultOrCustomLabelsConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var customLabels = values[0] as ObservableCollection<string>;
            var defaultLabels = values[1] as ObservableCollection<string>;

            if (customLabels != null && customLabels.Count > 0)
            {
                return customLabels;
            }
            else
            {
                return defaultLabels;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
