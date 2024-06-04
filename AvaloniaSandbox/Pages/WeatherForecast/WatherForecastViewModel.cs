using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using AvaloniaSandbox.Services;
using AvaloniaSandbox.ViewModels;
using ReactiveUI;

namespace AvaloniaSandbox.Pages.WeatherForecast;

public class WeatherForecastViewModel : ViewModelBase
{
    private readonly IWeatherForecastService _weatherForecastService;

    public List<CityModel> Cities { get; set; } = new List<CityModel>()
    {
        new CityModel("Kyiv", 52.52, 13.41),
        new CityModel("London", 51.50, 0.12),
    };

    private CityModel _selectedCity;

    public CityModel SelectedCity
    {
        get => _selectedCity;
        set => this.RaiseAndSetIfChanged(ref _selectedCity, value);
    }

    private readonly ObservableAsPropertyHelper<WeatherForecastModel> _currentForecast;

    public WeatherForecastModel CurrentForecast
    {
        get => _currentForecast.Value;
    }

    private readonly ObservableAsPropertyHelper<bool> _isLoading;
    public bool IsLoading
    {
        get => _isLoading.Value;
    }

    public WeatherForecastViewModel(IWeatherForecastService weatherForecastService)
    {
        _weatherForecastService = weatherForecastService;
        SelectedCity = Cities.First();
        _currentForecast = this.WhenAnyValue(t => t.SelectedCity)
            .SelectMany(async t => (await _weatherForecastService.GetForecast(t.Latitude, t.Longitude)).ToWeatherForecastModel(t.Name))
            .ToProperty(this, t => t.CurrentForecast);

        _isLoading = this.WhenAnyValue(t => t.SelectedCity, t => t.CurrentForecast)
            .Select(t => t.Item1?.Name != t.Item2?.CityName)
            .ToProperty(this, t => t.IsLoading);
    }
}

public record CityModel(string Name, double Latitude, double Longitude);

public record WeatherForecastModel(
    string CityName,
    double Temperature,
    string TemperatureUnit,
    double WindSpeed,
    string WindSpeedUnit);

public static class ConverterExtensions
{
    public static WeatherForecastModel ToWeatherForecastModel(this Services.WeatherForecast weatherForecast,
        string cityName)
    {
        return new WeatherForecastModel(cityName, 
            weatherForecast.Current.Temperature,
            weatherForecast.CurrentUnits.Temperature, 
            weatherForecast.Current.WindSpeed,
            weatherForecast.CurrentUnits.WindSpeed);
    }
}