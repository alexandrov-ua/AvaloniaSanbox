using System;
using AvaloniaSandbox.Pages.AsyncTest;
using AvaloniaSandbox.Pages.WeatherForecast;
using AvaloniaSandbox.Pages.Welcome;
using AvaloniaSandbox.Services;
using AvaloniaSandbox.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaSandbox;

public static class AppServices
{
    public static void AddCommonServices(this IServiceCollection collection) {
        collection.AddTransient<MainViewModel>();
        collection.AddTransient<AsyncTestViewModel>();
        collection.AddTransient<WeatherForecastViewModel>();
        collection.AddTransient<WelcomeViewModel>();
        collection.AddHttpClient<IWeatherForecastService, WeatherForecastService>(c =>
        {
            c.BaseAddress = new Uri("https://api.open-meteo.com");
        });
    }
}