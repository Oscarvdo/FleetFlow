using FleetFlow.Application.Loads;

namespace FleetFlow.Application.Abstractions.Loads;

public interface ILoadListService
{
    Task<IReadOnlyList<LoadListItem>> GetLoadsAsync(
        string? statusCode = null,
        string? searchText = null,
        CancellationToken cancellationToken = default);
}