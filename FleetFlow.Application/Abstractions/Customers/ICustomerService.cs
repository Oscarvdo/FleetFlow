using FleetFlow.Application.Customers;

namespace FleetFlow.Application.Abstractions.Customers;

public interface ICustomerService
{
    Task<IReadOnlyList<CustomerListItem>> SearchAsync(
        string? searchText = null,
        bool includeInactive = false,
        CancellationToken cancellationToken = default);

    Task<CustomerDetails?> GetByIdAsync(
        long customerId,
        CancellationToken cancellationToken = default);

    Task<CustomerCommandResult> SaveAsync(
        SaveCustomerRequest request,
        CancellationToken cancellationToken = default);

    Task<CustomerCommandResult> SetActiveAsync(
        long customerId,
        bool isActive,
        byte[] expectedRowVersion,
        CancellationToken cancellationToken = default);

    Task<CustomerLocationCommandResult> SaveLocationAsync(
        SaveCustomerLocationRequest request,
        CancellationToken cancellationToken = default);

    Task<CustomerLocationCommandResult> SetLocationActiveAsync(
        long customerId,
        long locationId,
        bool isActive,
        byte[] expectedRowVersion,
        CancellationToken cancellationToken = default);
}
