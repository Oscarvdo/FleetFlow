namespace FleetFlow.Application.Tracking;

public sealed class SimulationRunCommandResult
{
    public long SimulationRunId { get; init; }

    public Guid SimulationRunUid { get; init; }
}