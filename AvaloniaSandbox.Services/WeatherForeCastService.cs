using System.Text.Json;
using System.Text.Json.Serialization;

namespace AvaloniaSandbox.Services;

public interface IWeatherForecastService
{
    Task<WeatherForecast> GetForecast(double latitude, double longitude);
}

public class WeatherForecastDesignService : IWeatherForecastService
{
    public Task<WeatherForecast> GetForecast(double latitude, double longitude)
    {
        var str =
            "{\n\"latitude\": 52.52,\n\"longitude\": 13.419998,\n\"generationtime_ms\": 0.0400543212890625,\n\"utc_offset_seconds\": 0,\n\"timezone\": \"GMT\",\n\"timezone_abbreviation\": \"GMT\",\n\"elevation\": 38,\n\"current_units\": {\n\"time\": \"iso8601\",\n\"interval\": \"seconds\",\n\"temperature_2m\": \"\u00b0C\",\n\"wind_speed_10m\": \"km/h\"\n},\n\"current\": {\n\"time\": \"2024-06-04T19:45\",\n\"interval\": 900,\n\"temperature_2m\": 18,\n\"wind_speed_10m\": 5.1\n}\n}";
        return Task.FromResult(JsonSerializer.Deserialize<WeatherForecast>(str, WeatherForecastContext.Default.WeatherForecast))!;
    }
}

public class WeatherForecastService : IWeatherForecastService
{
    private readonly HttpClient _httpClient;

    public WeatherForecastService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<WeatherForecast> GetForecast(double latitude, double longitude)
    {
        await Task.Delay(1000);//TODO: delete it, just for tests
        
        var response = await _httpClient.GetAsync($"/v1/forecast?latitude={latitude:F}&longitude={longitude:F}&current=temperature_2m,wind_speed_10m&hourly=temperature_2m,relative_humidity_2m,wind_speed_10m");
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<WeatherForecast>(responseString, WeatherForecastContext.Default.WeatherForecast)!;
    }
}

public class WeatherForecast
{
    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }

    [JsonPropertyName("generationtime_ms")]
    public double GenerationtimeMs { get; set; }

    [JsonPropertyName("utc_offset_seconds")]
    public int UtcOffsetSeconds { get; set; }

    [JsonPropertyName("timezone")]
    public string Timezone { get; set; }

    [JsonPropertyName("timezone_abbreviation")]
    public string TimezoneAbbreviation { get; set; }

    [JsonPropertyName("elevation")]
    public double Elevation { get; set; }
    [JsonPropertyName("current")]
    public CurrentForecast Current { get; set; }
    [JsonPropertyName("current_units")]
    public ForecastUnits CurrentUnits { get; set; }
}

public class CurrentForecast
{
    [JsonPropertyName("time")]
    public DateTime Time { get; set; }
    [JsonPropertyName("temperature_2m")]
    public double Temperature { get; set; }
    [JsonPropertyName("wind_speed_10m")]
    public double WindSpeed { get; set; } 
}

public class ForecastUnits
{
    [JsonPropertyName("time")]
    public string Time { get; set; }
    [JsonPropertyName("temperature_2m")]
    public string Temperature { get; set; }
    [JsonPropertyName("wind_speed_10m")]
    public string WindSpeed { get; set; } 
}

[JsonSerializable(typeof(WeatherForecast))]
public partial class WeatherForecastContext : JsonSerializerContext
{
}