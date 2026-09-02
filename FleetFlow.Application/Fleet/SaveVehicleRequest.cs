namespace FleetFlow.Application.Fleet;

public sealed class SaveVehicleRequest
{
    public long? VehicleId { get; init; }
    public string UnitNumber { get; init; } = string.Empty;
    public string Vin { get; init; } = string.Empty;
    public short ModelYear { get; init; }
    public string Make { get; init; } = string.Empty;
    public string Model { get; init; } = string.Empty;
    public string LicensePlate { get; init; } = string.Empty;
    public string LicenseState { get; init; } = string.Empty;
    public decimal MaxPayloadLbs { get; init; }
    public decimal CurrentOdometerMiles { get; init; }
    public string StatusCode { get; init; } = "AVAILABLE";
    public byte[]? ExpectedRowVersion { get; init; }
}

public sealed class VehicleCommandResult
{
    public long VehicleId { get; init; }
    public byte[] RowVersion { get; init; } = [];
}
