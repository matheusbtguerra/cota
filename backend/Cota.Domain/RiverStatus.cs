namespace Cota.Domain;

public enum RiverStatus
{
    Normal,
    Attention,
    Alert,
    Flood
}

public static class RiverStatusRules
{
    public const decimal AttentionThreshold = 2.0m;
    public const decimal AlertThreshold = 2.5m;
    public const decimal FloodThreshold = 3.0m;

    public static RiverStatus FromLevel(decimal levelMeters) => levelMeters switch
    {
        >= FloodThreshold => RiverStatus.Flood,
        >= AlertThreshold => RiverStatus.Alert,
        >= AttentionThreshold => RiverStatus.Attention,
        _ => RiverStatus.Normal
    };
}