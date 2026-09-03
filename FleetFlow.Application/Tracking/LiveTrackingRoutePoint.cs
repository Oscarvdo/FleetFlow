namespace FleetFlow.Application.Tracking;

public sealed class LiveTrackingRoutePoint
{
    public long TripRoutePointId { get; init; }

    public long TripId { get; init; }

    public string TripNumber { get; init; } = string.Empty;

    public int PointSequence { get; init; }

    public decimal Latitude { get; init; }

    public decimal Longitude { get; init; }

    public decimal CumulativeDistanceMiles { get; init; }

    public int ExpectedOffsetSeconds { get; init; }

    public string? Instruction { get; init; }

    public string DataOriginCode { get; init; } = string.Empty;
}