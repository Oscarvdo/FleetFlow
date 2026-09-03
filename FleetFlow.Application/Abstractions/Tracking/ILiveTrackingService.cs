using FleetFlow.Application.Tracking;

namespace FleetFlow.Application.Abstractions.Tracking;

public interface ILiveTrackingService
{
    Task<IReadOnlyList<LiveTrackingVehicleItem>> GetMapStateAsync(
        bool includeOffline = true,
        int offlineAfterSeconds = 60,
        long? simulationRunId = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<LiveTrackingRoutePoint>> GetTripRouteAsync(
        long tripId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<LiveTrackingStopItem>> GetTripStopsAsync(
        long tripId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<LiveTrackingSimulationCandidate>>
        GetSimulationCandidatesAsync(
            CancellationToken cancellationToken = default);

    Task<IReadOnlyList<LiveTrackingSimulationRun>>
        GetSimulationRunsAsync(
            bool includeCompleted = false,
            CancellationToken cancellationToken = default);
}