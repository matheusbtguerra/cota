using Cota.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Cota.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RiverController(LatestReadingStore store) : ControllerBase
{
    [HttpGet("current")]
    public IActionResult GetCurrent()
    {
        var reading = store.Latest;
        if (reading is null)
        {
            return StatusCode(503, new { message = "No reading available yet" });
        }

        var station = KnownStations.Guaiba;
        var status = RiverStatusRules.FromLevel(reading.LevelMeters, station.Thresholds);

        return Ok(new
        {
            levelMeters = reading.LevelMeters,
            status = status.ToString(),
            measuredAt = reading.MeasuredAt,
            station = new
            {
                code = station.Code,
                name = station.Name,
                region = station.RegionName
            },
            thresholds = new
            {
                attention = station.Thresholds.AttentionMeters,
                alert = station.Thresholds.AlertMeters,
                flood = station.Thresholds.FloodMeters
            }
        });
    }
}