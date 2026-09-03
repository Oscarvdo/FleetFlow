using FleetFlow.Application.Tracking;

namespace FleetFlow.Application.Abstractions.Tracking;

public interface ILiveTrackingSimulationRunService
{
    Task<SimulationRunCommandResult> CreateAsync(
        CreateSimulationRunRequest request,
        CancellationToken cancellationToken = default);

    Task<LiveTrackingSimulationRun> SetStatusAsync(
        long simulationRunId,
        string status,
        CancellationToken cancellationToken = default);
}