using FleetFlow.Application.Trips;

namespace FleetFlow.Application.Abstractions.Trips;

public interface ITripListService
{
    Task<IReadOnlyList<TripListItem>> GetTripsAsync(
        string? statusCode = null,
        string? searchText = null,
        CancellationToken cancellationToken = default);
}