namespace FleetFlow.Application.Customers;

/// <summary>
/// Representa un cliente disponible para selección
/// dentro de los formularios de cargas.
/// </summary>
public sealed class CustomerLookupItem
{
    /// <summary>
    /// Identificador interno del cliente.
    /// </summary>
    public long CustomerId { get; init; }

    /// <summary>
    /// Número comercial del cliente,
    /// por ejemplo CUS-1001.
    /// </summary>
    public string CustomerNumber { get; init; } =
        string.Empty;

    /// <summary>
    /// Nombre comercial o razón social.
    /// </summary>
    public string CompanyName { get; init; } =
        string.Empty;

    /// <summary>
    /// Indica si el cliente puede utilizarse
    /// en nuevas operaciones.
    /// </summary>
    public bool IsActive { get; init; }

    /// <summary>
    /// Texto mostrado dentro del ComboBox.
    /// Los clientes inactivos quedan identificados.
    /// </summary>
    public string DisplayName =>
        IsActive
            ? $"{CustomerNumber} — {CompanyName}"
            : $"{CustomerNumber} — {CompanyName} (Inactive)";
}