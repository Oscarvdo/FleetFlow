namespace FleetFlow.Application.Loads;

/// <summary>
/// Contiene la información comercial necesaria
/// para actualizar una carga existente.
/// </summary>
public sealed class UpdateLoadRequest
{
    /// <summary>
    /// Identificador de la carga que será actualizada.
    /// </summary>
    public long LoadId { get; init; }

    /// <summary>
    /// Número comercial de la carga.
    /// </summary>
    public string LoadNumber { get; init; } =
        string.Empty;

    /// <summary>
    /// Cliente relacionado con la carga.
    /// </summary>
    public long CustomerId { get; init; }

    /// <summary>
    /// Descripción general del embarque.
    /// </summary>
    public string Description { get; init; } =
        string.Empty;

    /// <summary>
    /// Tipo de mercancía transportada.
    /// </summary>
    public string? Commodity { get; init; }

    /// <summary>
    /// Peso total expresado en libras.
    /// </summary>
    public decimal WeightLbs { get; init; }

    /// <summary>
    /// Número opcional de piezas.
    /// </summary>
    public int? Pieces { get; init; }

    /// <summary>
    /// Ingreso esperado por la carga.
    /// </summary>
    public decimal? RevenueAmount { get; init; }

    /// <summary>
    /// Instrucciones especiales de manejo o entrega.
    /// </summary>
    public string? SpecialInstructions { get; init; }

    /// <summary>
    /// Versión que tenía la carga cuando fue consultada.
    /// SQL Server la utiliza para detectar modificaciones
    /// realizadas por otra operación.
    /// </summary>
    public byte[] ExpectedRowVersion { get; init; } = [];
}