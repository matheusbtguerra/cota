using Cota.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Cota.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController(IWeatherClient weatherClient, IMemoryCache cache) : ControllerBase
{
    [HttpGet("forecast")]
    public async Task<IActionResult> GetForecast(CancellationToken ct)
    {
        if (!cache.TryGetValue("rain-forecast", out RainForecast? forecast))
        {
            forecast = await weatherClient.GetRainForecastAsync(ct);

            if (forecast is not null)
                cache.Set("rain-forecast", forecast, TimeSpan.FromMinutes(30));
        }

        if (forecast is null)
            return StatusCode(503, new { message = "Forecast unavailable" });

        return Ok(new
        {
            totalNext7DaysMm = forecast.TotalMm,
            days = forecast.Days,
            fetchedAt = forecast.FetchedAt
        });
    }
}