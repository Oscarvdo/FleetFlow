namespace FleetFlow.Application.Trips;

public sealed class TripDetailsResult
{
    public required TripDetails Trip { get; init; }

    public IReadOnlyList<TripStopItem> Stops { get; init; } =
        [];

    public IReadOnlyList<TripStatusTimelineItem> StatusTimeline
    { get; init; } = [];
}