namespace FleetFlow.Application.Tracking;

public sealed class LiveTrackingSimulationCandidate
{
    public long TripAssignmentId { get; init; }

    public long TripId { get; init; }

    public string TripNumber { get; init; } = string.Empty;

    public long VehicleId { get; init; }

    public string UnitNumber { get; init; } = string.Empty;

    public long DriverId { get; init; }

    public string DriverNumber { get; init; } = string.Empty;

    public string DriverName { get; init; } = string.Empty;

    public long? TrailerId { get; init; }

    public string? TrailerUnitNumber { get; init; }

    public decimal? PlannedDistanceMiles { get; init; }

    public DateTime ScheduledPickupUtc { get; init; }

    public DateTime ScheduledDeliveryUtc { get; init; }

    public string TripStatusCode { get; init; } = string.Empty;

    public string TripStatus { get; init; } = string.Empty;

    public int RoutePointCount { get; init; }

    public int FirstPointSequence { get; init; }

    public decimal FirstLatitude { get; init; }

    public decimal FirstLongitude { get; init; }

    public int LastPointSequence { get; init; }

    public decimal LastLatitude { get; init; }

    public decimal LastLongitude { get; init; }

    public string DisplayName =>
        $"{UnitNumber} — {TripNumber}";

    public bool HasValidRoute =>
        RoutePointCount >= 2 &&
        FirstLatitude is >= -90M and <= 90M &&
        FirstLongitude is >= -180M and <= 180M &&
        LastLatitude is >= -90M and <= 90M &&
        LastLongitude is >= -180M and <= 180M;
}