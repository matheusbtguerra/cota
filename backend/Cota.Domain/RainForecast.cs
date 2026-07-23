namespace Cota.Domain;

public record DailyRain(DateOnly Date, decimal PrecipitationMm);

public record RainForecast(IReadOnlyList<DailyRain> Days, DateTimeOffset FetchedAt)
{
    public decimal TotalMm => Days.Sum(d => d.PrecipitationMm);
}