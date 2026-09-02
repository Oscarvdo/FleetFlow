namespace FleetFlow.Application.Fleet;

public sealed class FleetOverview
{
    public FleetSummary Summary { get; set; } = new();
    public IReadOnlyList<VehicleListItem> Vehicles { get; set; } = [];
    public IReadOnlyList<TrailerListItem> Trailers { get; set; } = [];
    public IReadOnlyList<DriverListItem> Drivers { get; set; } = [];
}

public sealed class FleetSummary
{
    public int TotalVehicles { get; init; }
    public int AvailableVehicles { get; init; }
    public int VehiclesInMaintenance { get; init; }
    public int TotalTrailers { get; init; }
    public int TotalDrivers { get; init; }
    public int AvailableDrivers { get; init; }
}

public sealed class VehicleListItem
{
    public long VehicleId { get; init; }
    public string UnitNumber { get; init; } = string.Empty;
    public string Vin { get; init; } = string.Empty;
    public short ModelYear { get; init; }
    public string Make { get; init; } = string.Empty;
    public string Model { get; init; } = string.Empty;
    public string LicensePlate { get; init; } = string.Empty;
    public string LicenseState { get; init; } = string.Empty;
    public decimal MaxPayloadLbs { get; init; }
    public decimal CurrentOdometerMiles { get; init; }
    public string StatusCode { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public long? ActiveTripId { get; init; }
    public string? ActiveTripNumber { get; init; }
    public DateTime UpdatedAtUtc { get; init; }
    public byte[] RowVersion { get; init; } = [];
    public string Description => $"{ModelYear} {Make} {Model}";
}

public sealed class TrailerListItem
{
    public long TrailerId { get; init; }
    public string UnitNumber { get; init; } = string.Empty;
    public string Vin { get; init; } = string.Empty;
    public string TrailerType { get; init; } = string.Empty;
    public string LicensePlate { get; init; } = string.Empty;
    public string LicenseState { get; init; } = string.Empty;
    public decimal MaxPayloadLbs { get; init; }
    public string StatusCode { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public long? ActiveTripId { get; init; }
    public string? ActiveTripNumber { get; init; }
    public DateTime UpdatedAtUtc { get; init; }
    public byte[] RowVersion { get; init; } = [];
}

public sealed class DriverListItem
{
    public long DriverId { get; init; }
    public string DriverNumber { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string? Phone { get; init; }
    public string? Email { get; init; }
    public string LicenseNumber { get; init; } = string.Empty;
    public string LicenseState { get; init; } = string.Empty;
    public DateTime LicenseExpirationDate { get; init; }
    public string StatusCode { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public long? ActiveTripId { get; init; }
    public string? ActiveTripNumber { get; init; }
    public DateTime UpdatedAtUtc { get; init; }
    public byte[] RowVersion { get; init; } = [];
}
