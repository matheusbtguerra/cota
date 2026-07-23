using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Cota.Domain;

namespace Cota.Infrastructure;

public class OpenMeteoClient(HttpClient http) : IWeatherClient
{
    private const string RequestUri =
        "v1/forecast?latitude=-30.03&longitude=-51.22&daily=precipitation_sum&timezone=America%2FSao_Paulo";

    public async Task<RainForecast?> GetRainForecastAsync(CancellationToken ct = default)
    {
        var response = await http.GetFromJsonAsync<OpenMeteoResponse>(RequestUri, ct);
        if (response?.Daily is null) return null;
        
        var days = response.Daily.Time
            .Select((date, index) => new DailyRain(
                DateOnly.Parse(date),
                response.Daily.PrecipitationSum.ElementAtOrDefault(index) ?? 0m))
            .ToList();
        
        return new RainForecast(days, DateTimeOffset.UtcNow);
    }

    // Private DTOs for deserialization

    private sealed record OpenMeteoResponse(
        [property: JsonPropertyName("daily")] DailySection? Daily);

    private sealed record DailySection(
        [property: JsonPropertyName("time")] List<string> Time,
        [property: JsonPropertyName("precipitation_sum")] List<decimal?> PrecipitationSum);
    
}