namespace FleetFlow.Application.Loads;

public sealed class LoadListItem
{
    public long LoadId { get; init; }

    public string LoadNumber { get; init; } =  string.Empty;

    public long CustomerId { get; init; }

    public string CustomerNumber { get; init; } =    string.Empty;

    public string Customer { get; init; } =
        string.Empty;

    public string Description { get; init; } =
        string.Empty;

    public string? Commodity { get; init; }

    public decimal WeightLbs { get; init; }

    public int? Pieces { get; init; }

    public decimal? RevenueAmount { get; init; }

    public string LoadStatusCode { get; init; } =
        string.Empty;

    public string LoadStatus { get; init; } =
        string.Empty;

    public long? TripId { get; init; }

    public string? TripNumber { get; init; }

    public DateTime? ScheduledPickupUtc { get; init; }

    public DateTime? ScheduledDeliveryUtc { get; init; }

    public DateTime CreatedAtUtc { get; init; }

    public DateTime UpdatedAtUtc { get; init; }

    public byte[] RowVersion { get; init; } = [];
}