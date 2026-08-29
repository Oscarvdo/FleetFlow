namespace FleetFlow.Application.Trips;

public sealed class TripStatusTimelineItem
{
    public long TripStatusHistoryId { get; init; }

    public long TripId { get; init; }

    public string TripNumber { get; init; } = string.Empty;

    public string? PreviousStatusCode { get; init; }

    public string NewStatusCode { get; init; } = string.Empty;

    public DateTime ChangedAtUtc { get; init; }

    public string ChangedBy { get; init; } = string.Empty;

    public string Source { get; init; } = string.Empty;

    public string? Notes { get; init; }
}