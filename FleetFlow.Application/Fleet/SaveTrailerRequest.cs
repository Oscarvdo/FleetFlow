namespace FleetFlow.Application.Fleet;

public sealed class SaveTrailerRequest
{
    public long? TrailerId { get; init; }
    public string UnitNumber { get; init; } = string.Empty;
    public string Vin { get; init; } = string.Empty;
    public string TrailerType { get; init; } = string.Empty;
    public string LicensePlate { get; init; } = string.Empty;
    public string LicenseState { get; init; } = string.Empty;
    public decimal MaxPayloadLbs { get; init; }
    public string StatusCode { get; init; } = "AVAILABLE";
    public byte[]? ExpectedRowVersion { get; init; }
}

public sealed class TrailerCommandResult
{
    public long TrailerId { get; init; }
    public byte[] RowVersion { get; init; } = [];
}
