namespace FleetFlow.Application.Customers;

public sealed class CustomerCommandResult
{
    public long CustomerId { get; init; }
    public byte[] RowVersion { get; init; } = [];
}
