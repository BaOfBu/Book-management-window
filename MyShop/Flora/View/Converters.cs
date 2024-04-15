using Flora;
using System.Globalization;
using System.Windows.Data;
using System;

public class PlantToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Plant plant)
        {
            return $"{plant.Name} - {plant.Price}";
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}