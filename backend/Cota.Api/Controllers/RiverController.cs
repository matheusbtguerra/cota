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

        // Each station has its own thresholds; V1 uses the Guaíba station's thresholds
        var thresholds = KnownStations.Guaiba.Thresholds;
        var status = RiverStatusRules.FromLevel(reading.LevelMeters, thresholds);

        return Ok(new
        {
            levelMeters = reading.LevelMeters,
            status = status.ToString(),
            measuredAt = reading.MeasuredAt,
            station = reading.StationName
        });
    }
}