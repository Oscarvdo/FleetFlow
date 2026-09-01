using FleetFlow.Application.Loads;

namespace FleetFlow.Application.Abstractions.Loads;

/// <summary>
/// Define la operación necesaria para consultar
/// los detalles completos de una carga.
/// </summary>
public interface ILoadDetailsService
{
    /// <summary>
    /// Busca una carga mediante su identificador.
    /// Devuelve null cuando la carga no existe.
    /// </summary>
    Task<LoadDetails?> GetByIdAsync(
        long loadId,
        CancellationToken cancellationToken = default);
}