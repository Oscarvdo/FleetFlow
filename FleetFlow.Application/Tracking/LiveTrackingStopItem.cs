namespace FleetFlow.Application.Tracking;

public sealed class LiveTrackingStopItem
{
    public long TripStopId { get; init; }

    public long TripId { get; init; }

    public short StopSequence { get; init; }

    public string StopTypeCode { get; init; } = string.Empty;

    public string StopType { get; init; } = string.Empty;

    public string StopStatusCode { get; init; } = string.Empty;

    public string StopStatus { get; init; } = string.Empty;

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

    public DateTime? ScheduledArrivalUtc { get; init; }

    public DateTime? ScheduledDepartureUtc { get; init; }

    public DateTime? ActualArrivalUtc { get; init; }

    public DateTime? ActualDepartureUtc { get; init; }

    public string? Instructions { get; init; }

    public bool HasPosition =>
        Latitude.HasValue &&
        Longitude.HasValue;

    public string FullAddress
    {
        get
        {
            List<string> parts = new();

            if (!string.IsNullOrWhiteSpace(Address1))
            {
                parts.Add(Address1.Trim());
            }

            if (!string.IsNullOrWhiteSpace(Address2))
            {
                parts.Add(Address2.Trim());
            }

            string cityStatePostal =
                $"{City}, {StateCode} {PostalCode}".Trim();

            if (!string.IsNullOrWhiteSpace(cityStatePostal))
            {
                parts.Add(cityStatePostal);
            }

            return string.Join(", ", parts);
        }
    }
}