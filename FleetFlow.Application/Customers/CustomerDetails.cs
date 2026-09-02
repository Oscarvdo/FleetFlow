namespace FleetFlow.Application.Customers;

public sealed class CustomerDetails
{
    public long CustomerId { get; init; }
    public string CustomerNumber { get; init; } = string.Empty;
    public string CompanyName { get; init; } = string.Empty;
    public string? ContactName { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime UpdatedAtUtc { get; init; }
    public byte[] RowVersion { get; init; } = [];
    public int LoadCount { get; init; }
    public int OpenLoadCount { get; init; }
    public decimal TotalRevenueAmount { get; init; }
    public IReadOnlyList<CustomerLocationItem> Locations { get; set; } = [];
    public IReadOnlyList<CustomerRecentLoadItem> RecentLoads { get; set; } = [];
}
