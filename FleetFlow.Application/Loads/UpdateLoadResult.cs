namespace FleetFlow.Application.Loads;

/// <summary>
/// Representa el resultado devuelto por SQL Server
/// después de actualizar una carga.
/// </summary>
public sealed class UpdateLoadResult
{
    /// <summary>
    /// Identificador de la carga actualizada.
    /// </summary>
    public long LoadId { get; init; }

    /// <summary>
    /// Nueva versión generada automáticamente
    /// por SQL Server después de la actualización.
    /// </summary>
    public byte[] RowVersion { get; init; } = [];
}