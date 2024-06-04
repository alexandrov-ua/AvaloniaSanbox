using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace AvaloniaSandbox.Pages.WeatherForecast;


public class TemperatureToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var val = (double)value;
        return val switch
        {
            >25.0=> new SolidColorBrush(Colors.Red),
            <5 => new SolidColorBrush(Colors.Blue),
            _=> new SolidColorBrush(Colors.Green)
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}