using Cota.Api;
using Cota.Domain;

namespace Cota.Api;

public class RiverLevelWorker(
    ITelemetryClient telemetry,
    LatestReadingStore store,
    ILogger<RiverLevelWorker> logger) : BackgroundService
{
   protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await FetchAsync(stoppingToken);

        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(30));
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await FetchAsync(stoppingToken);
        }
    }

    private async Task FetchAsync(CancellationToken ct)
    {
        try
        {
            var reading = await telemetry.GetLatestReadingAsync(ct);
            if (reading is not null)
            {
                store.Update(reading);
                logger.LogInformation("River level: {Level}m at {Time}",
                    reading.LevelMeters, reading.MeasuredAt);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch river level");
        }
    }
}
