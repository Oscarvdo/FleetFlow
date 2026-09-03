namespace FleetFlow.Application.Tracking;

public sealed class VehicleTelemetryUpdate
{
    public int ClientRowId { get; init; }

    public Guid TelemetryId { get; init; } = Guid.NewGuid();

    public long VehicleId { get; init; }

    public long? TripId { get; init; }

    public DateTime RecordedAtUtc { get; init; }

    public long? SequenceNumber { get; init; }

    public decimal Latitude { get; init; }

    public decimal Longitude { get; init; }

    public decimal? SpeedMph { get; init; }

    public decimal? FuelPercent { get; init; }

    public decimal? OdometerMiles { get; init; }

    public decimal? HeadingDegrees { get; init; }

    public byte DataOriginId { get; init; } = 3;

    public long? ImportBatchId { get; init; }

    public long? SimulationRunId { get; init; }

    public string? PayloadJson { get; init; }
}