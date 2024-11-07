namespace UsageExample.Services;

public class WeatherService
{
    private readonly List<string> _weather = ["0 degrees", "10 degrees", "20 degrees",];

    public Task<string> GetWeather()
    {
        var weather = _weather[Random.Shared.Next(0, _weather.Count)];
        return Task.FromResult(weather);
    }
}
