namespace Cota.Domain;

public record RiverReading (
    decimal LevelMeters,
    DateTimeOffset MesuredAt,
    String StationName);