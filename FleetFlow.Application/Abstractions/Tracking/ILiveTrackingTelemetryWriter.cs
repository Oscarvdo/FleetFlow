using FleetFlow.Application.Tracking;

namespace FleetFlow.Application.Abstractions.Tracking;

public interface ILiveTrackingTelemetryWriter
{
    Task<TelemetryBatchResult> AppendBatchAsync(
        IReadOnlyCollection<VehicleTelemetryUpdate> telemetry,
        CancellationToken cancellationToken = default);
}