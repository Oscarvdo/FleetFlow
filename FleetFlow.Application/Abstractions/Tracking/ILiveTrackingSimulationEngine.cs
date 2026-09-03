using FleetFlow.Application.Tracking;

namespace FleetFlow.Application.Abstractions.Tracking;

public interface ILiveTrackingSimulationEngine
    : IAsyncDisposable
{
    bool IsRunning { get; }

    bool IsPaused { get; }

    long? CurrentSimulationRunId { get; }

    int ActiveTruckCount { get; }

    event Action<IReadOnlyList<VehicleTelemetryUpdate>>?
        TelemetryProduced;

    event Action<string>?
        StatusChanged;

    Task<SimulationRunCommandResult> StartAsync(
        CreateSimulationRunRequest request,
        CancellationToken cancellationToken = default);

    Task PauseAsync(
        CancellationToken cancellationToken = default);

    Task ResumeAsync(
        CancellationToken cancellationToken = default);

    Task StopAsync(
        CancellationToken cancellationToken = default);
}