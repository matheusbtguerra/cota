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

        return Ok(new
        {
            levelMeters = reading.LevelMeters,
            status = RiverStatusRules.FromLevel(reading.LevelMeters).ToString(),
            measuredAt = reading.MeasuredAt,
            station = reading.StationName
        });
    }
}