namespace FleetFlow.Application.Loads;

/// <summary>
/// Contiene la información necesaria para registrar
/// una carga nueva en FleetFlow.
/// </summary>
public sealed class CreateLoadRequest
{
    /// <summary>
    /// Número comercial único de la carga.
    /// </summary>
    public string LoadNumber { get; init; } =
        string.Empty;

    /// <summary>
    /// Cliente activo relacionado con la carga.
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
}