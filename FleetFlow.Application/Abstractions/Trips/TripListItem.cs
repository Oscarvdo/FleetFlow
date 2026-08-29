namespace FleetFlow.Application.Trips;

public sealed class TripListItem
{
    public long TripId { get; init; }

    public string TripNumber { get; init; } = string.Empty;

    public long LoadId { get; init; }

    public string LoadNumber { get; init; } = string.Empty;

    public long CustomerId { get; init; }

    public string CustomerNumber { get; init; } = string.Empty;

    public string Customer { get; init; } = string.Empty;

    public string TripStatusCode { get; init; } = string.Empty;

    public string TripStatus { get; init; } = string.Empty;

    public DateTime ScheduledPickupUtc { get; init; }

    public DateTime ScheduledDeliveryUtc { get; init; }

    public DateTime? ActualStartUtc { get; init; }

    public DateTime? ActualDeliveryUtc { get; init; }

    public decimal? PlannedDistanceMiles { get; init; }

    public decimal? ActualDistanceMiles { get; init; }

    public long TotalStops { get; init; }

    public long CompletedStops { get; init; }

    public decimal ProgressPercent { get; init; }

    public byte[] RowVersion { get; init; } = [];
}