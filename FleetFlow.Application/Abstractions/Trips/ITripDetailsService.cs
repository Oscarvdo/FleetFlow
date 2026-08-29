using FleetFlow.Application.Trips;

namespace FleetFlow.Application.Abstractions.Trips;

public interface ITripDetailsService
{
    Task<TripDetailsResult?> GetByIdAsync(
        long tripId,
        CancellationToken cancellationToken = default);
}