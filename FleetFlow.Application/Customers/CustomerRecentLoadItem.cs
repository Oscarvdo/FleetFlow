namespace FleetFlow.Application.Customers;

public sealed class CustomerRecentLoadItem
{
    public long LoadId { get; init; }
    public string LoadNumber { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string LoadStatus { get; init; } = string.Empty;
    public decimal? RevenueAmount { get; init; }
    public DateTime CreatedAtUtc { get; init; }
}
