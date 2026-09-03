namespace FleetFlow.Application.Tracking;

public sealed class LiveTrackingSimulationRun
{
    public long SimulationRunId { get; init; }

    public Guid SimulationRunUid { get; init; }

    public string Name { get; init; } = string.Empty;

    public string ScenarioCode { get; init; } = string.Empty;

    public string Status { get; init; } = string.Empty;

    public int RandomSeed { get; init; }

    public decimal TimeScale { get; init; }

    public int UpdateIntervalMilliseconds { get; init; }

    public int PlannedVehicleCount { get; init; }

    public string? ConfigurationJson { get; init; }

    public DateTime? StartedAtUtc { get; init; }

    public DateTime? EndedAtUtc { get; init; }

    public DateTime CreatedAtUtc { get; init; }

    public long TelemetryRows { get; init; }

    public int ActualVehicleCount { get; init; }

    public bool IsRunning =>
        string.Equals(
            Status,
            "RUNNING",
            StringComparison.OrdinalIgnoreCase);

    public bool IsPaused =>
        string.Equals(
            Status,
            "PAUSED",
            StringComparison.OrdinalIgnoreCase);

    public bool IsTerminal =>
        string.Equals(
            Status,
            "COMPLETED",
            StringComparison.OrdinalIgnoreCase)
        ||
        string.Equals(
            Status,
            "FAILED",
            StringComparison.OrdinalIgnoreCase)
        ||
        string.Equals(
            Status,
            "CANCELLED",
            StringComparison.OrdinalIgnoreCase);

    public string DisplayName =>
        $"{Name} — {Status}";
}