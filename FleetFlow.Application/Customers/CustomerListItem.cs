namespace FleetFlow.Application.Customers;

public sealed class CustomerListItem
{
    public long CustomerId { get; init; }
    public string CustomerNumber { get; init; } = string.Empty;
    public string CompanyName { get; init; } = string.Empty;
    public string? ContactName { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public bool IsActive { get; init; }
    public int LocationCount { get; init; }
    public int LoadCount { get; init; }
    public DateTime? LastLoadAtUtc { get; init; }
    public DateTime UpdatedAtUtc { get; init; }
    public byte[] RowVersion { get; init; } = [];
}
