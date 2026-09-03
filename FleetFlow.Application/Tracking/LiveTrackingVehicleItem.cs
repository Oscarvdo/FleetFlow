namespace FleetFlow.Application.Tracking;

public sealed class LiveTrackingVehicleItem
{
    public long VehicleId { get; init; }

    public string UnitNumber { get; init; } = string.Empty;

    public string Make { get; init; } = string.Empty;

    public string Model { get; init; } = string.Empty;

    public long? TripAssignmentId { get; init; }

    public long? TripId { get; init; }

    public string? TripNumber { get; init; }

    public string? TripStatusCode { get; init; }

    public string? TripStatus { get; init; }

    public long? LoadId { get; init; }

    public string? LoadNumber { get; init; }

    public long? CustomerId { get; init; }

    public string? CustomerName { get; init; }

    public long? DriverId { get; init; }

    public string? DriverNumber { get; init; }

    public string? DriverName { get; init; }

    public long? TrailerId { get; init; }

    public string? TrailerUnitNumber { get; init; }

    public DateTime? RecordedAtUtc { get; init; }

    public decimal? Latitude { get; init; }

    public decimal? Longitude { get; init; }

    public decimal? SpeedMph { get; init; }

    public decimal? FuelPercent { get; init; }

    public decimal? OdometerMiles { get; init; }

    public decimal? HeadingDegrees { get; init; }

    public long? SimulationRunId { get; init; }

    public int? TelemetryAgeSeconds { get; init; }

    public string TrackingStatus { get; init; } = "OFFLINE";

    public long? NextTripStopId { get; init; }

    public short? NextStopSequence { get; init; }

    public string? NextStopType { get; init; }

    public long? NextLocationId { get; init; }

    public string? NextStopName { get; init; }

    public decimal? NextStopLatitude { get; init; }

    public decimal? NextStopLongitude { get; init; }

    public DateTime? NextScheduledArrivalUtc { get; init; }

    public int? NearestRoutePointSequence { get; init; }

    public decimal? CumulativeDistanceMiles { get; init; }

    public decimal ProgressPercent { get; init; }

    public bool HasPosition =>
        Latitude.HasValue &&
        Longitude.HasValue;

    public bool IsOffline =>
        string.Equals(
            TrackingStatus,
            "OFFLINE",
            StringComparison.OrdinalIgnoreCase);

    public string VehicleDescription =>
        string.IsNullOrWhiteSpace(Make) &&
        string.IsNullOrWhiteSpace(Model)
            ? UnitNumber
            : $"{UnitNumber} — {Make} {Model}".Trim();
}