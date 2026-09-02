using FleetFlow.Application.Fleet;

namespace FleetFlow.Application.Abstractions.Fleet;

public interface IFleetOverviewService
{
    Task<FleetOverviewResult> GetOverviewAsync(
        bool includeInactive = false,
        CancellationToken cancellationToken = default);
}
