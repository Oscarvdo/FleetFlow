namespace FleetFlow.Application.Customers;

public sealed class SaveCustomerLocationRequest
{
    public long? LocationId { get; init; }
    public long CustomerId { get; init; }
    public string LocationCode { get; init; } = string.Empty;
    public string LocationType { get; init; } = "CUSTOMER";
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
    public byte[]? ExpectedRowVersion { get; init; }
}
