namespace Cota.Domain;

public record RiverReading (
    decimal LevelMeters,
    DateTimeOffset MeasuredAt,
    String StationName);