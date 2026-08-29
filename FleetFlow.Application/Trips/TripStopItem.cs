namespace FleetFlow.Application.Trips;

public sealed class TripStopItem
{
    public long TripStopId { get; init; }

    public long TripId { get; init; }

    public string TripNumber { get; init; } = string.Empty;

    public int StopSequence { get; init; }

    public string StopTypeCode { get; init; } = string.Empty;

    public string StopStatusCode { get; init; } = string.Empty;

    public long LocationId { get; init; }

    public string LocationCode { get; init; } = string.Empty;

    public string LocationName { get; init; } = string.Empty;

    public string Address1 { get; init; } = string.Empty;

    public string? Address2 { get; init; }

    public string City { get; init; } = string.Empty;

    public string StateCode { get; init; } = string.Empty;

    public string PostalCode { get; init; } = string.Empty;

    public decimal? Latitude { get; init; }

    public decimal? Longitude { get; init; }

    public DateTime ScheduledArrivalUtc { get; init; }

    public DateTime? ScheduledDepartureUtc { get; init; }

    public DateTime? ActualArrivalUtc { get; init; }

    public DateTime? ActualDepartureUtc { get; init; }

    public string? Instructions { get; init; }

    public byte[] RowVersion { get; init; } = [];
}