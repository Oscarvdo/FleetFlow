namespace FleetFlow.Application.Tracking;

public sealed class CreateSimulationRunRequest
{
    public string Name { get; init; } =
        "FleetFlow Live Tracking Simulation";

    public string ScenarioCode { get; init; } =
        "NORMAL_OPERATION";

    public int RandomSeed { get; init; } =
        Environment.TickCount;

    public decimal TimeScale { get; init; } = 10M;

    public int UpdateIntervalMilliseconds { get; init; } = 1000;

    public int PlannedVehicleCount { get; init; } = 1;

    public string? ConfigurationJson { get; init; }

    public long? CreatedByAppUserId { get; init; }
}