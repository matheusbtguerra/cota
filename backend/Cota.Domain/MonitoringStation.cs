namespace Cota.Domain;

public record MonitoringStation (
    string Code,               //  ANA Code, ex: "87450020"
    string Name,               // "Usina do Gasômetro"
    string RegionName,         // "Porto Alegre — Guaíba"
    RiverThresholds Thresholds);

public record RiverThresholds (
    decimal AlertMeter,
    decimal AttentionMeter,
    decimal FloodMeter);
