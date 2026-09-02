namespace FleetFlow.Application.Customers;

public sealed class CustomerLocationCommandResult
{
    public long LocationId { get; init; }
    public byte[] RowVersion { get; init; } = [];
}
