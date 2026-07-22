using Cota.Domain;

namespace Cota.Api;

public class LatestReadingStore
{
    private RiverReading? _latest;
    public RiverReading? Latest => _latest;
    public void Update(RiverReading reading) => _latest = reading;
}