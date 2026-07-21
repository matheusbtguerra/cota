namespace Cota.Domain;

public interface ITelemetryClient
{
    Task<RiverReading?> GetLatestReadingAsync(CancellationToken ct = default);
}
