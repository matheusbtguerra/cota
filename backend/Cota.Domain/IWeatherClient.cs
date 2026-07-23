namespace Cota.Domain;

public interface IWeatherClient
{
    Task<RainForecast?> GetRainForecastAsync(CancellationToken cancellationToken = default);
}