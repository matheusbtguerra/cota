using Cota.Domain;

namespace Cota.Infrastructure;


public class FakeTelemetryClient : ITelemetryClient
{
    private static readonly Random rng = new();
    private decimal _level = 1.20m;

    public Task<RiverReading?> GetLatestReadingAsync(CancellationToken ct = default)
    {
        // Simulate a random walk for the river level
        _level = Math.Clamp(_level + (decimal)(rng.NextDouble() - 0.48) * 0.8m, 0.3m, 5.5m);

        var reading = new RiverReading(
            Math.Round(_level, 2),
            DateTimeOffset.UtcNow,
            "Fake Station Cais Mauá");

        return Task.FromResult<RiverReading?>(reading);
    }
}
