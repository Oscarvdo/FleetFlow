namespace FleetFlow.Application.Loads;

/// <summary>
/// Contiene la información completa de una carga,
/// su cliente y el viaje relacionado cuando exista.
/// </summary>
public sealed class LoadDetails
{
    // Identificación de la carga.
    public long LoadId { get; init; }

    public string LoadNumber { get; init; } =
        string.Empty;

    // Información del cliente.
    public long CustomerId { get; init; }

    public string CustomerNumber { get; init; } =
        string.Empty;

    public string Customer { get; init; } =
        string.Empty;

    public string? CustomerContactName { get; init; }

    public string? CustomerEmail { get; init; }

    public string? CustomerPhone { get; init; }

    // Información comercial y operativa de la carga.
    public string Description { get; init; } =
        string.Empty;

    public string? Commodity { get; init; }

    public decimal WeightLbs { get; init; }

    public int? Pieces { get; init; }

    public decimal? RevenueAmount { get; init; }

    public string? SpecialInstructions { get; init; }

    public string LoadStatusCode { get; init; } =
        string.Empty;

    public string LoadStatus { get; init; } =
        string.Empty;

    // El viaje es opcional porque una carga nueva
    // todavía puede no haber sido planificada.
    public long? TripId { get; init; }

    public string? TripNumber { get; init; }

    public string? TripStatusCode { get; init; }

    public string? TripStatus { get; init; }

    public DateTime? ScheduledPickupUtc { get; init; }

    public DateTime? ScheduledDeliveryUtc { get; init; }

    // Información de auditoría.
    public DateTime CreatedAtUtc { get; init; }

    public DateTime UpdatedAtUtc { get; init; }

    // SQL Server utiliza RowVersion para detectar
    // modificaciones concurrentes.
    public byte[] RowVersion { get; init; } = [];
}