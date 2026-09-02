namespace FleetFlow.Application.Customers;

public sealed class CustomerLocationItem
{
    public long LocationId { get; init; }
    public string LocationCode { get; init; } = string.Empty;
    public string LocationType { get; init; } = string.Empty;
    public string LocationName { get; init; } = string.Empty;
    public string Address1 { get; init; } = string.Empty;
    public string? Address2 { get; init; }
    public string City { get; init; } = string.Empty;
    public string StateCode { get; init; } = string.Empty;
    public string PostalCode { get; init; } = string.Empty;
    public decimal? Latitude { get; init; }
    public decimal? Longitude { get; init; }
    public string? ContactName { get; init; }
    public string? ContactPhone { get; init; }
    public bool IsBillingLocation { get; init; }
    public bool IsActive { get; init; }
    public byte[] RowVersion { get; init; } = [];

    public string Address => string.Join(", ",
        new[] { Address1, Address2, City, StateCode, PostalCode }
            .Where(value => !string.IsNullOrWhiteSpace(value)));
}
