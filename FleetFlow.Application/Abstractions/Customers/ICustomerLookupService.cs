using FleetFlow.Application.Customers;

namespace FleetFlow.Application.Abstractions.Customers;

/// <summary>
/// Proporciona clientes disponibles para selección
/// dentro de los formularios de FleetFlow.
/// </summary>
public interface ICustomerLookupService
{
    /// <summary>
    /// Busca clientes por número o nombre.
    ///
    /// De forma predeterminada devuelve solamente clientes
    /// activos. La edición puede incluir clientes inactivos
    /// para conservar correctamente relaciones históricas.
    /// </summary>
    Task<IReadOnlyList<CustomerLookupItem>>
        SearchAsync(
            string? searchText = null,
            bool includeInactive = false,
            CancellationToken cancellationToken = default);
}