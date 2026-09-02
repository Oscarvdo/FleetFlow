namespace FleetFlow.Application.Customers;

public sealed class SaveCustomerRequest
{
    public long? CustomerId { get; init; }
    public string CustomerNumber { get; init; } = string.Empty;
    public string CompanyName { get; init; } = string.Empty;
    public string? ContactName { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public byte[]? ExpectedRowVersion { get; init; }
}
