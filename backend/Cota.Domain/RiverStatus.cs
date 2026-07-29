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
    /// <summary>
    /// Determines the status of a river based on its current level and the thresholds of a specific station.
    /// </summary>
    /// <param name="levelMeters">The current level of the river in meters.</param>
    /// <param name="t">The thresholds for the station.</param>
    /// <returns>The status of the river.</returns>
    public static RiverStatus FromLevel(decimal levelMeters, RiverThresholds t) => levelMeters switch
    {
        var l when l >= t.FloodMeter => RiverStatus.Flood,
        var l when l >= t.AlertMeter    => RiverStatus.Alert,
        var l when l >= t.AttentionMeter   => RiverStatus.Attention,
        _                                  => RiverStatus.Normal
    };
}